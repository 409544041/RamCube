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

	public static event Action<SwipeData> onSwipe;

	public struct SwipeData
	{
		public Vector2 startPosition;
		public Vector2 endPosition;
		public SwipeDirection swipeDirection;
	}

	public enum SwipeDirection { up, down, left, right }

	void Update()
	{
		foreach (Touch touch in Input.touches)
		{
			if(touch.phase == TouchPhase.Began)
			{
				fingerUpPos = touch.position;
				fingerDownPos = touch.position;
			}

			if(DetectBeforeRelease && touch.phase == TouchPhase.Moved)
			{
				fingerDownPos = touch.position;
				DetectSwipe();
			}

			if(touch.phase == TouchPhase.Ended)
			{
				fingerDownPos = touch.position;
				DetectSwipe();
			}
		}
	}

	private void DetectSwipe()
	{
		if(SwipeDistanceCheck())
		{
			if(IsUpSwipe())
			{
				var direction = SwipeDirection.up;
				SendSwipe(direction);
			}
			if (IsDownSwipe())
			{
				var direction = SwipeDirection.down;
				SendSwipe(direction);
			}
			if (IsLeftSwipe())
			{
				var direction = SwipeDirection.left;
				SendSwipe(direction);
			}
			if (IsRightSwipe())
			{
				var direction = SwipeDirection.right;
				SendSwipe(direction);
			}
		}
	}

	private void SendSwipe(SwipeDirection direction)
	{
		SwipeData swipeData = new SwipeData()
		{
			swipeDirection = direction, 
			startPosition = fingerDownPos,
			endPosition = fingerUpPos
		};

		onSwipe(swipeData);
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
