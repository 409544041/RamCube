using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Qbism.SpriteAnimations;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerStunJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MMFeedbacks stunWiggleJuice;
		[SerializeField] ParticleSystem stunVFX;
		[SerializeField] GameObject stunMesh;
		[SerializeField] AudioClip[] stunClips;
		[SerializeField] float stunSoundDelay = .2f;
		[SerializeField] AudioSource stunSource;
		[SerializeField] PlayerRefHolder refs;

		//Cache
		MMFeedbackWiggle stunMMWiggle;

		//States
		float shakeTimer = 0;
		float shakeDur = 0;
		float stunTimer = 0;
		float originalVolume;

		private void Awake()
		{
			stunMMWiggle = stunWiggleJuice.GetComponent<MMFeedbackWiggle>();
			shakeDur = stunMMWiggle.WigglePositionDuration;
			stunTimer = stunSoundDelay;
			originalVolume = stunSource.volume;
		}

		private void Update()
		{
			if (refs.playerMover.isStunned)
			{
				HandleShakeTimer();
				HandleStunSoundTimer();
			}
		}

			public void PlayStunVFX()
		{
			stunVFX.Play();
			stunMesh.SetActive(true);

			refs.exprHandler.SetSituationFace(ExpressionSituations.laserHit, 
				refs.exprHandler.GetRandomTime());
		}

		public void StopStunVFX()
		{
			stunVFX.Stop();
			stunMesh.SetActive(false);
			stunSource.volume = 0;
		}

		private void HandleShakeTimer()
		{
			//This repeats the shake feedback (which is .5s) over and over instead of
			//using 'repeat forever' bc couldn't find way to stop that

			shakeTimer += Time.deltaTime;

			if (shakeTimer >= shakeDur + .1f)
			{
				Shake();
				shakeTimer = 0;
			}
		}

		private void HandleStunSoundTimer()
		{
			stunTimer += Time.deltaTime;

			if (stunTimer >= stunSoundDelay)
			{
				PlayDenySounds();
				stunTimer = 0;
			}
		}

		private void PlayDenySounds()
		{
			if(stunSource.volume != originalVolume) stunSource.volume = originalVolume;

			float pitchValue = Random.Range(.3f, .5f);
			stunSource.pitch = pitchValue;

			int i = Random.Range(0, stunClips.Length);
			stunSource.PlayOneShot(stunClips[i], .75f);
		}

		private void Shake()
		{
			stunWiggleJuice.PlayFeedbacks();
		}
	}
}
