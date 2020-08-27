using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
	//Config parameters
	[SerializeField] bool DetectBeforeRelease = false;
	[SerializeField] float minSwipeDistance = 20f;

	//States
	Vector2 fingerDownPos;
	Vector2 fingerUpPos;

	public static event Action<SwipeDirection> onSwipe;
	public static event Action onTap;

	public enum SwipeDirection { up, down, left, right }

	void Update()
	{
		if(Input.touchCount > 0) DetectTouches();
	}

	private void DetectTouches()
	{
		Touch touch = Input.GetTouch(0);
		if (touch.phase == TouchPhase.Began)
		{
			fingerUpPos = touch.position;
			fingerDownPos = touch.position;
		}

		if (DetectBeforeRelease && touch.phase == TouchPhase.Moved)
		{
			fingerDownPos = touch.position;
			DetectSwipeOrTap();
		}

		if (touch.phase == TouchPhase.Ended)
		{
			fingerDownPos = touch.position;
			DetectSwipeOrTap();
		}
	}

	private void DetectSwipeOrTap()
	{
		if(SwipeDistanceCheck())
		{
			if(IsUpSwipe())
			{
				var direction = SwipeDirection.up;
				onSwipe(direction);
			}
			if (IsDownSwipe())
			{
				var direction = SwipeDirection.down;
				onSwipe(direction);
			}
			if (IsLeftSwipe())
			{
				var direction = SwipeDirection.left;
				onSwipe(direction);
			}
			if (IsRightSwipe())
			{
				var direction = SwipeDirection.right;
				onSwipe(direction);
			}
		}
		else onTap();
	}

	private bool SwipeDistanceCheck()
	{
		return Mathf.Abs(VerticalMoveDistance()) > minSwipeDistance 
			|| Mathf.Abs(HorizontalMoveDistance()) > minSwipeDistance;
	}

	private float VerticalMoveDistance()
	{
		return fingerDownPos.y - fingerUpPos.y;
	}

	private float HorizontalMoveDistance()
	{
		return fingerDownPos.x - fingerUpPos.x;
	}

	private bool IsUpSwipe()
	{
		return VerticalMoveDistance() > 0 && HorizontalMoveDistance() > 0;
	}

	private bool IsDownSwipe()
	{
		return VerticalMoveDistance() < 0 && HorizontalMoveDistance() < 0;
	}

	private bool IsLeftSwipe()
	{
		return VerticalMoveDistance() > 0 && HorizontalMoveDistance() < 0;
	}

	private bool IsRightSwipe()
	{
		return VerticalMoveDistance() < 0 && HorizontalMoveDistance() > 0;
	}
}
