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
		[SerializeField] MMFeedbacks denyJuiceWiggle, denyJuice, passJuice;
		[Header ("Laser")]
		[SerializeField] Light laserTipLight;
		[Header ("Particles")]
		[SerializeField] ParticleSystem laserBeam; 
		[SerializeField] ParticleSystem denyBeam, denySunSpots, pinkEyeVFX;
		[Header ("Audio")]
		[SerializeField] float stunSoundDelay = .2f;
		[SerializeField] AudioClip[] stunClips;
		[SerializeField] AudioSource stunSource;

		//Cache
		LaserMouthAnimator mouthAnim;
		LaserEyeAnimator eyeAnim;
		MMFeedbackWiggle denyMMWiggle;

		//States
		bool isDenying = false;
		float shakeTimer = 0;
		float shakeDur = 0;
		float stunTimer = 0;

		private void Awake() 
		{
			eyeAnim = GetComponentInChildren<LaserEyeAnimator>();
			mouthAnim = GetComponentInChildren<LaserMouthAnimator>();
			denyMMWiggle = denyJuiceWiggle.GetComponent<MMFeedbackWiggle>();
			shakeDur = denyMMWiggle.WigglePositionDuration;
		}

		private void Start() 
		{
			stunTimer = stunSoundDelay;
		}

		private void Update()
		{
			HandleShakeTimer();
			HandleStunSoundTimer();
		}

		public void AdjustBeamVisualLength(float dist)
		{
			var idleMain = laserBeam.main;
			idleMain.startSizeZMultiplier = dist + .1f;
			//The .1f is to ensure that the laser visuals don't stop before hitting rounded objects

			var denyMain = denyBeam.main;
			denyMain.startSizeZMultiplier = dist + .1f;

			var spotsShape = denySunSpots.shape;
			var spotsMain = denySunSpots.main;
			
			//Ensures that sunspots stop at player even with the extra length it gets from its speed
			var extraLength = spotsMain.startSpeedMultiplier * spotsMain.startLifetimeMultiplier;
			spotsShape.length = dist - extraLength;

			MoveTipLight(dist);
		}

		public void TriggerPassJuice()
		{
			isDenying = false;

			laserBeam.Stop();
			denyBeam.Stop();
			pinkEyeVFX.Play();

			passJuice.PlayFeedbacks();

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
			denyJuice.PlayFeedbacks();

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
			denyJuiceWiggle.Initialization();
			denyJuiceWiggle.PlayFeedbacks();
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

		private void HandleStunSoundTimer()
		{
			if (isDenying)
			{
				stunTimer += Time.deltaTime;

				if (stunTimer >= stunSoundDelay)
				{
					PlayDenySounds();
					stunTimer = 0;
				}
			}
		}

		private void PlayDenySounds()
		{
			float pitchValue = Random.Range(.3f, .5f);
			stunSource.pitch = pitchValue;

			int i = Random.Range(0, stunClips.Length);
			stunSource.PlayOneShot(stunClips[i], .75f);
		}

		private void MoveTipLight(float dist)
		{
			laserTipLight.transform.localPosition = new Vector3(0, 0, dist + 0.4f);
		}
	}
}
