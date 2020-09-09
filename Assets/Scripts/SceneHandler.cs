using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneHandler : MonoBehaviour
{
	//Config parameters
	[SerializeField] float loadDelay;
	[SerializeField] Text levelText;

	//States
	string currentSceneName;

	private void Start() 
	{
		currentSceneName = SceneManager.GetActiveScene().name;
		levelText.text = currentSceneName;
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
		if(currentSceneIndex == SceneManager.sceneCountInBuildSettings -1) return;
		SceneManager.LoadSceneAsync(currentSceneIndex + 1);
	}
}
