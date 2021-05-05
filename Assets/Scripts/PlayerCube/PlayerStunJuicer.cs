using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerStunJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] ParticleSystem stunVFX;

		public void PlayStunVFX()
		{
			stunVFX.Play();
		}

		public void StopStunVFX()
		{
			stunVFX.Stop();
		}
	}
}
