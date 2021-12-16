using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using Qbism.WorldMap;

namespace Qbism.Serpent
{
	public class SerpentMapHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float shrinkSpeed;
		public float mapFollowSpeed = 9f;
		[SerializeField] SplineFollower follower;
		[SerializeField] SerpentSegmentHandler segHandler;
		[SerializeField] float sizeAtStart, sizeAtTarget;

		//States
		Transform target;
		SplineComputer spline;

		private void Start() 
		{
			spline = follower.spline;
		}

		public void ActivateSerpent(LevelPinUI pinUI)
		{
			SetSplineToTarget(pinUI);
			SetShrinkingData();
			StartMovement();
		}

		private void SetSplineToTarget(LevelPinUI pinUI)
		{
			target = pinUI.levelPin.pinPather.pathPoint.transform;
			spline.SetPointPosition(0, target.position);
			spline.Rebuild();
		}

		private void SetShrinkingData()
		{
			for (int i = 0; i < segHandler.segments.Length; i++)
			{
				var shrinker = segHandler.segments[i].GetComponent<SegmentShrinker>();
				if (shrinker != null) shrinker.SetTargetData(target, sizeAtStart, sizeAtTarget);
			}
		}

		private void StartMovement()
		{
			follower.followSpeed = mapFollowSpeed;
			GetComponent<SerpentMovement>().SetMoving(true);
			
		}
	}
}
