using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
	//Config parameters
	[SerializeField] float loadDelay;
	[SerializeField] Text levelText;

	private void Start() 
	{
		levelText.text = SceneManager.GetActiveScene().name;
	}

	private void Update()
	{
		RestartLevel();
	}

	private static void RestartLevel()
	{
		if (Input.GetKeyDown(KeyCode.R))
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
