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
}
