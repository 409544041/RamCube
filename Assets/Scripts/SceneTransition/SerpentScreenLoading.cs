using Qbism.General;
using Qbism.Saving;
using Qbism.WorldMap;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Qbism.SceneTransition
{
	public class SerpentScreenLoading : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MapCoreRefHolder mcRef;

		//States
		PersistentRefHolder persRef;

		private void Awake()
		{
			persRef = mcRef.persRef;
		}

		public void StartLoadingSerpentScreen()
		{
			StartCoroutine(LoadSerpentScreen());
		}

		private IEnumerator LoadSerpentScreen()
		{
			transform.parent = null;
			DontDestroyOnLoad(gameObject);

			if (mcRef.musicFader) mcRef.musicFader.FadeMusicOut();
			yield return persRef.fader.FadeOut(persRef.fader.sceneTransTime);
			if (mcRef.musicFader) mcRef.musicFader.TurnMusicOff();

			yield return SceneManager.LoadSceneAsync("SerpentScreen");

			yield return persRef.fader.FadeIn(persRef.fader.sceneTransTime);

			Destroy(gameObject);
		}
	}
}
