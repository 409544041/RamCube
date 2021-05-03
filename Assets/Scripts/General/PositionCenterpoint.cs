using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using UnityEngine;

namespace Qbism.General
{
	public class PositionCenterpoint : MonoBehaviour
	{
		//States
		float highest, lowest, leftest, rightest;
		bool firstValueAssigned = false;
		Vector3 centerPoint = new Vector3(0, 0, 0);
		
		void Start()
		{
			PositionCenterPoint();
		}

		private void PositionCenterPoint()
		{
			FindEdges();
			CalculateCenterpoint();

			transform.position = centerPoint;

			CamCentralizer centralizer = GetComponent<CamCentralizer>();
			if(centralizer) centralizer.PositionCam();
		}

		private void FindEdges()
		{
			CubeHandler handler = FindObjectOfType<CubeHandler>();

				foreach (KeyValuePair<Vector2Int, FloorCube> cube in handler.floorCubeDic)
				{
					Vector2 viewPortPos = Camera.main.WorldToViewportPoint(cube.Value.transform.position);

					if (!firstValueAssigned)
					{
						highest = viewPortPos.y;
						lowest = viewPortPos.y;
						leftest = viewPortPos.x;
						rightest = viewPortPos.x;

						firstValueAssigned = true;
					}

					if (viewPortPos.y > highest) highest = viewPortPos.y;
					if (viewPortPos.y < lowest) lowest = viewPortPos.y;
					if (viewPortPos.x < leftest) leftest = viewPortPos.x;
					if (viewPortPos.x > rightest) rightest = viewPortPos.x;
				}
			}

			private void CalculateCenterpoint()
			{
				float deltaX = rightest - leftest;
				float deltaY = highest - lowest;

				float xPos = leftest + (deltaX / 2);
				float yPos = lowest + (deltaY / 2);

				centerPoint = Camera.main.ViewportToWorldPoint(new Vector3(xPos, yPos, 10));
			}
	}
}
