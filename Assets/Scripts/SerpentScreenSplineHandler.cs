using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using Qbism.General;

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
	GameControls controls;

	//States
	Vector2 stickValue;
	bool inputting = false;
	SplineFollower closestFollower = null;
	Vector3 triggerPos;
	float shortestDis;

	private void Awake() 
	{
		controls = new GameControls();
		controls.Gameplay.Movement.performed += ctx => stickValue = ctx.ReadValue<Vector2>();
		//TO DO: Move controls to own script. It's here just for testing
		triggerPos = spline.EvaluatePosition(spline.triggerGroups[0].triggers[0].position);
	}

	private void OnEnable()
	{
		controls.Gameplay.Enable();
	}

	void Update()
	{
		if (stickValue.y > -.1 && stickValue.y < .1)
			inputting = false;

		if (!inputting) Scroll();
	}

	private void Scroll()
	{
		foreach (SplineFollower follower in followers)
		{
			if (stickValue.y > .1)
			{
				follower.direction = Spline.Direction.Backward;
				follower.followSpeed = moveSpeed;
			}
			else if (stickValue.y < -.1)
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

			if (!firstValueAssigned)
			{
				shortestDis = disToTrigger;
				closestFollower = follower;
				firstValueAssigned = true;
			}

			if (disToTrigger < shortestDis)
			{
				shortestDis = disToTrigger;
				closestFollower = follower;
			}
		}
	}

	public void ResetSegmentPositions() //Called from spline trigger
	{
		//BUG: Assuming it has to do with cubes being positioned over the 100% point, which doesn't exist bc it starts at 0 again
		int startIndex = 0;
		double startPos = 0;

		for (int i = 0; i < followers.Length; i++)
		{
			if(followers[i] == closestFollower)
			{
				startIndex = i;
				startPos = followers[i].result.percent;
			}	
		}

		//Starting from startIndex, get the segments with higher list index but lower spline percent. Adjust the distances
		for (int i = startIndex + 1; i < followers.Length; i++)
		{
			double newPos = startPos - spacing * (i - startIndex);
			if(startIndex == 0) newPos -= headSpacing;
			if(newPos <= 0) newPos++;
			followers[i].result.percent = newPos;

			if (i == 19) print(followers[i].result.percent);
		}

		//Starting from startIndex, get the segments with lower list index but higher spline percent. Adjust the distances
		for (int i = startIndex - 1; i >= 0; i--)
		{
			double newPos = startPos - spacing * (i - startIndex);
			if(newPos >= 1) newPos--;
			followers[i].result.percent = newPos;

			if(i == 0) //checking if it's the head to give it some extra space
				followers[i].result.percent += headSpacing;
		}
	}

	private void OnDisable()
	{
		controls.Gameplay.Disable();
	}
}
