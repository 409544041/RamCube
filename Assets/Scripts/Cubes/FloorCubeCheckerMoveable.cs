using System.Collections;
using System.Collections.Generic;
using Qbism.MoveableCubes;
using UnityEngine;

namespace Qbism.Cubes
{
	public class FloorCubeCheckerMoveable : MonoBehaviour
	{
		//Config parameters
		[SerializeField] GameLogicRefHolder glRef;

		//Cache
		CubeHandler handler;
		MoveableCubeHandler moveHandler;
		MoveableCube[] moveableCubes;

		private void Awake() 
		{
			handler = glRef.cubeHandler;
			moveHandler = glRef.movCubeHandler;
			moveableCubes = glRef.gcRef.movCubes;
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
				FloorCube currentCube = handler.FetchCube(cubePos, true);
				FloorCube prevCube = handler.FetchCube(prevPos, true);

				if (currentCube.FetchType() == CubeTypes.Boosting ||
					(currentCube.FetchType() == CubeTypes.Turning))
				{
					if (prevCube.type == CubeTypes.Boosting &&
						moveHandler.CheckMoveableCubeDicKey(posAhead))
							moveHandler.StartMovingMoveable(posAhead, turnAxis, cubePos);

					currentCube.GetComponent<ICubeInfluencer>().
					PrepareActionForMoveable(side, turnAxis, posAhead, cube.gameObject, originPos, prevCube);
				}

				else if (currentCube.FetchType() == CubeTypes.Shrinking ||
					currentCube.FetchType() == CubeTypes.Static)
				{
					if (prevCube.type == CubeTypes.Boosting &&
						moveHandler.CheckMoveableCubeDicKey(posAhead))
					{
						moveHandler.StartMovingMoveable(posAhead, turnAxis, cubePos);
						cube.hasBumped = true;
					}

					cube.InitiateMove(side, turnAxis, posAhead, originPos);
				}

				else if (currentCube.FetchType() == CubeTypes.Finish)
					moveHandler.StopMovingMoveables(cubePos, cube, false);
			}

			else cube.InitiateLowering(cubePos);
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
