using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Qbism.Cubes;
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
		[SerializeField] float flyByScaleMod;	
		[SerializeField] float beamImpactAddedY = .55f;
		[SerializeField] float shockedFaceTime = .5f, launchDur = 1, flyByDelay, flyByDur = 1,
			flyByDistanceFromSCreen;

		//Cache
		ExpressionHandler exprHandler;
		PlayerFartJuicer juicer;
		FinishEndSeqHandler endSeqHandler;

		//States
		bool fartCollided = false;
		bool fartCounting = false;
		int fartCount = 0;
		ParticleSystem currentFartImpact = null;
		Vector3 originalScale;
		float[] beamOriginalMins;
		float[] beamOriginalMaxs;
		float beamOriginalMin;
		float beamOriginalMax;
		ParticleSystem[] beamParticles;
		bool impactOnFinish = false;
		public bool flyingBy { get; set; } = false;
		float flyByStartX, flyByStartY, flyByTargetX, flyByTargetY;
		Vector3 flyByStartPos, flyByEndPos;

		//Actions, events, delegates etc
		public event Action onDoneFarting;
		public event Action onStartFarting;
		public event Action onSwitchToEndCam;
		public event Action<bool> onSwitchVisuals;
		public Func<Vector3> onCheckFinishPos;

		private void Awake()
		{
			exprHandler = GetComponentInChildren<ExpressionHandler>();
			juicer = GetComponent<PlayerFartJuicer>();
			endSeqHandler = FindObjectOfType<FinishEndSeqHandler>();
		}

		private void Start()
		{
			SaveOriginalScales();
		}

		private void Update()
		{
			HandleFartCounting();
			KeepImpactOnFinishPos();

			if (flyingBy)
			{
				flyByStartPos = Camera.main.ViewportToWorldPoint(new Vector3(flyByStartX, flyByStartY, 
					flyByDistanceFromSCreen));
				flyByEndPos = Camera.main.ViewportToWorldPoint(new Vector3(flyByTargetX, flyByTargetY, 
					flyByDistanceFromSCreen));
			}
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

			endSeqHandler.FartChargeCamResize(feedbackDuration + shockedFaceTime);
			yield return new WaitForSeconds(feedbackDuration);

			exprHandler.SetFace(Expressions.shocked, -1);

			yield return new WaitForSeconds(shockedFaceTime);

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
			float elapsedTime = 0;
			var startPos = transform.position;

			endSeqHandler.FartLaunchCamResize(launchDur - .2f);
			
			while (Vector3.Distance(transform.position, target.position) > 0.1f)
			{
				elapsedTime += Time.deltaTime;
				var percentageComplete = elapsedTime / launchDur;

				transform.position = Vector3.Lerp(startPos, target.position, percentageComplete);

				yield return null;
			}

			onSwitchToEndCam();
			onDoneFarting();
			transform.position = target.position;
			onSwitchVisuals(false);
			juicer.StopBeamFartJuice();

			StartCoroutine(FlyBy());
		}

		private IEnumerator FlyBy()
		{
			if (!endSeqHandler.FetchHasSegment()) yield break;

			yield return new WaitForSeconds(flyByDelay);

			CalculateStartEnd();

			flyingBy = true;
			float elapsedTime = 0;

			transform.position = flyByStartPos;
			transform.LookAt(transform.position - (flyByEndPos - transform.position));

			onSwitchVisuals(true);
			juicer.BeamFartJuice();

			SetScale(flyByScaleMod);

			while (Vector3.Distance(transform.position, flyByEndPos) > .5f && flyingBy)
			{
				elapsedTime += Time.deltaTime;
				var percentageComplete = elapsedTime / flyByDur;

				transform.position = Vector3.Lerp(flyByStartPos, flyByEndPos, percentageComplete);

				yield return null;
			}

			flyingBy = false;
			onSwitchVisuals(false);
			SetScale(1);
			juicer.StopBeamFartJuice();
		}

		private void CalculateStartEnd()
		{
			float[] possibleX = new float[2];
			possibleX[0] = -2f;
			possibleX[1] = 2f;

			var index = UnityEngine.Random.Range(0, possibleX.Length);
			flyByStartX = possibleX[index];


			if (flyByStartX > 0) flyByTargetX = -4;
			else flyByTargetX = 5;

			flyByStartY = UnityEngine.Random.Range(.15f, .85f);
			flyByTargetY = UnityEngine.Random.Range(.15f, .85f);

			flyByStartPos = Camera.main.ViewportToWorldPoint(new Vector3(flyByStartX, flyByStartY, 
				flyByDistanceFromSCreen));
			flyByEndPos = Camera.main.ViewportToWorldPoint(new Vector3(flyByTargetX, flyByTargetY, 
				flyByDistanceFromSCreen));
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

