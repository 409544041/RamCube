using System.Collections;
using System.Collections.Generic;
using Qbism.MoveableCubes;
using Qbism.PlayerCube;
using UnityEngine;
using UnityEngine.Events;

namespace Qbism.Cubes
{
	public class BoostCube : MonoBehaviour, ICubeInfluencer
	{
		//Config parameters
		[SerializeField] float boostSpeed = 30f;
		[SerializeField] GameObject boostCollider = null;
		[SerializeField] Transform colliderSpawnPos = null;

		public UnityEvent onBoostEvent = new UnityEvent();

		public void PrepareAction(GameObject cube)
		{
			CreateSpawnCollider(cube);

			if (cube.GetComponent<PlayerCubeMover>()) StartCoroutine(ExecuteActionOnPlayer(cube));
			else if (cube.GetComponent<FeedForwardCube>()) StartCoroutine(ExecuteActionOnFF(cube));
		}

		public void PrepareActionForMoveable(Transform side, Vector3 turnAxis, 
			Vector2Int posAhead, GameObject cube, Vector2Int originPos, FloorCube prevCube)
		{
			CreateSpawnCollider(cube);
			StartCoroutine(ExecuteActionOnMoveable(side, turnAxis, posAhead, cube, originPos, prevCube));
		}

		private void CreateSpawnCollider(GameObject cube)
		{
			GameObject spawnedCollider = Instantiate(boostCollider,
							colliderSpawnPos.position, transform.localRotation);

			spawnedCollider.transform.parent = cube.transform;
			spawnedCollider.GetComponent<Collider>().enabled = true;
		}

		public IEnumerator ExecuteActionOnPlayer(GameObject cube)
		{
			var mover = cube.GetComponent<PlayerCubeMover>();
			var launchPos = mover.FetchGridPos();

			mover.input = false;
			mover.isBoosting = true;

			onBoostEvent.Invoke();
			mover.GetComponent<PlayerCubeBoostJuicer>().
				PlayBoostJuice(-transform.forward);

			while (mover.isBoosting)
			{
				cube.transform.position +=
					transform.TransformDirection(Vector3.forward) * boostSpeed * Time.deltaTime;
				yield return null;
			}

			mover.RoundPosition();
			mover.UpdateCenterPosition();

			var cubePos = mover.FetchGridPos();

			Transform side = null;
			Vector3 turnAxis = new Vector3(0, 0, 0);
			Vector2Int posAhead = new Vector2Int(0, 0);
			
			CalculateSide(mover, launchPos, cubePos, ref side, ref turnAxis, ref posAhead);

			mover.CheckFloorInNewPos(side, turnAxis, posAhead);
		}

		private static void CalculateSide(PlayerCubeMover mover, Vector2Int launchPos, Vector2Int cubePos, ref Transform side, ref Vector3 turnAxis, ref Vector2Int posAhead)
		{
			if (mover.CheckDeltaY(cubePos, launchPos) > 0)
			{
				side = mover.up;
				turnAxis = Vector3.right;
				posAhead = cubePos + Vector2Int.up;
			}
			else if (mover.CheckDeltaY(cubePos, launchPos) < 0)
			{
				side = mover.down;
				turnAxis = Vector3.left;
				posAhead = cubePos + Vector2Int.down;
			}
			else if (mover.CheckDeltaX(cubePos, launchPos) < 0)
			{
				side = mover.left;
				turnAxis = Vector3.forward;
				posAhead = cubePos + Vector2Int.left;
			}
			else if (mover.CheckDeltaX(cubePos, launchPos) > 0)
			{
				side = mover.right;
				turnAxis = Vector3.back;
				posAhead = cubePos + Vector2Int.right;
			}
		}

		public IEnumerator ExecuteActionOnFF(GameObject ffCube)
		{
			var ff = ffCube.GetComponent<FeedForwardCube>();

			ff.isBoosting = true;

			while (ff.isBoosting)
			{
				ffCube.transform.position +=
					transform.TransformDirection(Vector3.forward) * boostSpeed * Time.deltaTime;
				yield return null;
			}

			if (ff.gameObject.activeSelf)
			{
				ff.RoundPosition();
				ff.isBoosting = false;

				ff.CheckFloorInNewPos();
			}
		}

		public IEnumerator ExecuteActionOnMoveable(Transform side, Vector3 turnAxis, 
			Vector2Int posAhead, GameObject cube, Vector2Int originPos, FloorCube prevCube)
		{
			var moveable = cube.GetComponent<MoveableCube>();
			Vector2Int launchPos = moveable.FetchGridPos();

			moveable.isBoosting = true;

			while (moveable.isBoosting)
			{
				moveable.transform.position +=
					transform.TransformDirection(Vector3.forward) * boostSpeed * Time.deltaTime;
				yield return null;
			}
			moveable.RoundPosition();
			moveable.UpdateCenterPosition();

			Vector2Int cubePos = moveable.FetchGridPos();

			MoveableCubeHandler moveHandler = FindObjectOfType<MoveableCubeHandler>();

			CalculateSide(ref side, ref turnAxis, ref posAhead, moveable, launchPos, cubePos, moveHandler);

			moveable.CheckFloorInNewPos(side, turnAxis, posAhead, moveable, cubePos, originPos, launchPos);
		}

		private static void CalculateSide(ref Transform side, ref Vector3 turnAxis, ref Vector2Int posAhead, MoveableCube moveable, Vector2Int launchPos, Vector2Int cubePos, MoveableCubeHandler moveHandler)
		{
			if (moveHandler.CheckDeltaY(cubePos, launchPos) > 0)
			{
				side = moveable.up;
				turnAxis = Vector3.right;
				posAhead = cubePos + Vector2Int.up;
			}
			else if (moveHandler.CheckDeltaY(cubePos, launchPos) < 0)
			{
				side = moveable.down;
				turnAxis = Vector3.left;
				posAhead = cubePos + Vector2Int.down;
			}
			else if (moveHandler.CheckDeltaX(cubePos, launchPos) < 0)
			{
				side = moveable.left;
				turnAxis = Vector3.forward;
				posAhead = cubePos + Vector2Int.left;
			}
			else if (moveHandler.CheckDeltaX(cubePos, launchPos) > 0)
			{
				side = moveable.right;
				turnAxis = Vector3.back;
				posAhead = cubePos + Vector2Int.right;
			}
		}
	}
}
