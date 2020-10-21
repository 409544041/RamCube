using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Qbism.PlayerCube;
using Qbism.MoveableCubes;
using System;

namespace Qbism.Cubes
{
	public class TurningCube : MonoBehaviour, ICubeInfluencer
	{
		//Config parameters
		[SerializeField] int turnStep = 9;
		[SerializeField] float timeStep = 0.01f;
		[SerializeField] bool isLeftTurning = false;
		[SerializeField] GameObject topPlane = null;

		//Cache
		PlayerCubeMover mover;

		//States
		Vector3 turnAxis = new Vector3(0, 0, 0);

		public UnityEvent onTurnEvent = new UnityEvent();

		private void Awake()
		{
			mover = FindObjectOfType<PlayerCubeMover>();

			if(isLeftTurning)
			{
				topPlane.transform.localScale = new Vector3 (-1, 1, 1);
				turnAxis = Vector3.down;
			} 
			else turnAxis = Vector3.up;
		}

		public void PrepareAction(GameObject cube)
		{
			if (cube.GetComponent<PlayerCubeMover>()) StartCoroutine(ExecuteActionOnPlayer(cube));
			else if (cube.GetComponent<FeedForwardCube>()) StartCoroutine(ExecuteActionOnFF(cube));
		}

		public void PrepareActionForMoveable(Transform side, Vector3 turnAxis, 
			Vector2Int posAhead, GameObject cube, Vector2Int originPos)
		{
			StartCoroutine(ExecuteActionOnMoveable(side, turnAxis, posAhead, cube, originPos));
		}

		public IEnumerator ExecuteActionOnPlayer(GameObject cube)
		{
			mover.input = false;
			cube.GetComponent<Rigidbody>().isKinematic = true;

			var axis = transform.TransformDirection(turnAxis);

			onTurnEvent.Invoke();

			for (int i = 0; i < (90 / turnStep); i++)
			{
				cube.transform.Rotate(axis, turnStep, Space.World);
				yield return new WaitForSeconds(timeStep);
			}

			mover.RoundPosition();
			mover.UpdateCenterPosition();

			cube.GetComponent<Rigidbody>().isKinematic = false;

			mover.CheckFloorInNewPos();
		}

		public IEnumerator ExecuteActionOnFF(GameObject ffCube)
		{
			var ff = ffCube.GetComponent<FeedForwardCube>();

			var axis = transform.TransformDirection(turnAxis);

			for (int i = 0; i < (90 / turnStep); i++)
			{
				ffCube.transform.Rotate(axis, turnStep, Space.World);
				yield return null;
			}

			ff.RoundPosition();
		}

		public IEnumerator ExecuteActionOnMoveable(Transform side, Vector3 movingTurnAxis,
		Vector2Int posAhead, GameObject cube, Vector2Int originPos)
		{
			var moveable = cube.GetComponent<MoveableCube>();
			var cubePos = moveable.FetchGridPos();

			var axis = transform.TransformDirection(turnAxis);

			onTurnEvent.Invoke();

			for (int i = 0; i < (90 / turnStep); i++)
			{
				cube.transform.Rotate(axis, turnStep, Space.World);
				yield return new WaitForSeconds(timeStep);
			}

			moveable.RoundPosition();
			moveable.UpdateCenterPosition();

			if(isLeftTurning)
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

			if (cube.GetComponent<FloorCube>().FetchType() != CubeTypes.Boosting)
			{
				moveable.InitiateMove(side, movingTurnAxis, posAhead, originPos);
			}				
		}
	}
}
