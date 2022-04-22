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

		//Cache
		GameControls controls;

		private void Awake()
		{
			controls = new GameControls();

			controls.Gameplay.ANYkey.performed += ctx => StartSceneTransition();
		}

		private void OnEnable()
		{
			controls.Gameplay.Enable();
		}

		private void StartSceneTransition()
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

		private void OnDisable()
		{
			controls.Gameplay.Disable();
		}
	}
}
