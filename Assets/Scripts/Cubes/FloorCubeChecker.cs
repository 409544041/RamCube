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
		//Config parameters
		[SerializeField] GameLogicRefHolder glRef;

		//Cache
		PlayerCubeBoostJuicer playerBoostJuicer;
		PlayerCubeMover mover;
		CubeHandler handler;
		MoveableCubeHandler moveHandler;
		PlayerCubeFeedForward cubeFF;
		PlayerCubeFlipJuicer playerFlipJuicer;
		FeedForwardCube[] ffCubes;
		PlayerRefHolder player;

		//States
		public FloorCube currentCube { get; set; } = null;

		//Actions, events, delegates etc
		public event Action onCheckForFinish; //TO DO: Check where this ref goes

		private void Awake() 
		{
			player = glRef.gcRef.pRef;
			mover = player.playerMover;
			handler = glRef.cubeHandler;
			moveHandler = glRef.movCubeHandler;
			cubeFF = player.playerFF;
			playerFlipJuicer = player.flipJuicer;
			playerBoostJuicer = player.boostJuicer;
			ffCubes = player.ffCubes;
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
			currentCube = handler.FetchCube(player.cubePos.FetchGridPos(), true);
		}

		private void CheckFloorType(Vector2Int cubePos, GameObject cube,
			Transform side, Vector3 turnAxis, Vector2Int posAhead)
		{
			FloorCube previousCube;

			previousCube = currentCube;

			if (previousCube.FetchType() == CubeTypes.Static)
				previousCube.refs.staticCube.BecomeShrinkingCube();

			HandleMovingMoveableFromBoost(cubePos, turnAxis, posAhead, previousCube);

			if (handler.floorCubeDic.ContainsKey(cubePos)
				|| handler.movFloorCubeDic.ContainsKey(cubePos))
			{
				currentCube = handler.FetchCube(cubePos, true);
				bool differentCubes = currentCube != previousCube;

				if (currentCube.FetchType() == CubeTypes.Boosting && differentCubes)
					currentCube.refs.boostCube.PrepareAction(cube);

				else if ((currentCube.FetchType() == CubeTypes.Turning) && differentCubes)
					StartCoroutine(HandleTurning(cube, previousCube));

				else
				{
					if (differentCubes) HandleLandingOnFinalPos(previousCube);
					else
					{
						//landing on same cube, like after having turned
						if (!mover.isStunned) cubeFF.ShowFeedForward();
						if (moveHandler.movingMoveables == 0) mover.input = true;
						if (previousCube.FetchType() == CubeTypes.Boosting)
							playerBoostJuicer.PlayPostBoostJuice();
					}

					player.fartLauncher.ResetFartCollided();
					mover.justBoosted = false;
				}
			}

			else
			{
				//lowering
				if (previousCube.FetchType() == CubeTypes.Boosting)
					mover.InitiateLowering(cubePos, true);
				else mover.InitiateLowering(cubePos, false);

				mover.justBoosted = false;
			}
		}

		private void HandleMovingMoveableFromBoost(Vector2Int cubePos, Vector3 turnAxis, Vector2Int posAhead, FloorCube previousCube)
		{
			if (previousCube.FetchType() == CubeTypes.Boosting &&
				moveHandler.CheckMoveableCubeDicKey(posAhead))
			{
				var posAheadOfAhead = posAhead + (posAhead - cubePos);

				if (!glRef.wallHandler.CheckForWallAheadOfAhead(posAhead, posAheadOfAhead))
					moveHandler.StartMovingMoveable(posAhead, turnAxis, cubePos);
			}
		}

		private IEnumerator HandleTurning(GameObject cube, FloorCube previousCube)
		{
			if (previousCube.FetchType() == CubeTypes.Boosting)
			{
				playerBoostJuicer.PlayPostBoostJuice();
				var feedbackDur = playerBoostJuicer.feedbackDur;
				yield return new WaitForSeconds(feedbackDur);
			}

			currentCube.refs.turnCube.PrepareAction(cube);
			player.turnJuicer.PlayTurningVoice();
		}

		private void HandleLandingOnFinalPos(FloorCube previousCube)
		{
			if (!mover.isStunned) cubeFF.ShowFeedForward();
			if (moveHandler.movingMoveables == 0) mover.input = true;
			onCheckForFinish();

			if (previousCube.FetchType() != CubeTypes.Boosting)
				playerFlipJuicer.PlayPostFlipJuice();
			else if (mover.justBoosted) playerBoostJuicer.PlayPostBoostJuice();
		}

		private void CheckFloorTypeForFF(Vector2Int cubePos, GameObject cube)
		{
			FloorCube currentCube = handler.FetchCube(cubePos, true);

			if (currentCube.FetchType() == CubeTypes.Boosting)
				currentCube.refs.boostCube.PrepareAction(cube);

			else if (currentCube.FetchType() == CubeTypes.Turning)
				currentCube.refs.turnCube.PrepareAction(cube);
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
