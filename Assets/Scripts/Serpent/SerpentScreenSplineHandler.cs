using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using Qbism.Saving;

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

		//Cache
		SerpentProgress serpProg;

		//States
		SplineFollower closestFollower = null;
		Vector3 triggerPos;
		float shortestDis = 0f;
		bool startPosSet = false;
		bool canScrollUp = true;
		bool canScrollDown = true;
		int serpSize = 0;

		private void Awake()
		{
			serpProg = FindObjectOfType<SerpentProgress>();
			triggerPos = spline.EvaluatePosition(spline.triggerGroups[0].triggers[0].position);
		}

		private void Start()
		{
			FetchSerpentSize();
		}

		private void Update()
		{
			//Doing this in update to avoid a race condition with Dreamteck Splines' start positions
			SetStartFocus();
		}

		private void FetchSerpentSize()
		{
			foreach (bool serpData in serpProg.serpentDataList)
			{
				if (serpData) serpSize++;
			}
		}

		private void SetStartFocus()
		{
			if (startPosSet) return;

			followers[serpSize].result.percent = spline.triggerGroups[0].triggers[0].position;

			HighlightClosestFollower();
			SnapSegmentPositions();

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

		public void StopMovement() //Called by spline trigger
		{
			foreach (SplineFollower follower in followers) follower.followSpeed = 0;
		}

		public void HighlightClosestFollower() //Called by spline trigger
		{
			bool firstValueAssigned = false;

			foreach (SplineFollower follower in followers)
			{
				Vector3 followerPos = follower.result.position;
				float disToTrigger = Vector3.Distance(triggerPos, followerPos);

				if (!firstValueAssigned && follower.useTriggers)
				{
					shortestDis = disToTrigger;
					closestFollower = follower;
					firstValueAssigned = true;
				}

				if (disToTrigger < shortestDis && follower.useTriggers)
				{
					shortestDis = disToTrigger;
					closestFollower = follower;
				}
			}

			SetScrollPermissions();
		}

		public void SnapSegmentPositions() //Called from spline trigger
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
	}
}
