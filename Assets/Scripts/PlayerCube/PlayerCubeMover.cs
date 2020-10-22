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
		[SerializeField] Transform center = null;
		public Transform up = null;
		public Transform down = null;
		public Transform left = null;
		public Transform right = null;
		[SerializeField] int turnStep = 18;
		[SerializeField] float timeStep = 0.01f;
		[SerializeField] AudioClip landClip = null;


		//Cache
		AudioSource source;
		MoveableCubeHandler moveHandler;

		//States
		public bool isInBoostPos { get; set; } = true;
		public bool input { get; set; } = true;
		public bool isBoosting { get; set; } = false;

		public event Action<Vector2Int> onCubeShrink;
		public event Action<Vector2Int, GameObject> onFloorCheck;
		public event Action onRecordStart;
		public event Action<Vector3, Quaternion, Vector3> onInitialRecord;
		public event Action<Vector2Int> onActivateMoveableCube;

		public event Action onMoveableListShift;

		private void Awake()
		{
			source = GetComponentInChildren<AudioSource>();
			moveHandler = FindObjectOfType<MoveableCubeHandler>();
		}

		private void OnEnable() 
		{
			if(moveHandler != null) moveHandler.onSetPlayerInput += SetPlayerInput;
		}

		private void Start()
		{
			UpdateCenterPosition();
		}

		public void HandleSwipeInput(Transform side, Vector3 turnAxis, Vector2Int posAhead)
		{
			if (!input) return;
			StartCoroutine(Move(side, turnAxis, posAhead));
		}

		public void HandleKeyInput(Transform side, Vector3 turnAxis, Vector2Int posAhead)
		{
			if (!input) return;
			StartCoroutine(Move(side, turnAxis, posAhead));
		}

		public IEnumerator Move(Transform side, Vector3 turnAxis, Vector2Int posAhead)
		{
			var cubeToShrink = FetchGridPos();

			onInitialRecord(transform.position, transform.rotation, transform.localScale);
			onRecordStart();

			CheckPosAhead(posAhead, turnAxis);

			input = false;

			for (int i = 0; i < (90 / turnStep); i++)
			{
				transform.RotateAround(side.position, turnAxis, turnStep);
				yield return new WaitForSeconds(timeStep);
			}

			RoundPosition();
			UpdateCenterPosition();

			onCubeShrink(cubeToShrink);

			CheckFloorInNewPos();
		}

		public void RoundPosition()
		{
			transform.position = new Vector3(Mathf.RoundToInt(transform.position.x),
				Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));

			Quaternion rotation = Quaternion.Euler(Mathf.RoundToInt(transform.rotation.x),
				Mathf.RoundToInt(transform.rotation.y), Mathf.RoundToInt(transform.rotation.z));
		}

		private void CheckPosAhead(Vector2Int posAhead, Vector3 turnAxis)
		{
			if(moveHandler.CheckMoveableCubeDicKey(posAhead))
			{
				onMoveableListShift();
				moveHandler.ActivateMoveableCube(posAhead, turnAxis, FetchGridPos());
			}
		}

		public void CheckFloorInNewPos()
		{
			onFloorCheck(FetchGridPos(), this.gameObject);
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

		private void SetPlayerInput(bool value)
		{
			input = value;
		}

		public void PlayLandClip()
		{
			AudioSource.PlayClipAtPoint(landClip, Camera.main.transform.position, .05f);
		}

		private void OnDisable()
		{
			if (moveHandler != null) moveHandler.onSetPlayerInput -= SetPlayerInput;
		}
	}
}