﻿using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.MoveableCubes
{
	public class MoveableCube : MonoBehaviour, IActiveCube, IMovingCube
	{
		//Config parameters
		[SerializeField] CubeTypes type = CubeTypes.Shrinking;
		[SerializeField] float shrinkStep = 0f;
		[SerializeField] float shrinkTimeStep = 0f;
		[SerializeField] Transform center = null;
		public Transform up = null;
		public Transform down = null;
		public Transform left = null;
		public Transform right = null;
		[SerializeField] int turnStep = 18;
		[SerializeField] float timeStep = 0.01f;
		[SerializeField] AudioClip landClip = null;

		//States
		public bool canMove { get; set;} = true;
		public bool isMoving { get; set;} = false;
		private float yPos = .5f;

		public delegate bool KeyCheckDelegate(Vector3Int pos);
		public KeyCheckDelegate onWallKeyCheck;

		public delegate bool FloorKeyCheckDelegate(Vector2Int pos);
		public FloorKeyCheckDelegate onFloorKeyCheck;

		public event Action<Vector2Int, GameObject, float, float> onComponentAdd;
		public event Action<Vector2Int> onDictionaryRemove;
		public event Action<Transform, Vector3, Vector2Int, MoveableCube, Vector2Int> onFloorCheck;

		private void Start()
		{
			UpdateCenterPosition();
			yPos = transform.position.y;
		}

		public void InitiateMove(Transform side, Vector3 turnAxis, Vector2Int posAhead)
		{
			if (CheckForWallAhead(posAhead))
			{
				isMoving = false;
				return;
			}
			StartCoroutine(Move(side, turnAxis, posAhead));
		}

		public IEnumerator Move(Transform side, Vector3 turnAxis, Vector2Int posAhead)
		{
			isMoving = true;

			if(onFloorKeyCheck(posAhead))
			{
				for (int i = 0; i < (90 / turnStep); i++)
				{
					transform.RotateAround(side.position, turnAxis, turnStep);
					yield return new WaitForSeconds(timeStep);
				}

				RoundPosition();
				UpdateCenterPosition();

				if (side == up) posAhead = posAhead + Vector2Int.up;
				else if (side == down) posAhead = posAhead + Vector2Int.down;
				else if (side == left) posAhead = posAhead + Vector2Int.left;
				else if (side == right) posAhead = posAhead + Vector2Int.right;

				onFloorCheck(side, turnAxis, posAhead, this, FetchGridPos());
			}

			else if(!onFloorKeyCheck(posAhead))
			{
				for (int i = 0; i < (180 / turnStep); i++)
				{
					transform.RotateAround(side.position, turnAxis, turnStep);
					yield return new WaitForSeconds(timeStep);
				}

				RoundPosition();
				isMoving = false;

				onComponentAdd(posAhead, this.gameObject, shrinkStep, shrinkTimeStep);
				onDictionaryRemove(FetchGridPos());

			}
		}

		private bool CheckForWallAhead(Vector2Int pos)
		{
			Vector3Int posAhead = 
				new Vector3Int(pos.x, Mathf.RoundToInt(yPos), pos.y);

			return onWallKeyCheck(posAhead);
		}

		public void UpdateCenterPosition()
		{
			center.position = transform.position;
		}

		public void CheckFloorInNewPos()
		{
			throw new System.NotImplementedException();
		}

		public Vector2Int FetchGridPos()
		{
			Vector2Int roundedPos = new Vector2Int
				(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));

			return roundedPos;
		}

		public void RoundPosition()
		{
			transform.position = new Vector3(Mathf.RoundToInt(transform.position.x),
				Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));

			Quaternion rotation = Quaternion.Euler(Mathf.RoundToInt(transform.rotation.x),
				Mathf.RoundToInt(transform.rotation.y), Mathf.RoundToInt(transform.rotation.z));
		}

		public void PlayLandClip()
		{
			AudioSource.PlayClipAtPoint(landClip, Camera.main.transform.position, .05f);
		}
	}
}
