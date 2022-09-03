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
		[SerializeField] MMFeedbacks wiggleJuice;
		[SerializeField] ParticleSystem stunVFX, magnetVFX;
		[SerializeField] Material stunMaterial, magnetMaterial;
		[SerializeField] GameObject glowMesh;
		[SerializeField] AudioClip[] clips;
		[SerializeField] float soundDelay = .2f;
		[SerializeField] AudioSource source;
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
			stunMMWiggle = wiggleJuice.GetComponent<MMFeedbackWiggle>();
			shakeDur = stunMMWiggle.WigglePositionDuration;
			stunTimer = soundDelay;
			originalVolume = source.volume;
		}

		private void Update()
		{
			if (refs.playerMover.isStunned)
			{
				HandleShakeTimer();
				HandleStunSoundTimer();
			}
		}

		public void PlayStunVFX(TotemTypes totemType)
		{
			Material mat;
			ParticleSystem vfx;

			if (totemType == TotemTypes.laser)
			{
				mat = stunMaterial;
				vfx = stunVFX;
			}
			else
			{
				mat = magnetMaterial;
				vfx = magnetVFX;
			}

			var renderers = glowMesh.GetComponentsInChildren<Renderer>();

			foreach (var renderer in renderers)
			{
				renderer.material = mat;
			}

			vfx.Play();
			glowMesh.SetActive(true);

			refs.exprHandler.SetSituationFace(ExpressionSituations.laserHit, 
				refs.exprHandler.GetRandomTime());
		}

		public void StopStunVFX(TotemTypes totemType)
		{
			ParticleSystem vfx;

			if (totemType == TotemTypes.laser)vfx = stunVFX;
			else vfx = magnetVFX;
		
			vfx.Stop();
			glowMesh.SetActive(false);
			source.volume = 0;
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

			if (stunTimer >= soundDelay)
			{
				PlayDenySounds();
				stunTimer = 0;
			}
		}

		private void PlayDenySounds()
		{
			if(source.volume != originalVolume) source.volume = originalVolume;

			float pitchValue = Random.Range(.3f, .5f);
			source.pitch = pitchValue;

			int i = Random.Range(0, clips.Length);
			source.PlayOneShot(clips[i], .75f);
		}

		private void Shake()
		{
			wiggleJuice.PlayFeedbacks();
		}
	}
}
