using System.Collections;
using System.Collections.Generic;
using Qbism.WorldMap;
using UnityEngine;

namespace Qbism.General
{
	public class LineDrawer : MonoBehaviour
	{
		//Config parameters
		public LineTypes lineType = LineTypes.full;
		[SerializeField] float drawSpeed = 5f;
		[SerializeField] float lineWidth = .15f;

		//Cache
		LineRenderer lRender;

		//States
		public bool drawing { get; set; } = false;
		float counter = 0;
		float distance = 0;
		public Transform origin { get; set; }
		public LevelIDs destinationID { get; private set; }
		public Transform destination { get; set; }
		Vector3 pointAlongLine;
		public int pointToMove = 0;

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

				pointAlongLine = x * Vector3.Normalize(destination.position - 
				origin.position) + origin.position;

				lRender.SetPosition(pointToMove, pointAlongLine);
			}

			if (lRender.startWidth == 0 && lRender.endWidth == 0)
				SetLineWidth(lineWidth);

			if (Vector3.Distance(pointAlongLine, destination.position) <= .1) 
			{
				LevelPinUI[] pinUIs = FindObjectsOfType<LevelPinUI>();

				foreach (LevelPinUI pinUI in pinUIs)
				{
					if(pinUI.levelPin.levelID == destinationID)
						pinUI.DisableLockIcon(); 
				}
			}
		}

		public void SetPositions(Transform incOrigin, Transform incDestination, 
			bool retracting)
		{
			origin = incOrigin;
			destination = incDestination;
			destinationID = incDestination.GetComponentInParent<LevelPin>().levelID;

			lRender.positionCount = 2;
			lRender.SetPosition(0, origin.position);

			if (drawing)
			{
				if(retracting) lRender.SetPosition(1, destination.position);
				distance = Vector3.Distance(origin.position, destination.position);
			}
				
			
			if (!drawing)
			{
				lRender.SetPosition(1, destination.position);
				SetLineWidth(lineWidth);
			}
		}

		private void SetLineWidth(float width)
		{
			lRender.startWidth = width;
			lRender.endWidth = width;
		}
	}
}
