using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qbism.PlayerCube;
using Qbism.MoveableCubes;
using Qbism.SpriteAnimations;
using MoreMountains.Feedbacks;

namespace Qbism.Cubes
{
	public class TurningCube : MonoBehaviour, ICubeInfluencer
	{
		//Config parameters
		[SerializeField] int turnStep = 6;
		[SerializeField] float timeStep = 0.01f;
		public bool isLeftTurning = false;
		public CubeRefHolder refs;

		//Cache
		PlayerCubeMover mover;

		//States
		public Vector3 turnAxis { get; set; } = new Vector3(0, 0, 0);

		private void Awake()
		{
			mover = FindObjectOfType<PlayerCubeMover>();
		}

		public void PrepareAction(GameObject cube)
		{
			if (cube.GetComponent<PlayerCubeMover>()) StartCoroutine(ExecuteActionOnPlayer(cube));
			else if (cube.GetComponent<FeedForwardCube>()) StartCoroutine(ExecuteActionOnFF(cube));
		}

		public void PrepareActionForMoveable(Transform side, Vector3 turnAxis, 
			Vector2Int posAhead, GameObject cube, Vector2Int originPos, FloorCube prevCube)
		{
			StartCoroutine(ExecuteActionOnMoveable(side, turnAxis, posAhead, cube, originPos, prevCube));
		}

		public IEnumerator ExecuteActionOnPlayer(GameObject cube)
		{
			mover.input = false;
			mover.isTurning = true;

			var axis = transform.TransformDirection(turnAxis);

			refs.turnJuice.PlayFeedbacks();

			mover.GetComponentInChildren<ExpressionHandler>().
				SetSituationFace(ExpressionSituations.turning, 1f);

			for (int i = 0; i < (90 / turnStep); i++)
			{
				cube.transform.Rotate(axis, turnStep, Space.World);
				yield return new WaitForSeconds(timeStep);
			}

			mover.cubePoser.RoundPosition();
			mover.UpdateCenterPosition();

			Transform side = null;
			Vector2Int posAhead = new Vector2Int(0, 0);

			mover.CheckFloorInNewPos(side, turnAxis, posAhead);
			mover.isTurning = false;
		}

		public IEnumerator ExecuteActionOnFF(GameObject ffCube)
		{
			var ff = ffCube.GetComponent<FeedForwardCube>();

			var axis = transform.TransformDirection(turnAxis);

			for (int i = 0; i < (90 / turnStep); i++)
			{
				if (ff.cubePoser.FetchGridPos() == refs.cubePos.FetchGridPos())
				{
					ffCube.transform.Rotate(axis, turnStep, Space.World);
					yield return null;
				}
				else yield break;
			}

			ff.cubePoser.RoundPosition();
		}

		public IEnumerator ExecuteActionOnMoveable(Transform side, Vector3 movingTurnAxis,
		Vector2Int posAhead, GameObject cube, Vector2Int originPos, FloorCube prevCube)
		{
			var movRef = cube.GetComponent<CubeRefHolder>();
			var movCube = movRef.movCube;
			var cubePos = movRef.cubePos.FetchGridPos();
			var moveEffector = movRef.movEffector;

			var axis = transform.TransformDirection(turnAxis);

			if (moveEffector != null)
			{
				moveEffector.UpdateFacePos();
				moveEffector.ToggleEffectFace(true);
				moveEffector.ParentFaceToMoveable();
			}

			refs.turnJuice.PlayFeedbacks();

			for (int i = 0; i < (90 / turnStep); i++)
			{
				cube.transform.Rotate(axis, turnStep, Space.World);
				yield return new WaitForSeconds(timeStep);
			}

			movRef.cubePos.RoundPosition();
			movCube.UpdateCenterPosition();
			CalculateSide(ref side, ref movingTurnAxis, ref posAhead, movCube, cubePos);

			if (moveEffector != null) moveEffector.UnParentFace();

			if (prevCube.FetchType() != CubeTypes.Boosting)
				movCube.InitiateMove(side, movingTurnAxis, posAhead, originPos);
			else 
			{
				var moveHandler = refs.gcRef.glRef.movCubeHandler;
				moveHandler.StopMovingMoveables(cubePos, movCube, false);
			}

		}

		private void CalculateSide(ref Transform side, ref Vector3 movingTurnAxis, ref Vector2Int posAhead, MoveableCube moveable, Vector2Int cubePos)
		{
			if (isLeftTurning)
			{
				if (side == moveable.up)
				{
					side = moveable.left;
					movingTurnAxis = Vector3.forward;
					posAhead = cubePos + Vector2Int.left;
				}
				else if (side == moveable.down)
				{
					side = moveable.right;
					movingTurnAxis = Vector3.back;
					posAhead = cubePos + Vector2Int.right;
				}
				else if (side == moveable.left)
				{
					side = moveable.down;
					movingTurnAxis = Vector3.left;
					posAhead = cubePos + Vector2Int.down;
				}
				else if (side == moveable.right)
				{
					side = moveable.up;
					movingTurnAxis = Vector3.right;
					posAhead = cubePos + Vector2Int.up;
				}
			}
			else
			{
				if (side == moveable.up)
				{
					side = moveable.right;
					movingTurnAxis = Vector3.back;
					posAhead = cubePos + Vector2Int.right;
				}
				else if (side == moveable.down)
				{
					side = moveable.left;
					movingTurnAxis = Vector3.forward;
					posAhead = cubePos + Vector2Int.left;
				}
				else if (side == moveable.left)
				{
					side = moveable.up;
					movingTurnAxis = Vector3.right;
					posAhead = cubePos + Vector2Int.up;
				}
				else if (side == moveable.right)
				{
					side = moveable.down;
					movingTurnAxis = Vector3.left;
					posAhead = cubePos + Vector2Int.down;
				}
			}
		}
	}
}
