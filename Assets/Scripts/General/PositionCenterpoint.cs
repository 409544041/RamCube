using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using UnityEngine;

namespace Qbism.General
{
	public class PositionCenterpoint : MonoBehaviour
	{
		//Config parameters
		[SerializeField] bool isDynamicCam;

		//Cache
		CubeHandler handler;

		//States
		float highest, lowest, leftest, rightest;
		bool firstValueAssigned;
		Vector3 centerPoint = new Vector3(0, 0, 0);
		Vector2 highestCube, lowestCube, leftestCube, rightestCube;

		private void Awake() 
		{
			handler = FindObjectOfType<CubeHandler>();
		}
		
		private void Start()
		{
			FindEdges();
			PositionCenterPoint();
		}

		private void Update() 
		{
			if (isDynamicCam) 
			{
				FindEdgeCubes();
				PositionCenterPoint();
			}
		}

		private void PositionCenterPoint()
		{
			transform.position = centerPoint;

			if (!isDynamicCam)
			{
				CamCentralizer centralizer = GetComponent<CamCentralizer>();
				if (centralizer) centralizer.PositionCam();
			}
		}

		private void FindEdges()
		{
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

				CalculateCenterpoint();
			}

			firstValueAssigned = false;
		}

		private void FindEdgeCubes()
		{
			foreach (KeyValuePair<Vector2Int, FloorCube> cube in handler.floorCubeDic)
			{
				if (cube.Value == null) return;
				
				Vector2 viewPortPos = Camera.main.WorldToViewportPoint(cube.Value.transform.position);

				if (!firstValueAssigned)
				{
					highestCube = viewPortPos;
					lowestCube = viewPortPos;
					leftestCube = viewPortPos;
					rightestCube = viewPortPos;

					firstValueAssigned = true;
				}

				if (viewPortPos.y > highestCube.y) highestCube = viewPortPos;
				if (viewPortPos.y < lowestCube.y) lowestCube = viewPortPos;
				if (viewPortPos.x < leftestCube.x) leftestCube = viewPortPos;
				if (viewPortPos.x > rightestCube.x) rightestCube = viewPortPos;

				CalculateAvgPoint(highestCube, lowestCube, leftestCube, rightestCube);
			}

			firstValueAssigned = false;
		}

		private void CalculateCenterpoint()
		{
			float deltaX = rightest - leftest;
			float deltaY = highest - lowest;

			float xPos = leftest + (deltaX / 2);
			float yPos = lowest + (deltaY / 2);

			centerPoint = Camera.main.ViewportToWorldPoint(new Vector3(xPos, yPos, 15)); 
			centerPoint = new Vector3(centerPoint.x, 0, centerPoint.z);
		}

		private void CalculateAvgPoint(Vector2 highCube, Vector2 lowCube, Vector2 leftCube, Vector2 rightCube)
		{
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			Vector2 PlayerPortPos = Camera.main.WorldToViewportPoint(player.transform.position);

			Vector2[] PortPositions = {highCube, lowCube, leftCube, rightCube, PlayerPortPos};

			var totalX = 0f;
			var totalY = 0f;

			foreach (var pos in PortPositions)
			{
				totalX += pos.x;
				totalY += pos.y;
			}

			var centerX = totalX / PortPositions.Length;
			var centerY = totalY / PortPositions.Length;

			centerPoint = Camera.main.ViewportToWorldPoint(new Vector3(centerX, centerY, 15));
			centerPoint = new Vector3(centerPoint.x, 0, centerPoint.z);
		}
	}
}
