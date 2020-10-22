using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.MoveableCubes
{
	public class MoveableCube : MonoBehaviour, IActiveCube
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
		[SerializeField] float lowerStep = 0.5f;
		[SerializeField] AudioClip landClip = null;

		//States
		public bool canMove { get; set;} = true;
		public bool isMoving { get; set;} = false;
		private float yPos = 1f;
		public bool isBoosting { get; set; } = false;

		public delegate bool KeyCheckDelegate(Vector3Int pos);
		public KeyCheckDelegate onWallKeyCheck;

		public delegate bool FloorKeyCheckDelegate(Vector2Int pos);
		public FloorKeyCheckDelegate onFloorKeyCheck;

		public event Action<Vector2Int, GameObject, float, float> onComponentAdd;
		public event Action<Vector2Int> onDictionaryRemove;
		public event Action<Vector2Int, MoveableCube> onDictionaryAdd;
		public event Action<Transform, Vector3, Vector2Int, MoveableCube, Vector2Int, Vector2Int, Vector2Int> onFloorCheck;
		public event Action<MoveableCube, Vector3, Quaternion, Vector3> onInitialRecord;
		public event Action<MoveableCube> onRecordStop;
		public event Action onCheckForNewFloorCubes;

		private void Start()
		{
			UpdateCenterPosition();
			yPos = transform.position.y;
		}

		public void InitiateMove(Transform side, Vector3 turnAxis, Vector2Int posAhead, Vector2Int originPos)
		{
			if (CheckForWallAhead(posAhead))
			{
				isMoving = false;
				onDictionaryRemove(originPos);
				onDictionaryAdd(FetchGridPos(), this);
				return;
			}
			Vector2Int prevPos = FetchGridPos();
			StartCoroutine(Move(side, turnAxis, posAhead, originPos, prevPos));
		}

		public IEnumerator Move(Transform side, Vector3 turnAxis, Vector2Int posAhead, 
			Vector2Int originPos, Vector2Int prevPos)
		{
			onInitialRecord(this, transform.position, transform.rotation, transform.localScale);
			
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

				CheckFloorInNewPos(side, turnAxis, posAhead, this, FetchGridPos(), originPos, prevPos);
			}

			else if(!onFloorKeyCheck(posAhead)) //Become floorcube by moving
			{
				for (int i = 0; i < (180 / turnStep); i++)
				{
					transform.RotateAround(side.position, turnAxis, turnStep);
					yield return new WaitForSeconds(timeStep);
				}

				RoundPosition();
				isMoving = false;

				onComponentAdd(posAhead, this.gameObject, shrinkStep, shrinkTimeStep);
				onDictionaryRemove(originPos);
				onCheckForNewFloorCubes();
				onRecordStop(this);
			}
		}

		public void InitiateLowering(Vector2Int cubePos, Vector2Int originPos)
		{
			Vector3 targetPos = new Vector3(transform.position.x,
				transform.position.y - 1, transform.position.z);
			float step = lowerStep * Time.deltaTime;

			StartCoroutine(BecomeFloorByLowering(targetPos, step, cubePos, originPos));
		}

		private IEnumerator BecomeFloorByLowering(Vector3 targetPos, float step, 
			Vector2Int cubePos, Vector2Int originPos)
		{
			while(transform.position.y > targetPos.y)
			{
				transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
				yield return timeStep;
			}

			RoundPosition();
			isMoving = false;

			onComponentAdd(cubePos, this.gameObject, shrinkStep, shrinkTimeStep);
			onDictionaryRemove(originPos);
			onCheckForNewFloorCubes();
			onRecordStop(this);
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

		public void CheckFloorInNewPos(Transform side, Vector3 turnAxis, Vector2Int posAhead,
			MoveableCube cube, Vector2Int cubePos, Vector2Int originPos, Vector2Int prevPos)
		{
			onFloorCheck(side, turnAxis, posAhead, this, FetchGridPos(), originPos, prevPos);
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
