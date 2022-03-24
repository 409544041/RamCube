 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using System;
using Qbism.WorldMap;

namespace Qbism.Serpent
{
	public class SerpentMovement : MonoBehaviour
	{
		//Config parameters
		[SerializeField] SegmentRefHolder head = null;
		public float segmentSpacing = 1f;
		[SerializeField] MapCoreRefHolder mcRef;
		[SerializeField] SerpentSegmentHandler segHandler;

		//States
		List<Vector3> breadcrumbs = null;
		bool isMoving = false;
		SegmentRefHolder[] segments = null;
		bool firstCrumbs = false;


		private void Awake() 
		{
			breadcrumbs = new List<Vector3>();
		}

        private void Start()
        {
			if (mcRef != null) segments = segHandler.PrepareSegmentsWithBilly();
			else segments = segHandler.segRefs;
		}

        void Update()
		{
			if (isMoving) FollowHead();
		}

		private void FollowHead()
		{
			if (!firstCrumbs)
			{
				//populate the first set of crumbs by the initial positions of the segments.
				//add head first, because that's where the segments will be going.
				breadcrumbs.Add(head.transform.position);

				//we have an extra-crumb to mark where the last segment was...
				//so it's segment.length and not segment.length - 1
				for (int i = 0; i < segments.Length; i++)
					breadcrumbs.Add(segments[i].transform.position);
				
				firstCrumbs = true;
			}

			float headDisplacement = (head.transform.position - breadcrumbs[0]).magnitude;

			if (headDisplacement >= segmentSpacing)
			{
				breadcrumbs.RemoveAt(breadcrumbs.Count - 1);
				breadcrumbs.Insert(0, head.transform.position);
				headDisplacement = headDisplacement % segmentSpacing;
			}

			if (headDisplacement != 0)
			{
				Vector3 pos;

				for (int i = 1; i < segments.Length; i++)
				{
					pos = Vector3.Lerp(breadcrumbs[i + 1], breadcrumbs[i], headDisplacement / segmentSpacing);
					segments[i].transform.position = pos;

					//Need to check for != Vector3.zero to avoid Vector3.zero errors
					if (breadcrumbs[i] - breadcrumbs[i + 1] != Vector3.zero && breadcrumbs[i - 1] - breadcrumbs[i] != Vector3.zero)
						segments[i].transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(breadcrumbs[i] - breadcrumbs[i + 1]),
						Quaternion.LookRotation(breadcrumbs[i - 1] - breadcrumbs[i]), headDisplacement / segmentSpacing);
				}
			}
		}

		public void SetMoving(bool value)
		{
			isMoving = value;
		}
	}
}