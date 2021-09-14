using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.MoveableCubes;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerCubeMover : MonoBehaviour, IActiveCube
	{
		//Config parameters
		[Header ("Turn Axis")]
		[SerializeField] Transform center = null;
		public Transform up = null;
		public Transform down = null;
		public Transform left = null;
		public Transform right = null;
		[Header ("Turning")]
		[SerializeField] int turnStep = 18;
		[SerializeField] float timeStep = 0.01f;
		[SerializeField] float lowerStep = 2.5f;
		[Header ("Wiggle")]
		[SerializeField] int wiggleTurnStep = 18;
		[SerializeField] int wiggleRotation = 8;
		[Header ("Position")]
		[SerializeField] float yDuringPlay = .9f;
		[SerializeField] float yWhenLowered = 0;		

		//Cache
		MoveableCubeHandler moveHandler;
		MoveableCube[] moveableCubes;
		PlayerCubeFlipJuicer playerFlipJuicer;
		PlayerAnimator playerAnimator;
		PlayerCubeBoostJuicer boostJuicer;

		//States
		public bool isInBoostPos { get; set; } = true;
		public bool input { get; set; } = true;
		public bool isBoosting { get; set; } = false;
		public bool isTurning { get; set; } = false;
		bool initiatedByPlayer = true;
		public bool isMoving { get; set; } = false;
		public bool lasersInLevel { get; set; } = false;
		private Vector3 startScale = new Vector3(1, 1, 1);
		public bool isStunned { get; set; }	= false;
		public bool isOutOfBounds { get; set; } = false;
		public bool isInIntroSeq { get; set; } = false;
		public bool isLowered { get; set; } = false;

		//Actions, events, delegates etc
		public event Action<Vector2Int> onCubeShrink;
		public event Action<Vector2Int, GameObject, Transform, Vector3, Vector2Int> onFloorCheck;
		public event Action<Vector3, Quaternion, Vector3> onInitialRecord;
		public event Action onInitialFloorCubeRecord;
		public event Action<bool> onSetLaserTriggers;
		public event Action<InterfaceIDs> onRewindPulse;

		private void Awake()
		{
			moveHandler = FindObjectOfType<MoveableCubeHandler>();
			moveableCubes = FindObjectsOfType<MoveableCube>();
			playerFlipJuicer = GetComponent<PlayerCubeFlipJuicer>();
			playerAnimator = GetComponentInChildren<PlayerAnimator>();
			boostJuicer = GetComponent<PlayerCubeBoostJuicer>();
		}

		private void OnEnable() 
		{
			if (moveHandler != null) moveHandler.onSetPlayerInput += SetInput;

			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onPlayerPosCheck += FetchGridPos;
					cube.onActivatePlayerMove += InitiateMoveFromOther;
				}
			}

			if (playerAnimator != null)
			{
				playerAnimator.onInputSet += SetInput;
				playerAnimator.onIntroSet += SetInIntro;
			} 
		}

		private void Start()
		{
			RoundPosition();
			UpdateCenterPosition();
			startScale = transform.localScale;
		}

		public void HandleSwipeInput(Transform side, Vector3 turnAxis, Vector2Int posAhead)
		{
			if (!input) return;
			initiatedByPlayer = true;
			StartCoroutine(Move(side, turnAxis, posAhead));
		}

		public void HandleKeyInput(Transform side, Vector3 turnAxis, Vector2Int posAhead)
		{
			if (!input) return;
			initiatedByPlayer = true;
			StartCoroutine(Move(side, turnAxis, posAhead));
		}

		private void InitiateMoveFromOther(MoveableCube cube, Transform side, Vector3 turnAxis, Vector2Int posAhead)
		{
			SetSide(cube, ref side, ref posAhead);

			initiatedByPlayer = false;
			StartCoroutine(Move(side, turnAxis, posAhead));
		}

		public IEnumerator Move(Transform side, Vector3 turnAxis, Vector2Int posAhead)
		{
			//Added to ensure laser has time to register presence. Can possibly remove when using spherecast instead of raycast
			yield return new WaitForSeconds(.1f); 
	
			var cubeToShrink = FetchGridPos();

			isMoving = true;

			if (initiatedByPlayer)
			{
				onInitialRecord(transform.position, transform.rotation, startScale);
				onInitialFloorCubeRecord();
				moveHandler.InitialRecordMoveables();
			} 

			if(lasersInLevel) onSetLaserTriggers(true);

			ActivateMoveableAhead(posAhead, turnAxis);

			input = false;

			playerFlipJuicer.PlayFlipJuice();
			yield return new WaitForSeconds(playerFlipJuicer.preFlipJuiceDuration);

			for (int i = 0; i < (90 / turnStep); i++)
			{
				transform.RotateAround(side.position, turnAxis, turnStep);
				yield return new WaitForSeconds(timeStep);
			}

			RoundPosition();
			UpdateCenterPosition();
			isMoving = false;
			onCubeShrink(cubeToShrink);

			yield return null;

			CheckFloorInNewPos(side, turnAxis, posAhead);
		}

		public void InitiateWiggle(Transform side, Vector3 turnAxis)
		{
			StartCoroutine(Wiggle(side, turnAxis));
		}

		private IEnumerator Wiggle(Transform side, Vector3 turnAxis)
		{
			for (int i = 0; i < (8 / wiggleTurnStep); i++)
			{
				transform.RotateAround(side.position, turnAxis, wiggleTurnStep);
				yield return new WaitForSeconds(timeStep);
			}

			for (int i = 0; i < (wiggleRotation / wiggleTurnStep); i++)
			{
				transform.RotateAround(side.position, -turnAxis, wiggleTurnStep);
				yield return new WaitForSeconds(timeStep);
			}
		}

		public void InitiateLowering(Vector2Int cubePos)
		{
			Vector3 targetPos = new Vector3(transform.position.x,
				transform.position.y - .95f, transform.position.z);
			float step = lowerStep * Time.deltaTime;

			StartCoroutine(LowerCube(targetPos, step, cubePos));
		}

		private IEnumerator LowerCube(Vector3 targetPos, float step, Vector2Int cubePos)
		{
			isMoving = true;
			input = false;
			
			var juiceDur = boostJuicer.FetchJuiceDur();
			boostJuicer.PlayPostBoostJuice();
			yield return new WaitForSeconds(juiceDur);

			transform.localScale = new Vector3(.95f, .95f, .95f);

			boostJuicer.PlayLoweringSFX();

			while (transform.position.y > targetPos.y)
			{
				transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
				yield return timeStep;
			}

			isLowered = true;
			if (moveHandler.movingMoveables == 0) input = true;
			RoundPosition();
			onRewindPulse(InterfaceIDs.Rewind);
		}

		public void RoundPosition()
		{
			float yPos = 0;
			if (transform.position.y > .5f) yPos = .9f;

			transform.position = new Vector3(Mathf.RoundToInt(transform.position.x),
				yPos, Mathf.RoundToInt(transform.position.z));

			Quaternion rotation = Quaternion.Euler(Mathf.RoundToInt(transform.rotation.x),
				Mathf.RoundToInt(transform.rotation.y), Mathf.RoundToInt(transform.rotation.z));
		}

		public bool CheckForWallAhead(Vector2Int posAhead)
		{
			var currentPos = FetchGridPos();

			if (moveHandler.CheckMoveableCubeDicKey(posAhead))
			{
				var posAheadOfAhead = posAhead + (posAhead - currentPos);
				return moveHandler.CheckForWallAheadOfAhead(posAhead, posAheadOfAhead);
			}
			else return moveHandler.wallCubeDic.ContainsKey(posAhead);
		}

		private void ActivateMoveableAhead(Vector2Int posAhead, Vector3 turnAxis)
		{
			if(moveHandler.CheckMoveableCubeDicKey(posAhead))
			{
				moveHandler.ActivateMoveableCube(posAhead, turnAxis, FetchGridPos());
				moveHandler.movingMoveables++; 
				moveHandler.RemoveFromMoveableDic(posAhead);
			}
		}

		public void CheckFloorInNewPos(Transform side, Vector3 turnAxis, Vector2Int posAhead)
		{
			onFloorCheck(FetchGridPos(), this.gameObject, side, turnAxis, posAhead);
		}

		public void UpdateCenterPosition()
		{
			center.position = transform.position;
		}

		public Vector2Int FetchGridPos()
		{
			Vector2Int roundedPos = new Vector2Int
				(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));

			return roundedPos;
		}

		private void SetInput(bool value)
		{
			input = value;
		}

		private void SetInIntro(bool value)
		{
			isInIntroSeq = value;
		}

		private void SetSide(MoveableCube cube, ref Transform side, ref Vector2Int posAhead)
		{
			if (side == cube.up)
			{
				side = up;
				posAhead += Vector2Int.up;
			}
			if (side == cube.down)
			{
				side = down;
				posAhead += Vector2Int.down;
			}
			if (side == cube.left)
			{
				side = left;
				posAhead += Vector2Int.left;
			}
			if (side == cube.right)
			{
				side = right;
				posAhead += Vector2Int.right;
			}
		}

		public int CheckDeltaX(Vector2Int posA, Vector2Int posB)
		{
			return posA.x - posB.x;
		}

		public int CheckDeltaY(Vector2Int posA, Vector2Int posB)
		{
			return posA.y - posB.y;
		}

		private void OnDisable()
		{
			if (moveHandler != null) moveHandler.onSetPlayerInput -= SetInput;

			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onPlayerPosCheck -= FetchGridPos;
					cube.onActivatePlayerMove -= InitiateMoveFromOther;
				}
			}

			if (playerAnimator != null)
			{
				playerAnimator.onInputSet -= SetInput;
				playerAnimator.onIntroSet -= SetInIntro;
			}
		}
	}
}