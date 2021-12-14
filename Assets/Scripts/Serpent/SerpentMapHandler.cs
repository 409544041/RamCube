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

		public void StartShrinkingSerpent()
		{
			StartCoroutine(ShrinkSerpent());
		}

		private IEnumerator ShrinkSerpent()
		{
			var segHandler = GetComponent<SerpentSegmentHandler>();

			for (int i = 0; i < segHandler.segments.Length; i++)
			{
				var segment = segHandler.segments[i];

				while (!Mathf.Approximately(segment.transform.localScale.x, 0))
				{
					var size = Mathf.MoveTowards(segment.transform.localScale.x, 0, shrinkSpeed);
					segment.transform.localScale = new Vector3(size, size, size);
					yield return null;
				}
			}
		}

		public void StartMovement()
		{
			follower.followSpeed = mapFollowSpeed;
		}

		public void SetSplineDestinationPoint()
		{
			var target = FindObjectOfType<PinSelectionTracker>().selectedPin.
				pinUI.levelPin.pinPather.pathPoint;
			spline.SetPointPosition(0, target.transform.position);
			spline.Rebuild();
		}
	}
}
