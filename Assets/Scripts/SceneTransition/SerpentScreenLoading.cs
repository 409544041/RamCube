using Qbism.General;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Qbism.SceneTransition
{
	public class SerpentScreenLoading : MonoBehaviour
	{
		public void StartLoadingSerpentScreen()
		{
			StartCoroutine(LoadSerpentScreen());
		}

		private IEnumerator LoadSerpentScreen()
		{
			var fader = FindObjectOfType<Fader>();
			var musicFader = FindObjectOfType<MusicFadeOut>();

			transform.parent = null;
			DontDestroyOnLoad(gameObject);

			if (musicFader) musicFader.FadeMusicOut();
			yield return fader.FadeOut(fader.sceneTransTime);
			if (musicFader) musicFader.TurnMusicOff();

			yield return SceneManager.LoadSceneAsync("SerpentScreen");

			yield return fader.FadeIn(fader.sceneTransTime);

			Destroy(gameObject);
		}
	}
}
