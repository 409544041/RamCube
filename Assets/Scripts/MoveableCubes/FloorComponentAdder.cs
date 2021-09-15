using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Qbism.Cubes;
using UnityEngine;

namespace Qbism.MoveableCubes
{
	public class FloorComponentAdder : MonoBehaviour
	{
		//Actions, events, delegates etc
		public event Action<Vector2Int, FloorCube> onAddToMovFloorDic;

		public void AddComponent(Vector2Int cubePos, GameObject cube, float shrinkStep,
			float shrinkTimeStep, MMFeedbacks shrinkFeedback, float shrinkDuration,
			MeshRenderer mesh, MeshRenderer shrinkMesh, LineRenderer laserLine, Vector2Int originPos)
		{
			FloorCube newFloor = cube.AddComponent<FloorCube>();
			CubeShrinker newShrinker = cube.AddComponent<CubeShrinker>();

			newFloor.tag = "Environment";
			newFloor.type = CubeTypes.Shrinking;
			newFloor.laserLine = laserLine;

			newShrinker.shrinkStep = shrinkStep;
			newShrinker.timeStep = shrinkTimeStep;
			newShrinker.mesh = mesh;
			newShrinker.shrinkMesh = shrinkMesh;
			newShrinker.shrinkFeedback = shrinkFeedback;
			newShrinker.shrinkFeedbackDuration = shrinkDuration;

			onAddToMovFloorDic(cubePos, newFloor);
			LaserDottedLineCheck();
		}

		private void LaserDottedLineCheck()
		{
			LaserCube[] lasers = FindObjectsOfType<LaserCube>();
			if (lasers.Length > 0)
			{
				foreach (var laser in lasers)
				{
					laser.CastDottedLines(laser.dist, laser.distance);
				}
			}
		}
	}
}
