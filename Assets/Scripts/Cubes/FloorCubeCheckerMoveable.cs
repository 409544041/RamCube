using System.Collections;
using System.Collections.Generic;
using Qbism.MoveableCubes;
using UnityEngine;

namespace Qbism.Cubes
{
	public class FloorCubeCheckerMoveable : MonoBehaviour
	{
		//Cache
		CubeHandler handler = null;
		MoveableCubeHandler moveHandler = null;
		MoveableCube[] moveableCubes = null;

		private void Awake() 
		{
			handler = GetComponent<CubeHandler>();
			moveHandler = GetComponent<MoveableCubeHandler>();
			moveableCubes = FindObjectsOfType<MoveableCube>();
		}

		private void OnEnable() 
		{
			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onFloorCheck += CheckFloorTypeForMoveable;
				}
			}
		}

		private void CheckFloorTypeForMoveable(Transform side, Vector3 turnAxis, Vector2Int posAhead,
			MoveableCube cube, Vector2Int cubePos, Vector2Int originPos, Vector2Int prevPos)
		{
			if (handler.floorCubeDic.ContainsKey(cubePos)
				|| handler.movFloorCubeDic.ContainsKey(cubePos))
			{
				FloorCube currentCube = handler.FetchCube(cubePos);
				FloorCube prevCube = handler.FetchCube(prevPos);

				if (currentCube.FetchType() == CubeTypes.Boosting ||
					(currentCube.FetchType() == CubeTypes.Turning))
				{
					if (prevCube.type == CubeTypes.Boosting &&
						moveHandler.CheckMoveableCubeDicKey(posAhead))
						moveHandler.ActivateMoveableCube(posAhead, turnAxis, cubePos);

					currentCube.GetComponent<ICubeInfluencer>().
					PrepareActionForMoveable(side, turnAxis, posAhead, cube.gameObject, originPos, prevCube);
				}

				else if (currentCube.FetchType() == CubeTypes.Shrinking ||
					currentCube.FetchType() == CubeTypes.Static)
				{
					if (prevCube.type == CubeTypes.Boosting &&
						moveHandler.CheckMoveableCubeDicKey(posAhead))
					{
						moveHandler.ActivateMoveableCube(posAhead, turnAxis, cubePos);
						moveHandler.movingMoveables++;
						cube.hasBumped = true;
					}

					cube.InitiateMove(side, turnAxis, posAhead, originPos);
				}
			}

			else cube.InitiateLowering(cubePos, originPos);
		}

		private void OnDisable()
		{
			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onFloorCheck -= CheckFloorTypeForMoveable;
				}
			}
		}
	}
}
