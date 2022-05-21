using System.Collections;
using System.Collections.Generic;
using Qbism.MoveableCubes;
using Qbism.PlayerCube;
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
		PlayerRefHolder pRef;

		private void Awake() 
		{
			handler = glRef.cubeHandler;
			moveHandler = glRef.movCubeHandler;
			moveableCubes = glRef.gcRef.movCubes;
			pRef = glRef.gcRef.pRef;
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

				if (currentCube.FetchType() == CubeTypes.Boosting && currentCube != prevCube)
				{
					HandleBoostingIntoMoveable(turnAxis, posAhead, cubePos, prevCube);

					if (prevCube.FetchType() == CubeTypes.Boosting && posAhead == pRef.cubePos.FetchGridPos())
						pRef.playerMover.InitiateMoveFromOther(cube, side, turnAxis, posAhead);

					currentCube.GetComponent<ICubeInfluencer>().PrepareActionForMoveable(side, turnAxis,
						posAhead, cube.gameObject, originPos, prevCube);
				}

				//if remaining on boost cube bc boost direction is blocked
				if (currentCube.FetchType() == CubeTypes.Boosting && currentCube == prevCube)
					StartCoroutine(HandleRemainOnBoost(currentCube, side, turnAxis, posAhead, cube,
						originPos, prevCube));

				if (currentCube.FetchType() == CubeTypes.Turning)
				{
					HandleBoostingIntoMoveable(turnAxis, posAhead, cubePos, prevCube);

					currentCube.GetComponent<ICubeInfluencer>().PrepareActionForMoveable(side, turnAxis,
						posAhead, cube.gameObject, originPos, prevCube);
				}

				else if (currentCube.FetchType() == CubeTypes.Shrinking ||
					currentCube.FetchType() == CubeTypes.Static)
				{
					if (prevCube.type == CubeTypes.Boosting)
					{
						if (moveHandler.CheckMoveableCubeDicKey(posAhead))
						{
							moveHandler.StartMovingMoveable(posAhead, turnAxis, cubePos);
							cube.hasBumped = true;
						}

						if (posAhead == pRef.cubePos.FetchGridPos())
						{
							pRef.playerMover.InitiateMoveFromOther(cube, side, turnAxis, posAhead);
							cube.hasBumped = true;
						}
					}

					cube.InitiateMove(side, turnAxis, posAhead, originPos);
				}

				else if (currentCube.FetchType() == CubeTypes.Finish)
					moveHandler.StopMovingMoveables(cubePos, cube, false);
			}

			else cube.InitiateLowering(cubePos);
		}

		private void HandleBoostingIntoMoveable(Vector3 turnAxis, Vector2Int posAhead, Vector2Int cubePos, FloorCube prevCube)
		{
			if (prevCube.type == CubeTypes.Boosting && moveHandler.CheckMoveableCubeDicKey(posAhead))
				moveHandler.StartMovingMoveable(posAhead, turnAxis, cubePos);
		}

		private IEnumerator HandleRemainOnBoost(FloorCube currentCube, Transform side, Vector3 turnAxis,
			Vector2Int posAhead, MoveableCube cube, Vector2Int originPos, FloorCube prevCube)
		{
			while (pRef.playerMover.isMoving)
				yield return null;

			if (posAhead == pRef.cubePos.FetchGridPos())
				pRef.playerMover.InitiateMoveFromOther(cube, side, turnAxis, posAhead);

			//TO DO: Add initiating moveable?

			currentCube.GetComponent<ICubeInfluencer>().PrepareActionForMoveable(side, turnAxis,
				posAhead, cube.gameObject, originPos, prevCube);
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
