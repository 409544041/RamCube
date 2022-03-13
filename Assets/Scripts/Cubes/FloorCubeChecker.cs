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
		public event Action onCheckForFinish;

		private void Awake() 
		{
			//TO DO: Add player refs
			mover = FindObjectOfType<PlayerCubeMover>();
			handler = glRef.cubeHandler;
			wallHandler = glRef.wallHandler;
			moveHandler = glRef.movCubeHandler;
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
			currentCube = handler.FetchCube(mover.cubePoser.FetchGridPos(), true);
		}

		private void CheckFloorType(Vector2Int cubePos, GameObject cube,
			Transform side, Vector3 turnAxis, Vector2Int posAhead)
		{
			FloorCube previousCube;

			previousCube = currentCube;

			if (previousCube.FetchType() == CubeTypes.Static)
				previousCube.GetComponent<StaticCube>().BecomeShrinkingCube(cube);

			HandleMovingMoveableFromBoost(cubePos, turnAxis, posAhead, previousCube);

			if (handler.floorCubeDic.ContainsKey(cubePos)
				|| handler.movFloorCubeDic.ContainsKey(cubePos))
			{
				currentCube = handler.FetchCube(cubePos, true);
				bool differentCubes = currentCube != previousCube;

				if (currentCube.FetchType() == CubeTypes.Boosting && differentCubes)
					currentCube.GetComponent<ICubeInfluencer>().PrepareAction(cube);

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

					mover.GetComponent<PlayerFartLauncher>().ResetFartCollided();
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

				if (!wallHandler.CheckForWallAheadOfAhead(posAhead, posAheadOfAhead))
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

			currentCube.GetComponent<ICubeInfluencer>().PrepareAction(cube);
			mover.GetComponent<PlayerCubeTurnJuicer>().PlayTurningVoice();
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
