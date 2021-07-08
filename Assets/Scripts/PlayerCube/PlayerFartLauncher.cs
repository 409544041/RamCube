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
		[Header ("Farts")]
		// [SerializeField] float[] fartForces;
		// [SerializeField] float[] intervals;
		[SerializeField] float fartForce;
		[SerializeField] float stopFartViewportPos = 1.2f;
		[SerializeField] LayerMask fartRayCastLayers;
		[SerializeField] Transform fartVFXParent;
		[SerializeField] float impactBuffer;
		[Header("Falling")]
		[SerializeField] float fallMultiplier = 2.5f;
		[SerializeField] float lowLaunchMultiplier = 2f;
		[Header ("Juice")]
		[SerializeField] MMFeedbacks preFartJuice = null;
		public ParticleSystem fartCharge, bulletFart, fartBeam, 
			fartBeamImpact, bulletFartImpact;
		[SerializeField] GameObject playerVis;
		[SerializeField] float flyByScaleMod, flyBySpeedMod;

		//Cache
		GameControls controls;
		MMFeedbackWiggle preFartMMWiggle;
		ExpressionHandler exprHandler;

		//States
		Vector3 viewPos;
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

		//Actions, events, delegates etc
		public event Action onDoneFarting;
		public event Action onStartFarting;
		public event Action onSwitchToEndCam;
		public event Action onMoveCam;

		void Awake()
		{
			preFartMMWiggle = preFartJuice.GetComponent<MMFeedbackWiggle>();
			exprHandler = GetComponentInChildren<ExpressionHandler>();
		}

		private void Start()
		{
			SaveOriginalScales();
		}

		private void Update() 
		{
			if(fartCounting) fartCount++;
			if(fartCount > 10) StopFartHit();

			viewPos = Camera.main.WorldToViewportPoint(transform.position);
		}

		public void InitiateFartSequence(Transform target)
		{
			StartCoroutine(FartSequence(target));
		}

		private IEnumerator FartSequence(Transform target)
		{
			transform.parent = null;
			
			preFartJuice.Initialization();
			preFartJuice.PlayFeedbacks();
			fartCharge.Play();

			exprHandler.SetFace(ExpressionSituations.fartCharge, -1);

			float feedbackDuration = preFartMMWiggle.WigglePositionDuration;
			yield return new WaitForSeconds(feedbackDuration);

			exprHandler.SetFace(ExpressionSituations.preFartBlast, -1);
			onMoveCam();

			yield return new WaitForSeconds(.5f);
			exprHandler.SetFace(ExpressionSituations.endSeqFart, -1);
			fartBeam.Play();
			onStartFarting();
			StartCoroutine(LaunchPlayer(target));
		}

		public void StartBeamImpact()
		{
			currentFartImpact = fartBeamImpact;
			ProcessFartRaycast();
		}

		public void StartLaserBulletImpact()
		{
			currentFartImpact = bulletFartImpact;
			ProcessFartRaycast();
		}

		private void ProcessFartRaycast()
		{
			bool hasHit = false;
			RaycastHit hit = FireRayCast(out hasHit);
			
			if (hasHit)
			{
				if(!fartCollided)
				{
					currentFartImpact.transform.parent = null;
					currentFartImpact.transform.position = Vector3.Lerp(hit.point, transform.position, .05f);
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
			bulletFart.Play();
			GetComponentInChildren<ExpressionHandler>().
				SetFace(ExpressionSituations.fart, .75f);
		}

		private void StopFartHit()
		{
			var module = currentFartImpact.main;
			if (module.loop) currentFartImpact.Stop();
			currentFartImpact.transform.parent = fartVFXParent;
			fartCounting = false;
		}

		private IEnumerator LaunchPlayer(Transform target)
		{
			float step = fartForce * Time.deltaTime;

			while (Vector3.Distance(transform.position, target.position) > 0.01f)
			{
				transform.position = Vector3.MoveTowards(transform.position, target.position, step);

				if(Vector3.Distance(transform.position, Camera.main.transform.position) < 2 && !endCam)
				{
					onSwitchToEndCam();
					endCam = true;
				} 

				yield return null;
			}

			transform.position = target.position;
			SetVisuals(false);
			onDoneFarting();
			fartBeam.Stop();
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

			SetVisuals(true);
			fartBeam.Play();

			SetScale(flyByScaleMod);

			while (Vector3.Distance(transform.position, endPos) > .1f)
			{
				transform.position = Vector3.MoveTowards(transform.position, endPos, step);
				yield return null;
			}

			SetVisuals(false);
			SetScale(1);
			fartBeam.Stop();
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

			var main2 = fartBeam.main;
			float min2 = beamOriginalMin * multiplier;
			float max2 = beamOriginalMax * multiplier;

			main2.startSize = new ParticleSystem.MinMaxCurve(min2, max2);
		}

		private void SetVisuals(bool value)
		{
			SkinnedMeshRenderer[] meshes = playerVis.GetComponentsInChildren<SkinnedMeshRenderer>();

			foreach (var mesh in meshes)
			{
				mesh.enabled = value;
			}

			SpriteRenderer[] sprites = playerVis.GetComponentsInChildren<SpriteRenderer>();

			foreach (var sprite in sprites)
			{
				sprite.enabled = value;
			}
		}

		private void SaveOriginalScales()
		{
			originalScale = transform.localScale;

			beamParticles = fartBeam.GetComponentsInChildren<ParticleSystem>();
			beamOriginalMins = new float[beamParticles.Length];
			beamOriginalMaxs = new float[beamParticles.Length];

			for (int i = 0; i < beamParticles.Length; i++)
			{
				var main = beamParticles[i].main;
				beamOriginalMins[i] = main.startSize.constantMin;
				beamOriginalMaxs[i] = main.startSize.constantMax;
			}

			var main2 = fartBeam.main;
			beamOriginalMin = main2.startSize.constantMin;
			beamOriginalMax = main2.startSize.constantMax;
		}
	}
}

