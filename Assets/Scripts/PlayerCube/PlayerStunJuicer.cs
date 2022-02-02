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
		[SerializeField] PlayerCubeMover mover;

		//Cache
		MMFeedbackWiggle stunMMWiggle;

		//States
		float shakeTimer = 0;
		float shakeDur = 0;

		private void Awake()
		{
			stunMMWiggle = stunWiggleJuice.GetComponent<MMFeedbackWiggle>();
			shakeDur = stunMMWiggle.WigglePositionDuration;
		}

		private void Update()
		{
			if (mover.isStunned) HandleShakeTimer();
		}

		public void PlayStunVFX()
		{
			stunVFX.Play();
			stunMesh.SetActive(true);

			var expressHandler = GetComponentInChildren<ExpressionHandler>();
			expressHandler.SetSituationFace(ExpressionSituations.laserHit, 
				expressHandler.GetRandomTime());
		}

		public void StopStunVFX()
		{
			stunVFX.Stop();
			stunMesh.SetActive(false);
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

		private void Shake()
		{
			stunWiggleJuice.PlayFeedbacks();
		}
	}
}
