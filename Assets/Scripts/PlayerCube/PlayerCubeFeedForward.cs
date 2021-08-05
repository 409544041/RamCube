using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerCubeFeedForward : MonoBehaviour
	{
		//Config parameters
		[SerializeField] FeedForwardCube[] feedForwardCubes = null;

		//Cache
		PlayerCubeMover mover;
		PlayerAnimator playerAnimator;

		//States
		Vector2Int[] neighbourDirs;
		Vector3[] turnAxis;

		//Actions, events, delegates etc
		public delegate bool KeyCheckDel(Vector2Int pos);
		public KeyCheckDel onKeyCheck;

		public delegate bool ShrunkCheckDelegate(Vector2Int pos);
		public ShrunkCheckDelegate onShrunkCheck;

		private void Awake()
		{
			mover = GetComponent<PlayerCubeMover>();
			playerAnimator = GetComponentInChildren<PlayerAnimator>();
		}

		private void OnEnable() 
		{
			if (playerAnimator != null) playerAnimator.onShowFF += ShowFeedForward;
		}

		private void Start()
		{
			neighbourDirs = new Vector2Int[]
				{ Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

			turnAxis = new Vector3[]
				{ Vector3.right, Vector3.left, Vector3.forward, Vector3.back };
		}

		private void Update()
		{
			DisableFeedForwardOnMove();
		}

		private void DisableFeedForwardOnMove()
		{
			if (mover.isMoving || mover.isBoosting || mover.isTurning || mover.isInIntroSeq)
			{
				foreach (FeedForwardCube ffCube in feedForwardCubes)
				{
					ffCube.SwitchFF(false);
				}
			}
		}

		public void ShowFeedForward()
		{
			StartCoroutine(FeedForward());
		}

		private IEnumerator FeedForward()
		{
			yield return null; //This is dirty fix so laser hits before FF is shown

			for (int ffIndex = 0; ffIndex < feedForwardCubes.Length; ffIndex++)
			{
				var ffCube = feedForwardCubes[ffIndex];
				ffCube.transform.rotation = transform.rotation;

				var onePosAhead = mover.FetchGridPos() + neighbourDirs[ffIndex];

				if (onKeyCheck(onePosAhead) && onShrunkCheck(onePosAhead) == false)
				{
					ffCube.SwitchFF(true);
					ffCube.transform.position = new Vector3
						(onePosAhead.x, transform.position.y, onePosAhead.y);
					ffCube.transform.Rotate(turnAxis[ffIndex], 90, Space.World);

					ffCube.GetComponent<FeedForwardCube>().CheckFloorInNewPos();
				}
				else ffCube.SwitchFF(false);
			}
		}

		private void OnDisable()
		{
			if (playerAnimator != null) playerAnimator.onShowFF -= ShowFeedForward;
		}
	}
}
