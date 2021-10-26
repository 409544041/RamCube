﻿using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using Qbism.Environment;
using Qbism.MoveableCubes;
using Qbism.SpriteAnimations;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerCubeMover : MonoBehaviour
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
		[SerializeField] float loweredYDelta = .95f;		
		public CubePositioner cubePoser = null;

		//Cache
		MoveableCubeHandler moveHandler;
		WallHandler wallHandler;
		MoveableCube[] moveableCubes;
		PlayerCubeFlipJuicer playerFlipJuicer;
		PlayerAnimator playerAnimator;
		PlayerCubeBoostJuicer boostJuicer;
		ExpressionHandler expresHandler;

		//States
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
		public bool justBoosted { get; set; } = false;

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
			wallHandler = moveHandler.GetComponent<WallHandler>();
			moveableCubes = FindObjectsOfType<MoveableCube>();
			playerFlipJuicer = GetComponent<PlayerCubeFlipJuicer>();
			playerAnimator = GetComponentInChildren<PlayerAnimator>();
			boostJuicer = GetComponent<PlayerCubeBoostJuicer>();
			expresHandler = GetComponentInChildren<ExpressionHandler>();
		}

		private void OnEnable() 
		{
			if (moveHandler != null) moveHandler.onSetPlayerInput += SetInput;

			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onActivatePlayerMove += InitiateMoveFromOther;
					cube.onPlayerPosCheck += FetchPos;
				}
			}

			if (playerAnimator != null)
			{
				playerAnimator.onInputSet += SetInput;
				playerAnimator.onIntroSet += SetInIntro;
			} 

			if (expresHandler != null) expresHandler.onFetchStunned += FetchIsStunned;
		}

		private void Start()
		{
			cubePoser.RoundPosition();
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
			var cubeToShrink = cubePoser.FetchGridPos();
			moveHandler.ResetMovedMoveables();

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

			if (initiatedByPlayer)
			{
				playerFlipJuicer.PlayFlipJuice();
				yield return new WaitForSeconds(playerFlipJuicer.preFlipJuiceDuration);
			}

			for (int i = 0; i < (90 / turnStep); i++)
			{
				transform.RotateAround(side.position, turnAxis, turnStep);
				yield return new WaitForSeconds(timeStep);
			}

			cubePoser.RoundPosition();
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

		public void InitiateLowering(Vector2Int cubePos, bool fromBoost)
		{
			Vector3 targetPos = new Vector3(transform.position.x,
				transform.position.y - loweredYDelta, transform.position.z);
			float step = lowerStep * Time.deltaTime;

			StartCoroutine(LowerCube(targetPos, step, cubePos, fromBoost));
		}

		private IEnumerator LowerCube(Vector3 targetPos, float step, 
			Vector2Int cubePos, bool fromBoost)
		{
			isMoving = true;
			input = false;
			
			if (fromBoost)
			{
				var juiceDur = boostJuicer.FetchJuiceDur();
				boostJuicer.PlayPostBoostJuice();
				yield return new WaitForSeconds(juiceDur);
			}

			transform.localScale = new Vector3(.95f, .95f, .95f);

			boostJuicer.PlayLoweringSFX();

			while (transform.position.y > targetPos.y)
			{
				transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
				yield return timeStep;
			}

			isLowered = true;
			if (moveHandler.movingMoveables == 0) input = true;
			cubePoser.RoundPosition();
			onRewindPulse(InterfaceIDs.Rewind);
		}

		public bool CheckForWallAhead(Vector2Int posAhead)
		{
			var currentPos = cubePoser.FetchGridPos();

			if (moveHandler.CheckMoveableCubeDicKey(posAhead))
			{
				var posAheadOfAhead = posAhead + (posAhead - currentPos);
				return wallHandler.CheckForWallAheadOfAhead(posAhead, posAheadOfAhead);
			}
			else return wallHandler.wallCubeDic.ContainsKey(posAhead);
		}

		private void ActivateMoveableAhead(Vector2Int posAhead, Vector3 turnAxis)
		{
			if(moveHandler.CheckMoveableCubeDicKey(posAhead))
				moveHandler.StartMovingMoveable(posAhead, turnAxis, cubePoser.FetchGridPos());
		}

		public void CheckFloorInNewPos(Transform side, Vector3 turnAxis, Vector2Int posAhead)
		{
			onFloorCheck(cubePoser.FetchGridPos(), this.gameObject, side, turnAxis, posAhead);
		}

		public void UpdateCenterPosition()
		{
			center.position = transform.position;
		}

		private void SetInput(bool value)
		{
			input = value;
		}

		private void SetInIntro(bool value)
		{
			isInIntroSeq = value;
		}

		private bool FetchIsStunned()
		{
			return isStunned;
		}

		private Vector2Int FetchPos()
		{
			return cubePoser.FetchGridPos();
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

		private void OnDisable()
		{
			if (moveHandler != null) moveHandler.onSetPlayerInput -= SetInput;

			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onActivatePlayerMove -= InitiateMoveFromOther;
					cube.onPlayerPosCheck -= FetchPos;
				}
			}

			if (playerAnimator != null)
			{
				playerAnimator.onInputSet -= SetInput;
				playerAnimator.onIntroSet -= SetInIntro;
			}

			if (expresHandler != null) expresHandler.onFetchStunned += FetchIsStunned;
		}
	}
}