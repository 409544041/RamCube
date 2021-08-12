using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerOutroJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] AudioClip screamingClip, ouchClip, boingShortClip, boingLongClip;
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
			source.PlayOneShot(boingLongClip);
		}

		public void PlaySmallLandingSound()
		{
			source.PlayOneShot(boingShortClip);
		}
	}
}
