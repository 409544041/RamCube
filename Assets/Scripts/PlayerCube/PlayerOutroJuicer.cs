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
		[SerializeField] PlayerRefHolder refs;

		//Cache
		SerpentCollHandler serpCollHandler;

		private void Awake() 
		{
			serpCollHandler = refs.gcRef.finishRef.serpCollHandler;
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
			refs.source.clip = screamingClip;
			refs.source.Play();
		}

		public void PlaySwallowClip()
		{
			refs.source.PlayOneShot(swallowClip, 2);
		}

		public void PlayLandingSound()
		{
			refs.source.Stop();
			refs.source.PlayOneShot(boingLongClip);
		}

		public void PlaySmallLandingSound()
		{
			refs.source.PlayOneShot(boingShortClip);
		}

		public void PlaySurpriseSound()
		{
			refs.source.PlayOneShot(smallSurpriseClip);
		}

		public void PlayEndLaughSound()
		{
			refs.source.clip = endLaughClip;
			refs.source.volume = .3f;
			refs.source.Play();
		}

		public void PlayToothyLaughSound()
		{
			refs.source.PlayOneShot(toothyLaughClip);
		}

		private void StopLaughing()
		{
			if (refs.source.clip == endLaughClip && refs.source.isPlaying)
			refs.source.Stop();
		}

		private void PlayOuchSoundFromPickup()
		{
			refs.source.PlayOneShot(ouchClipAlt, .75f);
		}

		public void PlayOuchSound()
		{
			refs.source.PlayOneShot(ouchClip, .5f);
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
