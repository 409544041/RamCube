﻿using System;
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
		[SerializeField] Transform boostRayOrigin = null;
		[SerializeField] GameObject boostObjDir = null;
		[SerializeField] LayerMask boostMaskPlayer, boostMaskMoveable;

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
			var mover = cube.GetComponent<PlayerCubeMover>();
			var launchPos = mover.FetchGridPos();

			PopUpWall popWall = null;
			GameObject wallObject = null;
			Vector3 boostTarget = GetBoostTarget(boostMaskPlayer, out wallObject);
			if (wallObject) popWall = wallObject.GetComponent<PopUpWall>();

			mover.input = false;
			mover.isBoosting = true;

			mover.GetComponent<PlayerCubeBoostJuicer>().
				PlayBoostJuice(boostObjDir.transform.forward);

			while (mover.isBoosting)
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
				mover.RoundPosition();
				mover.UpdateCenterPosition();

				var cubePos = mover.FetchGridPos();

				Transform side = null;
				Vector3 turnAxis = new Vector3(0, 0, 0);
				Vector2Int posAhead = new Vector2Int(0, 0);

				CalculateSideForPlayer(mover, launchPos, cubePos, ref side, ref turnAxis, ref posAhead);

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
				ff.RoundPosition();
				ff.CheckFloorInNewPos();
			}
		}

		public IEnumerator ExecuteActionOnMoveable(Transform side, Vector3 turnAxis, 
			Vector2Int posAhead, GameObject cube, Vector2Int originPos, FloorCube prevCube)
		{
			var moveable = cube.GetComponent<MoveableCube>();
			Vector2Int launchPos = moveable.FetchGridPos();

			moveable.isBoosting = true;

			GameObject wallObject = null;
			Vector3 boostTarget = GetBoostTarget(boostMaskMoveable, out wallObject);

			while (moveable.isBoosting)
			{

				cube.transform.position = Vector3.MoveTowards(cube.transform.position,
					boostTarget, boostSpeed * Time.deltaTime);

				if (Vector3.Distance(cube.transform.position, boostTarget) < 0.001f)
					moveable.isBoosting = false;

				yield return null;
			}

			moveable.RoundPosition();
			moveable.UpdateCenterPosition();

			Vector2Int cubePos = moveable.FetchGridPos();

			MoveableCubeHandler moveHandler = FindObjectOfType<MoveableCubeHandler>();

			CalculateSideForMoveable(ref side, ref turnAxis, ref posAhead, moveable, launchPos, cubePos, moveHandler);

			moveable.CheckFloorInNewPos(side, turnAxis, posAhead, moveable, cubePos, originPos, launchPos);
		}

		private Vector3 GetBoostTarget(LayerMask mask, out GameObject wallObject)
		{
			RaycastHit wallHit;
			Vector3 target = new Vector3(0, 0, 0);

			if (Physics.Raycast(boostRayOrigin.position,
				boostObjDir.transform.TransformDirection(Vector3.forward), out wallHit, 20, mask))
			{
				//For this to work, wall or other blocker objects needs to be placed on 
				//'the grid', just like cubes
				float distance = Vector3.Distance(boostRayOrigin.position, wallHit.point) - .5f;
				target = boostRayOrigin.position + 
					(boostObjDir.transform.TransformDirection(Vector3.forward) * distance);
				wallObject = wallHit.transform.gameObject;
			}

			//this else is for when it flies out of bounds
			else
			{
				target = boostRayOrigin.position + (boostObjDir.transform.TransformDirection(Vector3.forward) * 30);
				wallObject = null;
			}
			return target;
		}

		private void CalculateSideForPlayer(PlayerCubeMover mover, Vector2Int launchPos,
		Vector2Int cubePos, ref Transform side, ref Vector3 turnAxis, ref Vector2Int posAhead)
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

		private void CalculateSideForMoveable(ref Transform side, ref Vector3 turnAxis, 
			ref Vector2Int posAhead, MoveableCube moveable, Vector2Int launchPos, 
			Vector2Int cubePos, MoveableCubeHandler moveHandler)
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
