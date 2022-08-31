using System;
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
		[SerializeField] PlayerRefHolder refs;

		//Cache
		MoveableCubeHandler moveHandler;
		WallHandler wallHandler;
		MoveableCube[] moveableCubes;
		PlayerCubeFlipJuicer playerFlipJuicer;
		PlayerCubeBoostJuicer boostJuicer;
		ExpressionHandler expresHandler;

		//States
		public bool allowMoveInput { get; set; } = true;
		public bool allowRewind { get; set; } = true;
		public bool isBoosting { get; set; } = false;
		public bool isTurning { get; set; } = false;
		public bool initiatedByPlayer { get; set; } = true;
		public bool isMoving { get; set; } = false;
		private Vector3 startScale = new Vector3(1, 1, 1);
		public bool isStunned { get; set; }	= false;
		public bool isOutOfBounds { get; set; } = false;
		public bool isInIntroSeq { get; set; } = false;
		public bool isLowered { get; set; } = false;
		public bool justBoosted { get; set; } = false;
		public bool isRewinding { get; set; } = false;
		public bool isResetting { get; set; } = false;
		public bool newInput { get; set; } = false;
		public bool prevMoveNewInput { get; set; } = false;

		//Actions, events, delegates etc
		public event Action<Vector2Int> onCubeShrink;
		public event Action<Vector2Int, GameObject, Transform, Vector3, Vector2Int> onFloorCheck;
		public event Action<Vector3, Quaternion, Vector3, Quaternion, Vector3> onInitialRecord;
		public event Action onInitialFloorCubeRecord;

		private void Awake()
		{
			moveHandler = refs.gcRef.glRef.movCubeHandler;
			wallHandler = refs.gcRef.glRef.wallHandler;
			moveableCubes = refs.gcRef.movCubes;
			playerFlipJuicer = refs.flipJuicer;
			boostJuicer = refs.boostJuicer;
			expresHandler = refs.exprHandler;
		}

		private void OnEnable() 
		{
			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onActivatePlayerMove += InitiateMoveFromOther;
					cube.onPlayerPosCheck += FetchPos;
				}
			}

			if (expresHandler != null) expresHandler.onFetchStunned += FetchIsStunned;
		}

		private void Start()
		{
			refs.cubePos.RoundPosition();
			UpdateCenterPosition();
			startScale = transform.localScale;
		}

		public void HandleSwipeInput(Transform side, Vector3 turnAxis, Vector2Int posAhead)
		{
			initiatedByPlayer = true;
			StartCoroutine(Move(side, turnAxis, posAhead));
		}

		public void HandleKeyInput(Transform side, Vector3 turnAxis, Vector2Int posAhead)
		{
			initiatedByPlayer = true;
			StartCoroutine(MoveableAndInitiationSourceCheck(side, turnAxis, posAhead));
		}

		public void InitiateMoveFromOther(MoveableCube cube, Transform side, Vector3 turnAxis, 
			Vector2Int posAhead)
		{
			SetSide(cube, ref side, ref posAhead);
			initiatedByPlayer = false;
			StartCoroutine(Move(side, turnAxis, posAhead));
		}

		private IEnumerator MoveableAndInitiationSourceCheck(Transform side, 
			Vector3 turnAxis, Vector2Int posAhead)
		{
			if (moveHandler.movingMoveables > 0 && initiatedByPlayer) moveHandler.InstantFinishMovingMoveables();
			while (moveHandler.movingMoveables > 0 && initiatedByPlayer)
			{
				yield return null;
			}
			moveHandler.ResetMovedMoveables();
			//Check if while waiting initiatedByPlayer hasn't been set to false
			if (initiatedByPlayer) StartCoroutine(Move(side, turnAxis, posAhead));
		}

		public IEnumerator Move(Transform side, Vector3 turnAxis, Vector2Int posAhead)
		{
			var cubeToShrink = refs.cubePos.FetchGridPos();

			if (CheckForWallAhead(posAhead)) //to avoid ffInputting into newly created mov wall
			{
				InitiateWiggle(side, turnAxis);
				yield break;
			}

			refs.stunJuicer.StopStunVFX();
			isMoving = true;
			allowRewind = false;

			if (initiatedByPlayer)
			{
				onInitialRecord(transform.position, transform.rotation, startScale, transform.rotation, 
					startScale);
				onInitialFloorCubeRecord();
				moveHandler.InitialRecordMoveables();
			} 

			ActivateMoveableAhead(posAhead, turnAxis);

			var startRot = transform.rotation;

			if (initiatedByPlayer && !prevMoveNewInput)
			{
				playerFlipJuicer.PlayFlipJuice();
				yield return new WaitForSeconds(playerFlipJuicer.preFlipJuiceDuration);
			}

			for (int i = 0; i < (90 / turnStep); i++)
			{
				if (!newInput)
				{
					transform.RotateAround(side.position, turnAxis, turnStep);
					yield return new WaitForSeconds(timeStep);
				}
				else
				{
					transform.rotation = startRot;
					transform.Rotate(turnAxis, 90, Space.World);
					transform.position = new Vector3(posAhead.x, transform.position.y, posAhead.y);
					break;
				}
			}

			if (!newInput) prevMoveNewInput = false;

			refs.cubePos.RoundPosition();
			UpdateCenterPosition();
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
			for (int i = 0; i < (wiggleRotation / wiggleTurnStep); i++)
			{
				if (isRewinding) yield break;

				transform.RotateAround(side.position, turnAxis, wiggleTurnStep);
				yield return null;
			}

			for (int i = 0; i < (wiggleRotation / wiggleTurnStep); i++)
			{
				if (isRewinding) yield break;

				transform.RotateAround(side.position, -turnAxis, wiggleTurnStep);
				yield return null;
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
			isLowered = true;

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

			if (moveHandler.movingMoveables == 0)
			{
				allowRewind = true;
				initiatedByPlayer = true;
			}

			isMoving = false;
			newInput = false;
			refs.cubePos.RoundPosition();
			refs.gcRef.rewindPulser.InitiatePulse();

			foreach (var lRef in refs.gcRef.laserRefs)
			{
				lRef.detector.GoIdle();
			}
		}

		public bool CheckForWallAhead(Vector2Int posAhead)
		{
			var currentPos = refs.cubePos.FetchGridPos();

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
				moveHandler.StartMovingMoveable(posAhead, turnAxis, refs.cubePos.FetchGridPos());
		}

		public void CheckFloorInNewPos(Transform side, Vector3 turnAxis, Vector2Int posAhead)
		{
			onFloorCheck(refs.cubePos.FetchGridPos(), this.gameObject, side, turnAxis, posAhead);
		}

		public void UpdateCenterPosition()
		{
			center.position = transform.position;
		}

		public void SetAllowInput(bool value)
		{
			if (refs.gcRef.pauseOverlayHandler.overlayActive) return;
			
			allowRewind = value;
			allowMoveInput = value;
		}

		private bool FetchIsStunned()
		{
			return isStunned;
		}

		private Vector2Int FetchPos()
		{
			return refs.cubePos.FetchGridPos();
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
			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onActivatePlayerMove -= InitiateMoveFromOther;
					cube.onPlayerPosCheck -= FetchPos;
				}
			}

			if (expresHandler != null) expresHandler.onFetchStunned += FetchIsStunned;
		}
	}
}