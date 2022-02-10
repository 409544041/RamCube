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
		//Config parameters
		[SerializeField] CubeShrinker shrinker = null;

		//Actions, events, delegates etc
		public event Action<Vector2Int, FloorCube> onAddToMovFloorDic;

		public void AddComponent(Vector2Int cubePos, GameObject cube, 
			LineRenderer laserLine, CubePositioner cubePoser, MoveableEffector moveEffector)
		{
			FloorCube newFloor = cube.AddComponent<FloorCube>();

			newFloor.tag = "Environment";
			if (moveEffector == null) newFloor.type = CubeTypes.Shrinking;
			else newFloor.type = moveEffector.effectorType;
			newFloor.laserLine = laserLine;
			newFloor.cubePoser = cubePoser;

			onAddToMovFloorDic(cubePos, newFloor);
			LaserDottedLineCheck();
			shrinker.SetResetData();
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
