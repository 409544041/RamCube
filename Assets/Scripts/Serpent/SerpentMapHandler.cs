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
		public float mapFollowSpeed = 9, mapFollowSpeedAtTarget = 1;
		[SerializeField] float sizeAtStart, sizeAtTarget;
		[SerializeField] MapCoreRefHolder mcRef;

		//States
		Transform target;
		SplineComputer currentSpline;

		public void ActivateSerpent(LevelPinRefHolder pin)
		{
			if (E_SegmentsGameplayData.GetEntity(0).f_Rescued == false) return;
			mcRef.mlRef.input.allowInput = false;
			
			SetSpline();
			SetSplineToTarget(pin);
			SetShrinkingData();
			PlaceSegments();
			var segmentArray = mcRef.serpSegHandler.PrepareSegmentsWithBilly();
			mcRef.serpSegHandler.EnableSegments(segmentArray); 
			StartMovement();
		}

		private void SetSpline()
		{
			var i = Random.Range(0, mcRef.splines.Length - 1);
			mcRef.splineFollower.spline = mcRef.splines[i];
		}

		private void PlaceSegments()
		{
			Vector3 pos = currentSpline.GetPoint(currentSpline.pointCount -1).position;

			for (int i = 0; i < mcRef.serpSegHandler.segRefs.Length; i++)
			{
				mcRef.serpSegHandler.segRefs[i].transform.position = pos;
			}
		}

		private void SetSplineToTarget(LevelPinRefHolder pin)
		{
			currentSpline = mcRef.splineFollower.spline;
			target = pin.transform;
			currentSpline.SetPointPosition(0, target.position);
			currentSpline.Rebuild();
		}

		private void SetShrinkingData()
		{
			for (int i = 0; i < mcRef.serpSegHandler.segRefs.Length; i++)
			{
				var shrinker = mcRef.serpSegHandler.segRefs[i].distShrinker;
				if (shrinker != null) shrinker.SetTargetData(target.position, sizeAtStart, sizeAtTarget, 0,
					mapFollowSpeed, mapFollowSpeedAtTarget);
			}
		}

		private void StartMovement()
		{
			mcRef.splineFollower.followSpeed = mapFollowSpeed;
			mcRef.serpMover.SetMoving(true);
			
		}
	}
}
