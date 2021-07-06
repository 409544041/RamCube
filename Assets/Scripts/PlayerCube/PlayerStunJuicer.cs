using System.Collections;
using System.Collections.Generic;
using Qbism.SpriteAnimations;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerStunJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] ParticleSystem stunVFX;
		[SerializeField] GameObject stunMesh;

		public void PlayStunVFX()
		{
			stunVFX.Play();
			stunMesh.SetActive(true);
			GetComponentInChildren<ExpressionHandler>().
				SetFace(ExpressionSituations.laserHit, -1f);
		}

		public void StopStunVFX()
		{
			stunVFX.Stop();
			stunMesh.SetActive(false);
		}
	}
}
