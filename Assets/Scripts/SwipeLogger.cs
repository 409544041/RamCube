using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwipeLogger : MonoBehaviour
{
	private void SwipeLog(SwipeDetector.SwipeDirection direction)
	{
		Debug.Log("Swipe in Direction: " + direction);
	}

	private void TapLog()
	{
		Debug.Log("Tapped");
	}
}
