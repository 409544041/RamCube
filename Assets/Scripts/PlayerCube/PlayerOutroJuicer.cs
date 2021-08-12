using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerOutroJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] AudioClip screamingClip, ouchClip, boingShortClip, 
			boingLongClip, endLaughClip, smallSurpriseClip, toothyLaughClip;
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

		public void PlaySurpriseSound()
		{
			source.PlayOneShot(smallSurpriseClip);
		}

		public void PlayEndLaughSound()
		{
			source.clip = endLaughClip;
			source.Play();
		}

		public void PlayToothyLaughSound()
		{
			source.PlayOneShot(toothyLaughClip);
		}
	}
}
