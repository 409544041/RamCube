using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Qbism.Cubes;
using UnityEngine;

namespace Qbism.General
{
	public class PositionCenterpoint : MonoBehaviour
	{
		//Config paramters
		[SerializeField] CinemachineVirtualCamera gameplayCam;
		//Cache
		CubeHandler handler;

		//States
		bool firstValueAssigned;
		Vector3 centerPoint = new Vector3(0, 0, 0);
		Vector3 highCube, lowCube, leftCube, rightCube;

		private void Awake() 
		{
			handler = FindObjectOfType<CubeHandler>();
		}

		private void Start() 
		{
			FindEdgeCubes();
		}

		private void Update() 
		{
			CalculateAvgPoint(highCube, lowCube, leftCube, rightCube);
			PositionCenterPoint();
		}

		private void PositionCenterPoint()
		{
			transform.position = centerPoint;
		}

		private void FindEdgeCubes()
		{
			Vector2 highCubePort = new Vector2(0, 0), lowCubePort = new Vector2(0, 0), 
				leftCubePort = new Vector2(0, 0), rightCubePort = new Vector2(0, 0);	

			foreach (KeyValuePair<Vector2Int, FloorCube> cube in handler.floorCubeDic)
			{
				if (cube.Value == null) return;
				
				Vector2 viewPortPos = Camera.main.WorldToViewportPoint(cube.Value.transform.position);

				if (!firstValueAssigned)
				{
					highCube = cube.Value.transform.position;
					lowCube = cube.Value.transform.position;
					leftCube = cube.Value.transform.position;
					rightCube = cube.Value.transform.position;

					highCubePort = viewPortPos;
					lowCubePort = viewPortPos;
					leftCubePort = viewPortPos;
					rightCubePort = viewPortPos;

					firstValueAssigned = true;
				}

				if (viewPortPos.y > highCubePort.y)
				{
					highCubePort = viewPortPos;
					highCube = cube.Value.transform.position;
				} 
				if (viewPortPos.y < lowCubePort.y)
				{
					lowCubePort = viewPortPos;
					lowCube = cube.Value.transform.position;
				} 
				if (viewPortPos.x < leftCubePort.x)
				{
					leftCubePort = viewPortPos;
					leftCube = cube.Value.transform.position;
				} 
				if (viewPortPos.x > rightCubePort.x)
				{
					rightCubePort = viewPortPos;
					rightCube = cube.Value.transform.position;
				} 
			}

			firstValueAssigned = false;
		}

		private void CalculateAvgPoint(Vector3 highCube, Vector3 lowCube, Vector3 leftCube, Vector3 rightCube)
		{
			Vector2 highCubePort = Camera.main.WorldToViewportPoint(highCube); 
			Vector2 lowCubePort = Camera.main.WorldToViewportPoint(lowCube);
			Vector2 leftCubePort = Camera.main.WorldToViewportPoint(leftCube);
			Vector2 rightCubePort = Camera.main.WorldToViewportPoint(rightCube);			

			GameObject player = GameObject.FindGameObjectWithTag("Player");
			Vector2 PlayerPortPos = Camera.main.WorldToViewportPoint(player.transform.position);

			Vector2[] PortPositions = {highCubePort, lowCubePort, leftCubePort, rightCubePort, PlayerPortPos};

			var totalX = 0f;
			var totalY = 0f;

			foreach (var pos in PortPositions)
			{
				totalX += pos.x;
				totalY += pos.y;
			}

			var centerX = totalX / PortPositions.Length;
			var centerY = totalY / PortPositions.Length;

			var dist = gameplayCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance; 
			centerPoint = Camera.main.ViewportToWorldPoint(new Vector3(centerX, centerY, dist));
			centerPoint = new Vector3(centerPoint.x, 0, centerPoint.z);
		}
	}
}
