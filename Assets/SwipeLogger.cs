using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeLogger : MonoBehaviour
{
	private void OnEnable() 
	{
		SwipeDetector.onSwipe += SwipeLog;
		SwipeDetector.onTap += TapLog;
	}

	private void SwipeLog(SwipeDetector.SwipeDirection direction)
	{
		Debug.Log("Swipe in Direction: " + direction);
	}

	private void TapLog()
	{
		Debug.Log("Tapped");
	}

	private void OnDisable()
	{
		SwipeDetector.onSwipe -= SwipeLog;
		SwipeDetector.onTap -= TapLog;
	}
}
