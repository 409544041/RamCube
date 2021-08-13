using System.Collections;
using System.Collections.Generic;
using Qbism.Serpent;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerOutroJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] AudioClip screamingClip, ouchClip, ouchClipAlt, boingShortClip, 
			boingLongClip, endLaughClip, smallSurpriseClip, toothyLaughClip;
		[SerializeField] AudioSource source;

		//Cache
		SerpentMovement serpMover;

		private void Awake() 
		{
			serpMover = FindObjectOfType<SerpentMovement>();
		}

		private void OnEnable() 
		{
			if (serpMover != null)
			{
				serpMover.onTriggerPlayerAudio += StopLaughing;
				serpMover.onTriggerPlayerAudio += PlayOuchSound;
			} 
		}

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

		private void StopLaughing()
		{
			if (source.clip == endLaughClip && source.isPlaying)
			source.Stop();
		}

		private void PlayOuchSound()
		{
			source.PlayOneShot(ouchClipAlt);
		}

		private void OnDisable()
		{
			if (serpMover != null)
			{
				serpMover.onTriggerPlayerAudio -= StopLaughing;
				serpMover.onTriggerPlayerAudio -= PlayOuchSound;
			}
		}
	}
}