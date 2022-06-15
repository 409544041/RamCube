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

		//States
		List<Vector2Int> posInBoostPath = new List<Vector2Int>();
		Dictionary<Vector3, Dictionary<LaserCube, bool>> laserCrossPointDic = 
			new Dictionary<Vector3, Dictionary<LaserCube, bool>>();

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
			posInBoostPath.Clear(); 
			PopUpWall popWall = null;
			GameObject wallObject = null;
			bool remainOnBoost = false;
			Vector3 boostTarget = GetBoostTarget(boostMaskPlayer, out wallObject, out remainOnBoost);
			if (wallObject) popWall = wallObject.GetComponent<PopUpWall>();
			GetPosInBoostPath(boostTarget);
			CheckForBoostLaserOverlap(cube);

			mover.isBoosting = true;
			mover.justBoosted = true;

			refs.gcRef.pRef.boostJuicer.PlayBoostJuice(refs.boostDirTrans.transform.forward);

			while (mover.isBoosting && !mover.isOutOfBounds)
			{
				cube.transform.position = Vector3.MoveTowards(cube.transform.position, 
					boostTarget, boostSpeed * Time.deltaTime);
				
				if (popWall && Vector3.Distance(cube.transform.position, boostTarget) < 2f)
					popWall.InitiatePopUp();

				if (laserCrossPointDic.Count > 0)
					StartCoroutine(HandleCrossingLasers(cube));

				if (Vector3.Distance(cube.transform.position, boostTarget) < 0.5f || mover.newInput)
				{
					mover.isBoosting = false;
					cube.transform.position = boostTarget;
				}
			
				yield return null;
			}

			if (laserCrossPointDic.Count > 0)
				StartCoroutine(HandleCrossingLasersForwarded());

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

		private IEnumerator HandleCrossingLasers(GameObject cube)
		{
			foreach (KeyValuePair<Vector3, Dictionary<LaserCube, bool>> pair in laserCrossPointDic)
			{
				var crossPoint = pair.Key;
				var nestedDic = pair.Value;
				LaserCube laser = null;
				bool farted = false;

				foreach (KeyValuePair<LaserCube, bool> nestedPair in nestedDic)
				{
					laser = nestedPair.Key;
					farted = nestedPair.Value;
				}

				if (Vector3.Distance(cube.transform.position, crossPoint) < 0.5f && !farted)
				{
					laser.HandleHittingPlayerInBoost(crossPoint, true);
					nestedDic[laser] = true;
					yield return null;
				}
			}
		}

		private IEnumerator HandleCrossingLasersForwarded()
		{
			foreach (KeyValuePair<Vector3, Dictionary<LaserCube, bool>> pair in laserCrossPointDic)
			{
				var crossPoint = pair.Key;
				var nestedDic = pair.Value;

				foreach (KeyValuePair<LaserCube, bool> nestedPair in nestedDic)
				{
					if (nestedPair.Value == false)
					{
						nestedPair.Key.HandleHittingPlayerInBoost(crossPoint, false);
						yield return null;
					}
				}
			}
		}

		private void GetPosInBoostPath(Vector3 boostTarget)
		{
			var dir = refs.boostDirTrans.transform.forward;
			var dist = Vector3.Distance(boostTarget, transform.position) + 1;
			int distRoundDown = (int)(Math.Floor(dist));

			for (int i = 1; i < distRoundDown; i++)
			{
				Vector3 checkPos = transform.position + (dir * i);
				Vector2Int roundedCheckPos = new Vector2Int
					(Mathf.RoundToInt(checkPos.x), Mathf.RoundToInt(checkPos.z));
				posInBoostPath.Add(roundedCheckPos);
			}
		}

		private void CheckForBoostLaserOverlap(GameObject cube)
		{
			laserCrossPointDic.Clear();
			var lRefs = refs.gcRef.laserRefs;

			foreach (var lRef in lRefs)
			{
				foreach (var lPos in lRef.laser.posInLaserPath)
				{
					foreach (var bPos in posInBoostPath)
					{
						if (bPos != lPos) continue;

						Dictionary<LaserCube, bool> laserDic = new Dictionary<LaserCube, bool>();
						laserDic.Add(lRef.laser, false);

						laserCrossPointDic.Add(new Vector3(bPos.x, 
							cube.transform.position.y, bPos.y), laserDic);
					}
				}
			}
		}

		public IEnumerator ExecuteActionOnFF(GameObject ffCube)
		{
			var ff = ffCube.GetComponent<FeedForwardCube>();

			ff.isBoosting = true;

			GameObject wallObject = null;
			bool remainOnBoost = false;
			Vector3 boostTarget = GetBoostTarget(boostMaskPlayer, out wallObject, out remainOnBoost);

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
			var mover = refs.gcRef.pRef.playerMover;
			movCube.isBoosting = true;

			GameObject wallObject = null;
			bool remainOnBoost = false;
			Vector3 boostTarget = GetBoostTarget(boostMaskMoveable, out wallObject, out remainOnBoost);
			if (!remainOnBoost) movRef.boostJuicer.PlayBoostJuice(refs.boostDirTrans.transform.forward);

			while (movCube.isBoosting && !remainOnBoost)
			{
				cube.transform.position = Vector3.MoveTowards(cube.transform.position,
					boostTarget, boostSpeed * Time.deltaTime);

				if (Vector3.Distance(cube.transform.position, boostTarget) < 0.001f ||
					movCube.newPlayerMove)
				{
					movCube.isBoosting = false;
					cube.transform.position = boostTarget;
				}

				yield return null;
			}

			movCube.isBoosting = false;
			movRef.cubePos.RoundPosition();
			movCube.UpdateCenterPosition();

			Vector2Int cubePos = movRef.cubePos.FetchGridPos();

			CalculateSide(null, movCube, cubePos, ref side, ref turnAxis, ref posAhead);

			movCube.CheckFloorInNewPos(side, turnAxis, posAhead, originPos, launchPos);
		}

		private Vector3 GetBoostTarget(LayerMask mask, out GameObject wallObject, out bool remainOnBoost)
		{
			RaycastHit wallHit;
			Vector3 target = new Vector3(0, 0, 0);

			if (Physics.Raycast(refs.boostRayOrigin.position,
				refs.boostDirTrans.transform.TransformDirection(Vector3.forward), out wallHit, 20, mask, 
				QueryTriggerInteraction.Collide))
			{
				wallObject = wallHit.transform.gameObject;
				var playerTarget = wallObject.tag == "Player";

				//For this to work, wall or other blocker objects needs to be placed on 
				//'the grid', just like cubes
				float distance = Vector3.Distance(refs.boostRayOrigin.position, 
					wallHit.point) - .5f;

				// Probs only situation is when pushing mov directly onto boost
				// and hitting player next to it again	
				if (playerTarget && distance < 1)
				{
					target = refs.boostRayOrigin.position;
					remainOnBoost = true;
					return target;
				}

				target = refs.boostRayOrigin.position + (refs.boostDirTrans.transform.
					TransformDirection(Vector3.forward) * Mathf.RoundToInt(distance));
				remainOnBoost = false;
				return target;
			}

			//this else is for when it flies out of bounds
			else
			{
				target = refs.boostRayOrigin.position + 
					(refs.boostDirTrans.transform.TransformDirection(Vector3.forward) * 30);
				wallObject = null;
				remainOnBoost = false;
				return target;
			}
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
