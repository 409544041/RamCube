using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using Qbism.PlayerCube;
using Qbism.Rewind;
using UnityEngine;

namespace Qbism.Control
{
	public class SwipeDetector : MonoBehaviour
	{
		//Config parameters
		[SerializeField] bool DetectBeforeRelease = false;
		[SerializeField] float minSwipeDistance = 20f;

		//Cache
		CubeHandler handler;
		PlayerCubeMover mover;
		RewindHandler rewinder;

		//States
		Vector2 fingerDownPos;
		Vector2 fingerUpPos;

		public enum SwipeDirection { up, down, left, right }

		private void Awake()
		{
			handler = FindObjectOfType<CubeHandler>();
			mover = FindObjectOfType<PlayerCubeMover>();
			rewinder = FindObjectOfType<RewindHandler>();
		}

		void Update()
		{
			if (Input.touchCount > 0) DetectTouches();
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
			if (SwipeDistanceCheck())
			{
				if (IsUpSwipe())
				{
					var posAhead = mover.FetchGridPos() + Vector2Int.up;

					if (handler.floorCubeDic.ContainsKey(posAhead)
						&& handler.FetchShrunkStatus(posAhead) == false)
						mover.HandleSwipeInput(mover.up, Vector3.right, posAhead);
				}

				if (IsDownSwipe())
				{
					var posAhead = mover.FetchGridPos() + Vector2Int.down;

					if (handler.floorCubeDic.ContainsKey(posAhead)
						&& handler.FetchShrunkStatus(posAhead) == false)
						mover.HandleSwipeInput(mover.down, Vector3.left, posAhead);
				}

				if (IsLeftSwipe())
				{
					var posAhead = mover.FetchGridPos() + Vector2Int.left;

					if (handler.floorCubeDic.ContainsKey(posAhead)
						&& handler.FetchShrunkStatus(posAhead) == false)
						mover.HandleSwipeInput(mover.left, Vector3.forward, posAhead);
				}

				if (IsRightSwipe())
				{
					var posAhead = mover.FetchGridPos() + Vector2Int.right;

					if (handler.floorCubeDic.ContainsKey(posAhead)
						&& handler.FetchShrunkStatus(posAhead) == false)
						mover.HandleSwipeInput(mover.right, Vector3.back, posAhead);
				}
			}

			else if (rewinder.rewindsAmount > 0) rewinder.StartRewinding();
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

}