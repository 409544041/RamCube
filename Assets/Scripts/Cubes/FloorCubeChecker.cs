using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.Environment;
using Qbism.MoveableCubes;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.Cubes
{
	public class FloorCubeChecker : MonoBehaviour
	{
		//Cache
		PlayerCubeBoostJuicer playerBoostJuicer = null;
		PlayerCubeMover mover = null;
		CubeHandler handler = null;
		WallHandler wallHandler = null;
		MoveableCubeHandler moveHandler = null;
		PlayerCubeFeedForward cubeFF = null;
		PlayerCubeFlipJuicer playerFlipJuicer = null;
		FeedForwardCube[] ffCubes = null;

		//States
		public FloorCube currentCube { get; set; } = null;

		//Actions, events, delegates etc
		public event Action onLand;

		private void Awake() 
		{
			mover = FindObjectOfType<PlayerCubeMover>();
			handler = GetComponent<CubeHandler>();
			wallHandler = GetComponent<WallHandler>();
			moveHandler = GetComponent<MoveableCubeHandler>();
			cubeFF = FindObjectOfType<PlayerCubeFeedForward>();
			playerFlipJuicer = mover.GetComponent<PlayerCubeFlipJuicer>();
			playerBoostJuicer = mover.GetComponent<PlayerCubeBoostJuicer>();
			ffCubes = FindObjectsOfType<FeedForwardCube>();
		}

		private void OnEnable() 
		{
			if (mover != null) mover.onFloorCheck += CheckFloorType;

			if (ffCubes != null)
			{
				foreach (FeedForwardCube ffCube in ffCubes)
				{
					ffCube.onFeedForwardFloorCheck += CheckFloorTypeForFF;
				}
			}
		}

		private void Start()
		{
			currentCube = handler.FetchCube(mover.FetchGridPos());
		}

		private void CheckFloorType(Vector2Int cubePos, GameObject cube,
			Transform side, Vector3 turnAxis, Vector2Int posAhead)
		{
			FloorCube previousCube;

			previousCube = currentCube;

			if (previousCube.FetchType() == CubeTypes.Static)
				previousCube.GetComponent<StaticCube>().BecomeShrinkingCube(cube);

			if (previousCube.FetchType() == CubeTypes.Boosting &&
				moveHandler.CheckMoveableCubeDicKey(posAhead))
			{
				var posAheadOfAhead = posAhead + (posAhead - cubePos);

				if (!wallHandler.CheckForWallAheadOfAhead(posAhead, posAheadOfAhead))
				{
					moveHandler.ActivateMoveableCube(posAhead, turnAxis, cubePos);
					moveHandler.movingMoveables++;
					moveHandler.RemoveFromMoveableDic(posAhead);
				}
			}

			if (handler.floorCubeDic.ContainsKey(cubePos)
				|| handler.movFloorCubeDic.ContainsKey(cubePos))
			{
				currentCube = handler.FetchCube(cubePos);
				bool differentCubes = currentCube != previousCube;

				if (currentCube.FetchType() == CubeTypes.Boosting)
					currentCube.GetComponent<ICubeInfluencer>().PrepareAction(cube);

				else if ((currentCube.FetchType() == CubeTypes.Turning) && differentCubes)
				{
					if (onLand != null) onLand();
					currentCube.GetComponent<ICubeInfluencer>().PrepareAction(cube);
					mover.GetComponent<PlayerCubeTurnJuicer>().PlayTurningVoice();
				}

				else
				{
					if (differentCubes && onLand != null)
					{
						if (!mover.isStunned) cubeFF.ShowFeedForward();
						if (moveHandler.movingMoveables == 0) mover.input = true;
						onLand();

						if (previousCube.FetchType() != CubeTypes.Boosting)
							playerFlipJuicer.PlayPostFlipJuice();

						else playerBoostJuicer.PlayPostBoostJuice();

					}
					else
					{
						//landing on same cube, like after having turned/flipped
						if (!mover.isStunned) cubeFF.ShowFeedForward();
						if (moveHandler.movingMoveables == 0) mover.input = true;
					}

					mover.GetComponent<PlayerFartLauncher>().ResetFartCollided();
				}
			}

			else
			{
				//lowering
				if (previousCube.FetchType() == CubeTypes.Boosting)
					mover.InitiateLowering(cubePos);
			}
		}

		private void CheckFloorTypeForFF(Vector2Int cubePos, GameObject cube)
		{
			FloorCube currentCube = handler.FetchCube(cubePos);

			if (currentCube.FetchType() == CubeTypes.Boosting ||
				currentCube.FetchType() == CubeTypes.Turning)
				currentCube.GetComponent<ICubeInfluencer>().PrepareAction(cube);
		}

		private void OnDisable()
		{
			if (mover != null) mover.onFloorCheck -= CheckFloorType;

			if (ffCubes != null)
			{
				foreach (FeedForwardCube ffCube in ffCubes)
				{
					ffCube.onFeedForwardFloorCheck -= CheckFloorTypeForFF;
				}
			}
		}
	}
}
