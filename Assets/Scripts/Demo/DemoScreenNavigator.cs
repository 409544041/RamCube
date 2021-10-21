using System.Collections;
using System.Collections.Generic;
using Qbism.General;
using Qbism.SceneTransition;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Qbism.Demo
{
	public class DemoScreenNavigator : MonoBehaviour
	{
		//Config parameters
		[SerializeField] GameObject[] screens;
		[SerializeField] float fadeTime;
		[SerializeField] Color fadeColor;
		[SerializeField] int worldMapIndex, firstLevelIndex;

		//Cache
		Fader fader;
		Image fadeImage;

		//States
		int currentScreen = 0;
		Color originalFadeColor;

		private void Awake() 
		{
			fader = FindObjectOfType<Fader>();
			fadeImage = fader.GetComponentInChildren<Image>();
		}

		private void Start() 
		{
			originalFadeColor = fadeImage.color;
		}

		public void GoNext()
		{
			StartCoroutine(Next());
		}

		private IEnumerator Next()
		{
			if (currentScreen < screens.Length - 1)
			{
				fadeImage.color = fadeColor;

				yield return fader.FadeOut(fadeTime);
				screens[currentScreen].SetActive(false);
				currentScreen++;
				
				screens[currentScreen].SetActive(true);
				yield return fader.FadeIn(fadeTime);
			}

			else
			{
				GetComponent<DemoScreenInputHandler>().enabled = false;
				DontDestroyOnLoad(gameObject);

				fadeImage.color = originalFadeColor;

				yield return fader.FadeOut(fader.sceneTransTime);
				screens[currentScreen].SetActive(false);

				var switchBoard = FindObjectOfType<FeatureSwitchBoard>();

				if (switchBoard.worldMapConnected)
					yield return SceneManager.LoadSceneAsync(worldMapIndex);
				else yield return SceneManager.LoadSceneAsync(firstLevelIndex);

				yield return fader.FadeIn(fader.sceneTransTime);
				Destroy(gameObject);
			}
		}
	}
}
