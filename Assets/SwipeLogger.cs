using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeLogger : MonoBehaviour
{
	private void OnEnable() 
	{
		SwipeDetector.onSwipe += SwipeLog;
	}

	private void SwipeLog(SwipeDetector.SwipeData data)
	{
		Debug.Log("Swipe in Direction: " + data.swipeDirection);
	}

	private void OnDisable()
	{
		SwipeDetector.onSwipe -= SwipeLog;
	}
}
