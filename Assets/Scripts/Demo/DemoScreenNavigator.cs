using System.Collections;
using System.Collections.Generic;
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

				int i = SceneManager.GetActiveScene().buildIndex;
				yield return SceneManager.LoadSceneAsync(i + 1);

				yield return fader.FadeIn(fader.sceneTransTime);
				Destroy(gameObject);
			}
		}
	}
}
