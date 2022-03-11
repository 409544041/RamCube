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
		[SerializeField] MapCoreRefHolder mapCoreRef;

		//States
		PersistentRefHolder persRef;

		private void Awake()
		{
			persRef = mapCoreRef.persistantRef;
		}

		public void StartLoadingSerpentScreen()
		{
			StartCoroutine(LoadSerpentScreen());
		}

		private IEnumerator LoadSerpentScreen()
		{
			transform.parent = null;
			DontDestroyOnLoad(gameObject);

			if (mapCoreRef.musicFadeOut) mapCoreRef.musicFadeOut.FadeMusicOut();
			yield return persRef.fader.FadeOut(persRef.fader.sceneTransTime);
			if (mapCoreRef.musicFadeOut) mapCoreRef.musicFadeOut.TurnMusicOff();

			yield return SceneManager.LoadSceneAsync("SerpentScreen");

			yield return persRef.fader.FadeIn(persRef.fader.sceneTransTime);

			Destroy(gameObject);
		}
	}
}
