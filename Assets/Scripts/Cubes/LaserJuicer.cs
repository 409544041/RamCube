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
		[SerializeField] Light laserTipLight;
		[SerializeField] AudioClip passClip = null, denyClip = null;
		[SerializeField] ParticleSystem laserBeam, denyBeam, denySunSpots, pinkEyeVFX;
		[SerializeField] MMFeedbacks denyJuice;

		//Cache
		AudioSource source;
		LaserMouthAnimator mouthAnim;
		LaserEyeAnimator eyeAnim;
		MMFeedbackWiggle denyMMWiggle;

		//States
		bool isDenying = false;
		float shakeTimer = 0;
		float shakeDur = 0;

		//Actions, events, delegates etc
		public UnityEvent onLaserPassEvent = new UnityEvent();

		private void Awake() 
		{
			source = GetComponentInChildren<AudioSource>();
			eyeAnim = GetComponentInChildren<LaserEyeAnimator>();
			mouthAnim = GetComponentInChildren<LaserMouthAnimator>();
			denyMMWiggle = denyJuice.GetComponent<MMFeedbackWiggle>();
			shakeDur = denyMMWiggle.WigglePositionDuration;
		}

		private void Update()
		{
			HandleShakeTimer();
		}

		public void AdjustBeamVisualLength(float dist)
		{
			var idleMain = laserBeam.main;
			idleMain.startSizeZMultiplier = dist;

			var denyMain = denyBeam.main;
			denyMain.startSizeZMultiplier = dist;

			var shape = denySunSpots.shape;
			var spotsMain = denySunSpots.main;
			//Ensures that sunspots stop at player even with the extra length it gets from its speed
			var extraLength = spotsMain.startSpeedMultiplier * spotsMain.startLifetimeMultiplier;
			shape.length = dist - extraLength;

			MoveTipLight(dist);
		}

		public void TriggerPassJuice()
		{
			isDenying = false;

			laserBeam.Stop();
			denyBeam.Stop();
			pinkEyeVFX.Play();

			source.clip = passClip;
			onLaserPassEvent.Invoke();

			eyeAnim.CloseEyes();
			mouthAnim.SadMouth();

		}

		public void TriggerDenyJuice(float dist)
		{
			isDenying = true;

			laserBeam.Stop();
			pinkEyeVFX.Stop();
			denyBeam.Play();

			Shake();

			source.clip = denyClip;
			onLaserPassEvent.Invoke();

			eyeAnim.ShootyEyes();
			mouthAnim.SadMouth();
		}

		public void TriggerIdleJuice()
		{
			isDenying = false;

			pinkEyeVFX.Stop();
			denyBeam.Stop();
			laserBeam.Play();

			eyeAnim.OpenEyes();
			mouthAnim.HappyMouth();
		}

		private void Shake()
		{
			denyJuice.Initialization();
			denyJuice.PlayFeedbacks();
		}

		private void HandleShakeTimer()
		{
			//This repeats the shake feedback (which is .5s) over and over instead of
			//using 'repeat forever' bc couldn't find way to stop that
			if (isDenying)
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
