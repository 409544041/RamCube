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
		[SerializeField] SplineComputer spline;
		public float mapFollowSpeed = 9f;
		[SerializeField] SplineFollower follower;
		[SerializeField] SerpentSegmentHandler segHandler;
		[SerializeField] float sizeAtStart, sizeAtTarget;

		//States
		Transform target;

		public void ActivateSerpent()
		{
			SetSplineToTarget();
			SetShrinkingData();
			StartMovement();
		}

		private void SetSplineToTarget()
		{
			target = FindObjectOfType<PinSelectionTracker>().selectedPin.
				pinUI.levelPin.pinPather.pathPoint.transform;
			spline.SetPointPosition(0, target.position);
			spline.Rebuild();
		}

		private void SetShrinkingData()
		{
			for (int i = 0; i < segHandler.segments.Length; i++)
			{
				segHandler.segments[i].GetComponent<SegmentShrinker>().SetTargetData(target,
					sizeAtStart, sizeAtTarget);
			}
		}

		private void StartMovement()
		{
			follower.followSpeed = mapFollowSpeed;
			GetComponent<SerpentMovement>().SetMoving(true);
			
		}
	}
}
