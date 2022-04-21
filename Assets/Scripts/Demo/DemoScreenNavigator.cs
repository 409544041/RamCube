using System.Collections;
using System.Collections.Generic;
using Qbism.SceneTransition;
using UnityEngine;
using UnityEngine.UI;

namespace Qbism.Demo
{
	public class DemoScreenNavigator : MonoBehaviour
	{
		//Config parameters
		[SerializeField] GameObject[] screens;
		[SerializeField] float fadeTime;
		[SerializeField] Color fadeColor;
		[SerializeField] WorldMapLoading mapLoader;

		//Cache
		Fader fader;
		Image fadeImage;

		//States
		int currentScreen = 0;

		private void Awake() 
		{
			fader = FindObjectOfType<Fader>();
			fadeImage = fader.GetComponentInChildren<Image>();
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
				mapLoader.StartLoadingWorldMap(false);
			}
		}
	}
}
