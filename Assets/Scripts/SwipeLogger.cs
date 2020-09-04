using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwipeLogger : MonoBehaviour
{
	private void Update() 
	{
		//TO DO: Remove and place in a sceneloader script once it's made
		if (Input.GetKeyDown(KeyCode.R))
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);	
	}

	private void SwipeLog(SwipeDetector.SwipeDirection direction)
	{
		Debug.Log("Swipe in Direction: " + direction);
	}

	private void TapLog()
	{
		Debug.Log("Tapped");
	}
}
