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
			boingLongClip, endLaughClip, smallSurpriseClip, toothyLaughClip, swallowClip;
		[SerializeField] AudioSource source;

		//Cache
		SerpentCollHandler serpCollHandler;

		private void Awake() 
		{
			serpCollHandler = FindObjectOfType<SerpentCollHandler>();
		}

		private void OnEnable() 
		{
			if (serpCollHandler != null)
			{
				serpCollHandler.onTriggerPlayerAudio += StopLaughing;
				serpCollHandler.onTriggerPlayerAudio += PlayOuchSoundFromPickup;
			} 
		}

		public void PlayFallingSound()
		{
			source.clip = screamingClip;
			source.Play();
		}

		public void PlaySwallowClip()
		{
			print("Playing swallow");
			source.PlayOneShot(swallowClip, 2);
		}

		public void PlayLandingSound()
		{
			source.Stop();
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
			source.volume = .3f;
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

		private void PlayOuchSoundFromPickup()
		{
			source.PlayOneShot(ouchClipAlt, .75f);
		}

		public void PlayOuchSound()
		{
			source.PlayOneShot(ouchClip, .5f);
		}

		private void OnDisable()
		{
			if (serpCollHandler != null)
			{
				serpCollHandler.onTriggerPlayerAudio -= StopLaughing;
				serpCollHandler.onTriggerPlayerAudio -= PlayOuchSoundFromPickup;
			}
		}
	}
}
