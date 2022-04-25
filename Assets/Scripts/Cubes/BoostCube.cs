using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.Environment;
using Qbism.MoveableCubes;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.Cubes
{
	public class BoostCube : MonoBehaviour, ICubeInfluencer
	{
		//Config parameters
		[Header ("Boosting")]
		[SerializeField] float boostSpeed = 30f;
		public LayerMask boostMaskPlayer, boostMaskMoveable;
		public CubeRefHolder refs;

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
			var mover = refs.gcRef.pRef.playerMover;

			PopUpWall popWall = null;
			GameObject wallObject = null;
			Vector3 boostTarget = GetBoostTarget(boostMaskPlayer, out wallObject);
			if (wallObject) popWall = wallObject.GetComponent<PopUpWall>();

			mover.input = false;
			mover.isBoosting = true;
			mover.justBoosted = true;

			refs.gcRef.pRef.boostJuicer.PlayBoostJuice(refs.boostDirTrans.transform.forward);

			while (mover.isBoosting && !mover.isOutOfBounds)
			{
				cube.transform.position = Vector3.MoveTowards(cube.transform.position, 
					boostTarget, boostSpeed * Time.deltaTime);
				
				if (popWall && Vector3.Distance(cube.transform.position, boostTarget) < 2f)
					popWall.InitiatePopUp();
				
				if (Vector3.Distance(cube.transform.position, boostTarget) < 0.75f)
				{
					mover.isBoosting = false;
					cube.transform.position = boostTarget;
				}
			
				yield return null;
			}

			if (!mover.isOutOfBounds)
			{
				refs.gcRef.pRef.cubePos.RoundPosition();
				mover.UpdateCenterPosition();

				var cubePos = refs.gcRef.pRef.cubePos.FetchGridPos();

				Transform side = null;
				Vector3 turnAxis = new Vector3(0, 0, 0);
				Vector2Int posAhead = new Vector2Int(0, 0);

				CalculateSide(mover, null, cubePos, ref side, ref turnAxis, ref posAhead);

				mover.CheckFloorInNewPos(side, turnAxis, posAhead);
			}
		}

		public IEnumerator ExecuteActionOnFF(GameObject ffCube)
		{
			var ff = ffCube.GetComponent<FeedForwardCube>();

			ff.isBoosting = true;

			GameObject wallObject = null;
			Vector3 boostTarget = GetBoostTarget(boostMaskPlayer, out wallObject);

			while (ff.isBoosting)
			{
				ffCube.transform.position = Vector3.MoveTowards(ffCube.transform.position,
					boostTarget, boostSpeed * Time.deltaTime);

				if (Vector3.Distance(ffCube.transform.position, boostTarget) < .5f)
				{
					ff.isBoosting = false;
					ff.transform.position = boostTarget;
				}

				yield return null;
			}

			if (!ff.isOutOfBounds)
			{
				ff.cubePoser.RoundPosition();
				ff.CheckFloorInNewPos();
			}
		}

		public IEnumerator ExecuteActionOnMoveable(Transform side, Vector3 turnAxis, 
			Vector2Int posAhead, GameObject cube, Vector2Int originPos, FloorCube prevCube)
		{
			var movRef = cube.GetComponent<CubeRefHolder>();
			var movCube = movRef.movCube;
			Vector2Int launchPos = movRef.cubePos.FetchGridPos();

			movCube.isBoosting = true;

			GameObject wallObject = null;
			Vector3 boostTarget = GetBoostTarget(boostMaskMoveable, out wallObject);

			while (movCube.isBoosting)
			{
				cube.transform.position = Vector3.MoveTowards(cube.transform.position,
					boostTarget, boostSpeed * Time.deltaTime);

				if (Vector3.Distance(cube.transform.position, boostTarget) < 0.001f)
					movCube.isBoosting = false;

				yield return null;
			}

			movRef.cubePos.RoundPosition();
			movCube.UpdateCenterPosition();

			Vector2Int cubePos = movRef.cubePos.FetchGridPos();

			CalculateSide(null, movCube, cubePos, ref side, ref turnAxis, ref posAhead);

			movCube.CheckFloorInNewPos(side, turnAxis, posAhead, originPos, launchPos);
		}

		private Vector3 GetBoostTarget(LayerMask mask, out GameObject wallObject)
		{
			RaycastHit wallHit;
			Vector3 target = new Vector3(0, 0, 0);

			if (Physics.Raycast(refs.boostRayOrigin.position,
				refs.boostDirTrans.transform.TransformDirection(Vector3.forward), out wallHit, 20, mask, 
				QueryTriggerInteraction.Collide))
			{
				//For this to work, wall or other blocker objects needs to be placed on 
				//'the grid', just like cubes
				float distance = Vector3.Distance(refs.boostRayOrigin.position, wallHit.point) - .5f;
				target = refs.boostRayOrigin.position + 
					(refs.boostDirTrans.transform.TransformDirection(Vector3.forward) * distance);
				wallObject = wallHit.transform.gameObject;
			}

			//this else is for when it flies out of bounds
			else
			{
				target = refs.boostRayOrigin.position + 
					(refs.boostDirTrans.transform.TransformDirection(Vector3.forward) * 30);
				wallObject = null;
			}
			return target;
		}

		private void CalculateSide(PlayerCubeMover mover, MoveableCube moveable, Vector2Int cubePos, 
			ref Transform side, ref Vector3 turnAxis, ref Vector2Int posAhead)
		{
			if (V3Equal(refs.boostDirTrans.transform.forward, Vector3.forward))
			{
				if (mover) side = mover.up;
				if (moveable) side = moveable.up;
				turnAxis = Vector3.right;
				posAhead = cubePos + Vector2Int.up;
			}
			else if (V3Equal(refs.boostDirTrans.transform.forward, Vector3.back))
			{
				if (mover) side = mover.down;
				if (moveable) side = moveable.down;
				turnAxis = Vector3.left;
				posAhead = cubePos + Vector2Int.down;
			}
			else if (V3Equal(refs.boostDirTrans.transform.forward, Vector3.left))
			{
				if (mover) side = mover.left;
				if (moveable) side = moveable.left;
				turnAxis = Vector3.forward;
				posAhead = cubePos + Vector2Int.left;
			}
			else if (V3Equal(refs.boostDirTrans.transform.forward, Vector3.right))
			{
				if (mover) side = mover.right;
				if (moveable) side = moveable.right;
				turnAxis = Vector3.back;
				posAhead = cubePos + Vector2Int.right;
			}
		}

		private bool V3Equal(Vector3 a, Vector3 b)
		{
			return Vector3.SqrMagnitude(a - b) < 0.001;
		}
	}
}
