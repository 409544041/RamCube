using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Qbism.General;

namespace Qbism.SceneTransition
{
	public class SplashSceneLoading : MonoBehaviour
	{
		//Config parameters
		[SerializeField] int firstLevelIndex;
		[SerializeField] float splashTime = 1f;

		private void Start() 
		{
			StartCoroutine(SceneTransition());
		}

		private IEnumerator SceneTransition()
		{
			Fader fader = FindObjectOfType<Fader>();

			transform.parent = null;
			DontDestroyOnLoad(gameObject);

			yield return new WaitForSeconds(splashTime);
			yield return fader.FadeOut(fader.sceneTransTime);

			var switchBoard = FindObjectOfType<FeatureSwitchBoard>();
			
			if (switchBoard.demoSplashConnected)
				yield return SceneManager.LoadSceneAsync("DemoSplashScene");
			else if (switchBoard.worldMapConnected)
				yield return SceneManager.LoadSceneAsync("WorldMap");
			else yield return SceneManager.LoadSceneAsync(firstLevelIndex);
			
			yield return fader.FadeIn(fader.sceneTransTime); 
			Destroy(gameObject);
		}
	}
}
