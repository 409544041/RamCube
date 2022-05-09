using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Qbism.General;
using Qbism.WorldMap;

namespace Qbism.SceneTransition
{
	public class SplashSceneLoading : MonoBehaviour
	{
		//Config parameters
		[SerializeField] int firstLevelIndex;

		public void StartSceneTransition()
		{
			StartCoroutine(SceneTransition());
		}

		private IEnumerator SceneTransition()
		{
			Fader fader = FindObjectOfType<Fader>();

			transform.parent = null;
			DontDestroyOnLoad(gameObject);

			yield return fader.FadeOut(fader.sceneTransTime);

			var switchBoard = FindObjectOfType<FeatureSwitchBoard>();
			
			if (switchBoard.demoSplashConnected)
				yield return SceneManager.LoadSceneAsync("DemoSplashScene");

			else if (switchBoard.worldMapConnected)
			{
				yield return SceneManager.LoadSceneAsync("WorldMap");

				var centerPoint = FindObjectOfType<PositionBiomeCenterpoint>();
				centerPoint.PositionCenterPointOnMapLoad();
			}

			else yield return SceneManager.LoadSceneAsync(firstLevelIndex);
			
			yield return fader.FadeIn(fader.sceneTransTime); 
			Destroy(gameObject);
		}
	}
}
