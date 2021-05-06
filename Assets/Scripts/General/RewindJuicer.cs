using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class RewindJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] ParticleSystem rewindVFX = null;

		public void StartRewindDust()
		{
			rewindVFX.Play();
		}
	}
}
