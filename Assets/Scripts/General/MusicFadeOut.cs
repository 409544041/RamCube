using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class MusicFadeOut : MonoBehaviour
	{
		//Config parameters
		[SerializeField] AudioSource source;
		[SerializeField] float fadeSpeed = .01f;

		public void FadeMusicOut()
		{
			StartCoroutine(FadeMusic());
		}

		private IEnumerator FadeMusic()
		{
			while (!Mathf.Approximately(source.volume, 0))
			{
				source.volume = Mathf.MoveTowards(source.volume, 0, fadeSpeed);
				yield return null;
			}
		}

		public void TurnMusicOff()
		{
			source.volume = 0;
		}
	}
}
