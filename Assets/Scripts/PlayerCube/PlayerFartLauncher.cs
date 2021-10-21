using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Qbism.SpriteAnimations;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerFartLauncher : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float fartForce;
		[SerializeField] LayerMask fartRayCastLayers;
		[SerializeField] Transform fartVFXParent;
		[SerializeField] GameObject playerVis;
		[SerializeField] float flyByScaleMod, flyBySpeedMod;	
		[SerializeField] float beamImpactAddedY = .55f;
		[SerializeField] float camDisForCamSwitch = 7f;

		//Cache
		GameControls controls;
		ExpressionHandler exprHandler;
		PlayerFartJuicer juicer;

		//States
		bool fartCollided = false;
		bool fartCounting = false;
		int fartCount = 0;
		ParticleSystem currentFartImpact = null;
		bool endCam = false;
		Vector3 originalScale;
		float[] beamOriginalMins;
		float[] beamOriginalMaxs;
		float beamOriginalMin;
		float beamOriginalMax;
		ParticleSystem[] beamParticles;
		bool impactOnFinish = false;

		//Actions, events, delegates etc
		public event Action onDoneFarting;
		public event Action onStartFarting;
		public event Action onSwitchToEndCam;
		public event Action onMoveCam;
		public event Action<bool> onSwitchVisuals;

		public delegate Vector3 FinishPosFetchDel();
		public FinishPosFetchDel onCheckFinishPos;

		private void Awake()
		{
			exprHandler = GetComponentInChildren<ExpressionHandler>();
			juicer = GetComponent<PlayerFartJuicer>();
		}

		private void Start()
		{
			SaveOriginalScales();
		}

		private void Update()
		{
			HandleFartCounting();
			KeepImpactOnFinishPos();
		}

		private void KeepImpactOnFinishPos()
		{
			if (impactOnFinish)
			{
				var glowFinishPos = onCheckFinishPos();
				var impactPos = new Vector3(glowFinishPos.x,
					glowFinishPos.y + beamImpactAddedY, glowFinishPos.z);
				currentFartImpact.transform.position = impactPos;
			}
		}

		private void HandleFartCounting() //Not sure why this
		{
			if (fartCounting) fartCount++;
			if (fartCount > 10) StopFartHit();
		}

		public void InitiateFartSequence(Transform target)
		{
			StartCoroutine(FartSequence(target));
		}

		private IEnumerator FartSequence(Transform target)
		{
			transform.parent = null;

			juicer.PreFartJuice();
			exprHandler.SetFace(Expressions.pushing, -1);
			float feedbackDuration = juicer.preFartMMWiggle.WigglePositionDuration;
			yield return new WaitForSeconds(feedbackDuration);

			exprHandler.SetFace(Expressions.shocked, -1);
			onMoveCam();

			yield return new WaitForSeconds(.5f);

			exprHandler.SetFace(Expressions.gleeful, -1);
			juicer.BeamFartJuice();
			onStartFarting();
			StartCoroutine(LaunchPlayer(target));
		}

		public void StartBeamImpact()
		{
			currentFartImpact = juicer.fartBeamImpact;
			ProcessFartRaycast(true);
		}

		public void StartLaserBulletImpact()
		{
			currentFartImpact = juicer.bulletFartImpact;
			ProcessFartRaycast(false);
		}

		private void ProcessFartRaycast(bool isBeam)
		{
			bool hasHit = false;
			RaycastHit hit = FireRayCast(out hasHit);
			
			if (hasHit)
			{
				if(!fartCollided)
				{
					currentFartImpact.transform.parent = null;

					if (isBeam) impactOnFinish = true;
					else currentFartImpact.transform.position = 
						Vector3.Lerp(hit.point, transform.position, .05f);
					
					currentFartImpact.Play();
					fartCollided = true;
				}

				fartCount = 0;
				fartCounting = true;
			}
		}

		public RaycastHit FireRayCast(out bool hasHit)
		{
			//returning hasHit bool for a raycast check done in laser cube
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.forward, out hit, 20, 
				fartRayCastLayers, QueryTriggerInteraction.Ignore))
				hasHit = true;

			else hasHit = false;

			return hit;
		}

		public void FireBulletFart()
		{
			juicer.BulletFartJuice();
			exprHandler.SetSituationFace(ExpressionSituations.fart, .75f);
		}

		private void StopFartHit()
		{
			impactOnFinish = false;
			var module = currentFartImpact.main;
			if (module.loop) currentFartImpact.Stop();
			currentFartImpact.transform.parent = fartVFXParent;
			fartCounting = false;
		}

		private IEnumerator LaunchPlayer(Transform target)
		{
			float step = fartForce * Time.deltaTime;

			while (Vector3.Distance(transform.position, target.position) > 0.1f)
			{
				transform.position = Vector3.MoveTowards(transform.position, target.position, step);

				if(Vector3.Distance(transform.position, Camera.main.transform.position) < 
					camDisForCamSwitch && !endCam)
				{
					onSwitchToEndCam();
					onDoneFarting();
					endCam = true;
				} 

				yield return null;
			}

			transform.position = target.position;
			onSwitchVisuals(false);
			juicer.StopBeamFartJuice();
		}

		public void InitiateFlyBy(Vector3 startPos, Vector3 endPos)
		{
			StartCoroutine(FlyBy(startPos, endPos));
		}

		private IEnumerator FlyBy(Vector3 startPos, Vector3 endPos)
		{
			var step = fartForce * flyBySpeedMod * Time.deltaTime;

			transform.position = startPos;
			transform.LookAt(transform.position - (endPos - transform.position));

			onSwitchVisuals(true);
			juicer.BeamFartJuice();

			SetScale(flyByScaleMod);

			while (Vector3.Distance(transform.position, endPos) > .5f)
			{
				transform.position = Vector3.MoveTowards(transform.position, endPos, step);
				yield return null;
			}

			transform.position = endPos;
			onSwitchVisuals(false);
			SetScale(1);
			juicer.StopBeamFartJuice();
		}

		private void SetScale(float multiplier)
		{
			transform.localScale = originalScale * multiplier;

			for (int i = 0; i < beamParticles.Length; i++)
			{
				var main = beamParticles[i].main;
				float min = beamOriginalMins[i] * multiplier;
				float max = beamOriginalMaxs[i]	* multiplier;

				main.startSize = new ParticleSystem.MinMaxCurve(min, max);
			}

			var main2 = juicer.fartBeam.main;
			float min2 = beamOriginalMin * multiplier;
			float max2 = beamOriginalMax * multiplier;

			main2.startSize = new ParticleSystem.MinMaxCurve(min2, max2);
		}

		private void SaveOriginalScales()
		{
			originalScale = transform.localScale;

			beamParticles = juicer.fartBeam.GetComponentsInChildren<ParticleSystem>();
			beamOriginalMins = new float[beamParticles.Length];
			beamOriginalMaxs = new float[beamParticles.Length];

			for (int i = 0; i < beamParticles.Length; i++)
			{
				var main = beamParticles[i].main;
				beamOriginalMins[i] = main.startSize.constantMin;
				beamOriginalMaxs[i] = main.startSize.constantMax;
			}

			var main2 = juicer.fartBeam.main;
			beamOriginalMin = main2.startSize.constantMin;
			beamOriginalMax = main2.startSize.constantMax;
		}

		public void ResetFartCollided()
		{
			fartCollided = false;
		}
	}
}

