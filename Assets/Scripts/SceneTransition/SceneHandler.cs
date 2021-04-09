using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Qbism.SceneTransition
{
	public class SceneHandler : MonoBehaviour
	{
		//States
		string currentSceneName;

		private void Start()
		{
			currentSceneName = SceneManager.GetActiveScene().name;
		}
		
		public void RestartLevel()
		{
			SceneManager.LoadScene(currentSceneName);
		}

		public void PreviousLevel()
		{
			var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
			if (currentSceneIndex == 0) return;
			SceneManager.LoadSceneAsync(currentSceneIndex - 1);
		}

		public void NextLevel()
		{
			var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
			if (currentSceneIndex == SceneManager.sceneCountInBuildSettings - 1) return;
			SceneManager.LoadSceneAsync(currentSceneIndex + 1);
		}

		public void LoadWorldMap()
		{
			SceneManager.LoadSceneAsync(1); //TO DO: Index order might change. Don't forget to change it here too.
		}

		public void LoadSerpentScreen()
		{
			SceneManager.LoadSceneAsync(2); //TO DO: Index order might change. Don't forget to change it here too.
		}

		public void LoadBySceneIndex(int index)
		{
			SceneManager.LoadSceneAsync(index);
		}
	}
}
