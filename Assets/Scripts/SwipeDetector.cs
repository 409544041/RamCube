using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
	//Config parameters
	[SerializeField] bool DetectBeforeRelease = false;
	[SerializeField] float minSwipeDistance = 20f;

	//Cache
	CubeHandler handler;
	PlayerCubeMover mover;

	//States
	Vector2 fingerDownPos;
	Vector2 fingerUpPos;

	public enum SwipeDirection { up, down, left, right }

	private void Awake() 
	{
		handler = FindObjectOfType<CubeHandler>();
		mover = FindObjectOfType<PlayerCubeMover>();
	}

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
			DetectSwipes();
		}

		if (touch.phase == TouchPhase.Ended)
		{
			fingerDownPos = touch.position;
			DetectSwipes();
		}
	}

	private void DetectSwipes()
	{
		if(!SwipeDistanceCheck()) return;

		if(SwipeDistanceCheck())
		{
			if(IsUpSwipe() &&
				handler.floorCubeGrid.ContainsKey(mover.FetchGridPos() + Vector2Int.up))
				mover.HandleSwipeInput(mover.up, Vector3.right);

			if (IsDownSwipe() &&
				handler.floorCubeGrid.ContainsKey(mover.FetchGridPos() + Vector2Int.down))
				mover.HandleSwipeInput(mover.down, Vector3.left);

			if (IsLeftSwipe() &&
				handler.floorCubeGrid.ContainsKey(mover.FetchGridPos() + Vector2Int.left))
				mover.HandleSwipeInput(mover.left, Vector3.forward);

			if (IsRightSwipe() &&
				handler.floorCubeGrid.ContainsKey(mover.FetchGridPos() + Vector2Int.right))
				mover.HandleSwipeInput(mover.right, Vector3.back);
		}
	}

	private bool SwipeDistanceCheck()
	{	print(Mathf.Abs(VerticalMoveDistance()) > minSwipeDistance
			|| Mathf.Abs(HorizontalMoveDistance()) > minSwipeDistance);
			
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
