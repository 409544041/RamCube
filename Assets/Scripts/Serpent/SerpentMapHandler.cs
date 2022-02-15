using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using Qbism.WorldMap;
using Qbism.General;

namespace Qbism.Serpent
{
	public class SerpentMapHandler : MonoBehaviour
	{
		//Config parameters
		public float mapFollowSpeed = 9f;
		[SerializeField] SplineFollower follower;
		[SerializeField] SerpentSegmentHandler segHandler;
		[SerializeField] float sizeAtStart, sizeAtTarget;
		[SerializeField] SplineComputer[] splines;

		//States
		Transform target;
		SplineComputer spline;

		public void ActivateSerpent(LevelPinUI pinUI)
		{
			if (E_SegmentsGameplayData.GetEntity(0).f_Rescued == false) return;
			
			SetSpline();
			SetSplineToTarget(pinUI);
			SetShrinkingData();
			PlaceSegments();
			var segmentArray = segHandler.PrepareSegmentsWithBilly();
			segHandler.EnableSegments(segmentArray); 
			StartMovement();
		}

		private void SetSpline()
		{
			var i = Random.Range(0, splines.Length - 1);
			follower.spline = splines[i];
		}

		private void PlaceSegments()
		{
			Vector3 pos = spline.GetPoint(spline.pointCount -1).position;

			for (int i = 0; i < segHandler.segments.Length; i++)
			{
				segHandler.segments[i].position = pos;
			}
		}

		private void SetSplineToTarget(LevelPinUI pinUI)
		{
			spline = follower.spline;
			target = pinUI.levelPin.pinPather.pathPoint.transform;
			spline.SetPointPosition(0, target.position);
			spline.Rebuild();
		}

		private void SetShrinkingData()
		{
			for (int i = 0; i < segHandler.segments.Length; i++)
			{
				var shrinker = segHandler.segments[i].GetComponent<ScreenDistanceShrinker>();
				if (shrinker != null) shrinker.SetTargetData(target.position, sizeAtStart, sizeAtTarget, 0);
			}
		}

		private void StartMovement()
		{
			follower.followSpeed = mapFollowSpeed;
			GetComponent<SerpentMovement>().SetMoving(true);
			
		}
	}
}
