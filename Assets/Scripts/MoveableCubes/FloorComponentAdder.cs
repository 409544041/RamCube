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
		public Renderer markOnGround;
		[SerializeField] float markY = -0.495f;
		[SerializeField] CubeRefHolder refs;

		//Actions, events, delegates etc
		public event Action<Vector2Int, FloorCube> onAddToMovFloorDic;

		public void AddComponent(Vector2Int cubePos, GameObject cube, MoveableEffector moveEffector)
		{
			FloorCube newFloor = cube.AddComponent<FloorCube>();

			newFloor.tag = "Environment";
			if (moveEffector == null) newFloor.type = CubeTypes.Shrinking;
			else newFloor.type = moveEffector.effectorType;
			refs.floorCube = newFloor;
			newFloor.refs = refs;

			onAddToMovFloorDic(cubePos, newFloor);
			LaserDottedLineCheck();
			refs.cubeShrink.SetResetData();
			PlaceMarkOnGround();
		}

		private void PlaceMarkOnGround()
		{
			var markPos = new Vector3(refs.movCube.transform.position.x,
				markY, refs.movCube.transform.position.z);
			markOnGround.transform.position = markPos;
			markOnGround.enabled = true;
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
