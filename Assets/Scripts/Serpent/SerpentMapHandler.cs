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
		[SerializeField] float sizeAtStart, sizeAtTarget;
		[SerializeField] MapCoreRefHolder mapCoreRef;

		//States
		Transform target;
		SplineComputer currentSpline;

		public void ActivateSerpent(LevelPinUI pinUI)
		{
			if (E_SegmentsGameplayData.GetEntity(0).f_Rescued == false) return;
			
			SetSpline();
			SetSplineToTarget(pinUI);
			SetShrinkingData();
			PlaceSegments();
			var segmentArray = mapCoreRef.serpSegHandler.PrepareSegmentsWithBilly();
			mapCoreRef.serpSegHandler.EnableSegments(segmentArray); 
			StartMovement();
		}

		private void SetSpline()
		{
			var i = Random.Range(0, mapCoreRef.splines.Length - 1);
			mapCoreRef.splineFollower.spline = mapCoreRef.splines[i];
		}

		private void PlaceSegments()
		{
			Vector3 pos = currentSpline.GetPoint(currentSpline.pointCount -1).position;

			for (int i = 0; i < mapCoreRef.serpSegHandler.segments.Length; i++)
			{
				mapCoreRef.serpSegHandler.segments[i].position = pos;
			}
		}

		private void SetSplineToTarget(LevelPinUI pinUI)
		{
			currentSpline = mapCoreRef.splineFollower.spline;
			target = pinUI.levelPin.pinPather.pathPoint.transform;
			currentSpline.SetPointPosition(0, target.position);
			currentSpline.Rebuild();
		}

		private void SetShrinkingData()
		{
			for (int i = 0; i < mapCoreRef.serpSegHandler.segments.Length; i++)
			{
				var shrinker = mapCoreRef.serpSegHandler.segments[i].GetComponent<ScreenDistanceShrinker>();
				if (shrinker != null) shrinker.SetTargetData(target.position, sizeAtStart, sizeAtTarget, 0);
			}
		}

		private void StartMovement()
		{
			mapCoreRef.splineFollower.followSpeed = mapFollowSpeed;
			GetComponent<SerpentMovement>().SetMoving(true);
			
		}
	}
}
