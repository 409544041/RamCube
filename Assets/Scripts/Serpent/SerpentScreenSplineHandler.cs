using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using System;

namespace Qbism.Serpent
{
	public class SerpentScreenSplineHandler : MonoBehaviour
	{
		//Config Parameters
		[SerializeField] SplineComputer spline;
		[SerializeField] float moveSpeed = 5f;
		[SerializeField] double spacing = .025;
		[SerializeField] double headSpacing = .015;
		[SerializeField] SplineFollower[] followers;
		[SerializeField] GameObject[] highlightSegments;
		//Using doubles because that's how spline percent works

		//States
		SplineFollower closestFollower = null;
		int closestFollowerIndex = 0;
		Vector3 triggerPos;
		float shortestDis = 0f;
		bool startPosSet = false;
		bool canScrollUp = true;
		bool canScrollDown = true;
		int serpSize = 0;
		List<bool> serpDataList = new List<bool>();

		//Actions, events, delegates etc
		public Func<List<bool>> onFetchSerpDataList;


		private void Awake()
		{
			triggerPos = spline.EvaluatePosition(spline.triggerGroups[0].triggers[0].position);
		}

		private void Start()
		{
			serpDataList = onFetchSerpDataList();
			FetchSerpentSize();
		}

		private void Update()
		{
			//Doing this in update to avoid a race condition with Dreamteck Splines' start positions
			SetStartFocus();
		}

		private void FetchSerpentSize()
		{
			foreach (bool serpData in serpDataList)
			{
				if (serpData) serpSize++;
			}
		}

		private void SetStartFocus()
		{
			if (startPosSet) return;

			followers[serpSize].result.percent = spline.triggerGroups[0].triggers[0].position;

			GetClosestFollower();
			SnapSegmentPositions();
			SetScrollPermissions();
			HighlightSegment();

			startPosSet = true;
		}

		public void Scroll(Vector2 stickValue)
		{
			foreach (SplineFollower follower in followers)
			{
				if (stickValue.y > .1 && canScrollUp)
				{
					follower.direction = Spline.Direction.Backward;
					follower.followSpeed = moveSpeed;
				}
				else if (stickValue.y < -.1 && canScrollDown)
				{
					follower.direction = Spline.Direction.Forward;
					follower.followSpeed = moveSpeed;
				}
			}
		}

		private void StopMovement() 
		{
			foreach (SplineFollower follower in followers) follower.followSpeed = 0;
		}

		private void GetClosestFollower()
		{
			bool firstValueAssigned = false;

			for (int i = 0; i < followers.Length; i++)
			{
				Vector3 followerPos = followers[i].result.position;
				float disToTrigger = Vector3.Distance(triggerPos, followerPos);

				if (!firstValueAssigned && followers[i].useTriggers)
				{
					shortestDis = disToTrigger;
					closestFollower = followers[i];
					closestFollowerIndex = i;
					firstValueAssigned = true;
				}

				if (disToTrigger < shortestDis && followers[i].useTriggers)
				{
					shortestDis = disToTrigger;
					closestFollower = followers[i];
					closestFollowerIndex = i;
				}
			}
		}

		private void SnapSegmentPositions()
		{
			int startIndex = 0;
			double startPos = 0;

			for (int i = 0; i < followers.Length; i++)
			{
				if (followers[i] == closestFollower)
				{
					startIndex = i;
					startPos = followers[i].result.percent;
				}
			}

			//Starting from startIndex, get the segments with higher list index but lower spline percent
			for (int i = startIndex + 1; i < followers.Length; i++)
			{
				double newPos = startPos - spacing * (i - startIndex);
				if (startIndex == 0) newPos -= headSpacing;
				if (newPos <= 0) newPos++;
				followers[i].result.percent = newPos;
			}

			//Starting from startIndex, get the segments with lower list index but higher spline percent
			for (int i = startIndex - 1; i >= 0; i--)
			{
				double newPos = startPos - spacing * (i - startIndex);
				if (newPos >= 1) newPos--;
				followers[i].result.percent = newPos;

				if (i == 0) //checking if it's the head to give it some extra space
					followers[i].result.percent += headSpacing;
			}
		}

		private void HighlightSegment()
		{
			//Ensure order is same in highlightSegments and followers arrays for this to work correctly
			for (int i = 0; i < highlightSegments.Length; i++)
			{
				var mRender = highlightSegments[i].GetComponentInChildren<Renderer>();
				var sRender = highlightSegments[i].GetComponentInChildren<SpriteRenderer>();

				if (!mRender || !sRender) Debug.LogWarning
					 (highlightSegments[i] + " is missing either a meshrenderer or spriterenderer!");

				if(i == closestFollowerIndex) 
				{
					mRender.enabled = true;
					if (sRender) sRender.enabled = true; 
				}
				else
				{
					mRender.enabled = false;
					if (sRender) sRender.enabled = false;
				}
			}
		}

		private void SetScrollPermissions()
		{
			if (followers.Length > 1 && closestFollower == followers[serpSize])
			{
				canScrollUp = true;
				canScrollDown = false;
			}
			else if (followers.Length > 1 && closestFollower == followers[0])
			{
				canScrollUp = false;
				canScrollDown = true;
			}
			else if (followers.Length == 1)
			{
				canScrollUp = false;
				canScrollDown = false;
			}
			else
			{
				canScrollUp = true;
				canScrollDown = true;
			}
		}

		public void HandleSplineTrigger() //Called by spline trigger
		{
			StopMovement();
			GetClosestFollower();
			SnapSegmentPositions();
			SetScrollPermissions();
			HighlightSegment();
		}
	}
}
