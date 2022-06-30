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
		[SerializeField] FeatureSwitchBoard switchBoard;

		//States
		bool firstLevelCompleted;

		private void Start()
		{
			firstLevelCompleted = E_LevelGameplayData.GetEntity(0).f_Completed;
		}

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

			if (switchBoard.demoSplashConnected)
				yield return SceneManager.LoadSceneAsync("DemoSplashScene");

			else if ((switchBoard.worldMapConnected && firstLevelCompleted) ||
				!switchBoard.showFirstLevelIntroRoll)
			{
				yield return SceneManager.LoadSceneAsync("WorldMap");

				var centerPoint = FindObjectOfType<PositionBiomeCenterpoint>();
				centerPoint.PositionCenterPointOnMapLoad();
			}

			else if (!firstLevelCompleted && switchBoard.showFirstLevelIntroRoll)
				yield return SceneManager.LoadSceneAsync(E_LevelData.GetEntity(0).f_Level);

			else yield return SceneManager.LoadSceneAsync(firstLevelIndex);
			
			yield return fader.FadeIn(fader.sceneTransTime); 
			Destroy(gameObject);
		}
	}
}
