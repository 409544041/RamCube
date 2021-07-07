using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Qbism.SceneTransition
{
	public class SplashSceneLoading : MonoBehaviour
	{
		//Config parameters
		[SerializeField] int sceneIndex = -1;
		[SerializeField] float splashTime = 1f;

		private void Start() 
		{
			StartCoroutine(SceneTransition());
		}

		private IEnumerator SceneTransition()
		{
			if (sceneIndex < 0)
			{
				Debug.LogError("sceneIndex is not set");
				yield break;
			}

			Fader fader = FindObjectOfType<Fader>();

			transform.parent = null;
			DontDestroyOnLoad(gameObject);

			yield return new WaitForSeconds(splashTime);
			yield return fader.FadeOut(fader.sceneTransTime);

			yield return SceneManager.LoadSceneAsync(sceneIndex);
			
			yield return fader.FadeIn(fader.sceneTransTime); 
			Destroy(gameObject);
		}
	}
}
