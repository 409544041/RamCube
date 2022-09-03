using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

namespace Qbism.Cubes
{
	public class LaserJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MMFeedbacks activateJuiceWiggle, activateJuice, passJuice;
		[Header ("Laser")]
		[SerializeField] Light laserTipLight;
		[Header ("Particles")]
		[SerializeField] ParticleSystem laserBeam; 
		[SerializeField] ParticleSystem activatedBeam, activatedSpots, pinkEyeVFX;
		[SerializeField] int spotsEmissionMult = 25;

		//Cache
		LaserMouthAnimator mouthAnim;
		LaserEyeAnimator eyeAnim;
		MMFeedbackWiggle denyMMWiggle;

		//States
		public bool isActivated { get; private set; } = false;
		float shakeTimer = 0;
		float shakeDur = 0;

		private void Awake() 
		{
			eyeAnim = GetComponentInChildren<LaserEyeAnimator>();
			mouthAnim = GetComponentInChildren<LaserMouthAnimator>();
			denyMMWiggle = activateJuiceWiggle.GetComponent<MMFeedbackWiggle>();
			shakeDur = denyMMWiggle.WigglePositionDuration;
		}

		private void Update()
		{
			HandleShakeTimer();
		}

		public void AdjustBeamVisualLength(float dist)
		{
			var idleMain = laserBeam.main;
			var activatedMain = activatedBeam.main;
			var spotsShape = activatedSpots.shape;
			var spotsMain = activatedSpots.main;
			var spotsEmission = activatedSpots.emission;

			//The .1f is to ensure that the laser visuals don't stop before hitting rounded objects
			idleMain.startSizeZMultiplier = dist + .1f;
			activatedMain.startSizeZMultiplier = dist + .1f;
			spotsEmission.rateOverTime = dist * spotsEmissionMult;

			//Ensures that spots stop at player even with the extra length it gets from its speed
			var extraLength = spotsMain.startSpeedMultiplier * spotsMain.startLifetimeMultiplier;
			spotsShape.length = dist - extraLength;

			MoveTipLight(dist);
		}

		public void TriggerPassJuice()
		{
			isActivated = false;

			laserBeam.Stop();
			activatedBeam.Stop();
			pinkEyeVFX.Play();

			passJuice.PlayFeedbacks();

			eyeAnim.CloseEyes();
			mouthAnim.SadMouth();
		}

		public void CloseEyeForFinish()
		{
			eyeAnim.CloseEyes();
			mouthAnim.SadMouth();
			activatedBeam.Stop();
			laserBeam.Stop();
			pinkEyeVFX.Stop();
			laserTipLight.enabled = false;
		}

		public void TriggerActivationJuice()
		{
			isActivated = true;

			laserBeam.Stop();
			pinkEyeVFX.Stop();
			activatedBeam.Play();

			Shake();
			activateJuice.PlayFeedbacks();

			eyeAnim.ShootyEyes();
			mouthAnim.SadMouth();
		}

		public void TriggerIdleJuice()
		{
			isActivated = false;

			pinkEyeVFX.Stop();
			activatedBeam.Stop();
			laserBeam.Play();

			eyeAnim.OpenEyes();
			mouthAnim.HappyMouth();
		}

		private void Shake()
		{
			activateJuiceWiggle.PlayFeedbacks();
		}

		private void HandleShakeTimer()
		{
			//This repeats the shake feedback (which is .5s) over and over instead of
			//using 'repeat forever' bc couldn't find way to stop that
			if (isActivated)
			{
				shakeTimer += Time.deltaTime;

				if (shakeTimer >= shakeDur + .1f)
				{
					Shake();
					shakeTimer = 0;
				}
			}
		}

		private void MoveTipLight(float dist)
		{
			laserTipLight.transform.localPosition = new Vector3(0, 0, dist + 0.4f);
		}
	}
}
