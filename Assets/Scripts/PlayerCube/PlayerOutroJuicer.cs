using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerOutroJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] AudioClip screamingClip, ouchClip, hitFloorClip, hitSegClip;
		[SerializeField] AudioSource source;

		public void PlayFallingSound()
		{
			source.clip = screamingClip;
			source.Play();
		}

		public void PlayLandingSound()
		{
			source.Stop();
			source.PlayOneShot(ouchClip);
			source.PlayOneShot(hitSegClip);
		}
	}
}
