using System.Collections;
using System.Collections.Generic;
using Qbism.WorldMap;
using UnityEngine;

namespace Qbism.General
{
	public class LineDrawer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float drawSpeed = 5f;
		[SerializeField] float lineWidth = .15f;

		//Cache
		LineRenderer lRender;

		//States
		public bool drawing { get; set; } = false;
		float counter = 0;
		float distance = 0;
		public Transform origin { get; set; }
		public Transform destination { get; set; }

		private void Awake()
		{
			lRender = GetComponent<LineRenderer>();
			SetLineWidth(0);
		}

		private void Update()
		{
			AnimateLineDrawing();
		}

		private void AnimateLineDrawing()
		{
			if (!drawing) return;

			if (counter < distance)
			{
				counter += .1f / drawSpeed;
				float x = Mathf.Lerp(0, distance, counter);

				Vector3 pointAlongLine = x *
					Vector3.Normalize(destination.position - origin.position)
					+ origin.position;

				lRender.SetPosition(1, pointAlongLine);
			}

			if (lRender.startWidth == 0 && lRender.endWidth == 0)
				SetLineWidth(lineWidth);
		}

		public void SetPositions(Transform incOrigin, Transform incDestination)
		{
			origin = incOrigin;
			destination = incDestination;

			lRender.positionCount = 2;
			lRender.SetPosition(0, origin.position);
			if (drawing)
			{
				lRender.SetPosition(0, origin.position);
				distance = Vector3.Distance(origin.position, destination.position);
			}
			if (!drawing)
			{
				lRender.SetPosition(1, destination.position);
				SetLineWidth(lineWidth);
			}
		}

		private void InitiateDrawPath(Transform origin, Transform destination)
		{
			if(drawing)return;
			drawing = true;
			SetPositions(origin, destination);
		}

		private void SetLineWidth(float width)
		{
			lRender.startWidth = width;
			lRender.endWidth = width;
		}
	}
}
