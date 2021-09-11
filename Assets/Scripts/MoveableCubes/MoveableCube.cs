using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Qbism.MoveableCubes
{
	public class MoveableCube : MonoBehaviour, IActiveCube
	{
		//Config parameters
		[Header("Becoming Floor")]
		[SerializeField] float shrinkStep = 0f;
		[SerializeField] float shrinkTimeStep = 0f;
		[SerializeField] MeshRenderer shrinkingmesh;
		public LineRenderer laserLine = null;
		[Header("Flip Center")]
		[SerializeField] Transform center = null;
		public Transform up = null;
		public Transform down = null;
		public Transform left = null;
		public Transform right = null;
		[Header("Flipping & Lowering")]
		[SerializeField] int turnStep = 18;
		[SerializeField] float timeStep = 0.01f;
		[SerializeField] float lowerStep = 0.5f;
		[Header("Feedback")]
		[SerializeField] AudioClip landClip = null;
		[SerializeField] Vector3 moveScale = new Vector3( .9f, .9f, .9f);
		[SerializeField] MMFeedbacks shrinkFeedback;
		[SerializeField] float shrinkFeedbackDuration;
		public MeshRenderer mesh;

		//States
		public bool isMoving { get; set;} = false;
		private float yPos = 1f;
		public bool isBoosting { get; set; } = false;
		public bool hasBumped { get; set; } = false;
		public bool isDocked { get; set; } = false;
		public bool isOutOfBounds { get; set; } = false;
		Quaternion resetRot;

		//Actions, events, delegates etc
		public delegate bool KeyCheckDel(Vector2Int pos);
		public KeyCheckDel onWallKeyCheck;
		public delegate bool CheckDel(Vector2Int pos);
		public CheckDel onFloorKeyCheck, onMoveableKeyCheck, onShrunkCheck, onMovingCheck;
		public delegate Vector2Int PlayerPosDelegate();
		public PlayerPosDelegate onPlayerPosCheck;
		
		public event Action<Vector2Int, GameObject, float, float, MMFeedbacks, float, MeshRenderer, MeshRenderer, LineRenderer> onComponentAdd;
		public event Action<Transform, Vector3, Vector2Int, MoveableCube, Vector2Int, Vector2Int, Vector2Int> onFloorCheck;
		public event Action onCheckForNewFloorCubes;
		public event Action<Vector2Int, Vector3, Vector2Int> onActivateOtherMoveable;
		public event Action<Vector2Int, bool> onSetFindable;
		public event Action<Vector2Int> onDicRemove;
		public event Action<MoveableCube, Transform, Vector3, Vector2Int> onActivatePlayerMove;
		public event Action<Vector2Int, bool> onSetShrunk;

		private void Start()
		{
			UpdateCenterPosition();
			yPos = transform.position.y;
			transform.localScale = moveScale;
			resetRot = transform.rotation;
		}

		public void InitiateMove(Transform side, Vector3 turnAxis, Vector2Int posAhead, Vector2Int originPos)
		{
			if (CheckForWallAhead(posAhead) || hasBumped)
			{
				isMoving = false;
				hasBumped = false;
				return;
			}

			Vector2Int prevPos = FetchGridPos();
			StartCoroutine(Move(side, turnAxis, posAhead, originPos, prevPos));
		}

		public IEnumerator Move(Transform side, Vector3 turnAxis, Vector2Int posAhead, 
			Vector2Int originPos, Vector2Int prevPos)
		{	
			isMoving = true;	

			if(onMoveableKeyCheck(posAhead) && !onMovingCheck(posAhead)) //Checking if it's not moving to ensure it's not checking itself in his origin pos
			{
				onActivateOtherMoveable(posAhead, turnAxis, FetchGridPos());
				hasBumped = true;
			} 	

			if(posAhead == onPlayerPosCheck())	//Checking if it bumps into player
			{
				onActivatePlayerMove(this, side, turnAxis, posAhead);
				onSetShrunk(posAhead, true);
				hasBumped = true;
			}

			if(onFloorKeyCheck(posAhead) && !onShrunkCheck(posAhead)) //Normal movement
			{
				for (int i = 0; i < (90 / turnStep); i++)
				{
					transform.RotateAround(side.position, turnAxis, turnStep);
					yield return new WaitForSeconds(timeStep);
				}

				RoundPosition();
				UpdateCenterPosition();

				if (side == up) posAhead += Vector2Int.up;
				else if (side == down) posAhead += Vector2Int.down;
				else if (side == left) posAhead += Vector2Int.left;
				else if (side == right) posAhead += Vector2Int.right;

				CheckFloorInNewPos(side, turnAxis, posAhead, this, FetchGridPos(), originPos, prevPos);
			}

			//Docking
			else if(!onFloorKeyCheck(posAhead) || (onFloorKeyCheck(posAhead) && onShrunkCheck(posAhead))) 
			{
				for (int i = 0; i < (180 / turnStep); i++)
				{
					transform.RotateAround(side.position, turnAxis, turnStep);
					yield return new WaitForSeconds(timeStep);
				}

				transform.localScale = new Vector3(1, 1, 1);
				transform.rotation = resetRot; //reset rotation so shrink anim plays correct way up

				RoundPosition();
				isMoving = false;
				hasBumped = false;
				isDocked = true;

				var cubePos = FetchGridPos();

				if(onFloorKeyCheck(cubePos))
				{
					onSetFindable(cubePos, false);
					onDicRemove(cubePos);
				} 

				onComponentAdd(cubePos, this.gameObject, shrinkStep, shrinkTimeStep, 
					shrinkFeedback, shrinkFeedbackDuration, mesh, shrinkingmesh, laserLine);
				onCheckForNewFloorCubes();
				UpdateCenterPosition();
			}
		}

		public void InitiateLowering(Vector2Int cubePos)
		{
			Vector3 targetPos = new Vector3(transform.position.x,
				transform.position.y - 1, transform.position.z);
			float step = lowerStep * Time.deltaTime;

			StartCoroutine(BecomeFloorByLowering(targetPos, step, cubePos));
		}

		private IEnumerator BecomeFloorByLowering(Vector3 targetPos, float step, 
			Vector2Int cubePos)
		{
			while(transform.position.y > targetPos.y)
			{
				transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
				yield return timeStep;
			}

			transform.localScale = new Vector3(1, 1, 1);
			transform.rotation = resetRot; //reset rotation so shrink anim plays correct way up

			RoundPosition();
			isMoving = false;
			isDocked = true;

			onComponentAdd(cubePos, this.gameObject, shrinkStep, shrinkTimeStep, 
				shrinkFeedback, shrinkFeedbackDuration, mesh, shrinkingmesh, laserLine);
			onCheckForNewFloorCubes();
			UpdateCenterPosition();
		}

		private bool CheckForWallAhead(Vector2Int pos)
		{
			return onWallKeyCheck(pos);
		}

		public void UpdateCenterPosition()
		{
			center.position = transform.position;
			laserLine.transform.position = new Vector3 (transform.position.x, 
				laserLine.transform.position.y, transform.position.z);
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
