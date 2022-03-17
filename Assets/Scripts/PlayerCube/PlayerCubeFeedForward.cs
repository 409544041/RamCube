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
		[SerializeField] PlayerRefHolder refs;

		//Cache
		PlayerCubeMover mover;
		PlayerAnimator playerAnimator;

		//States
		Vector2Int[] neighbourDirs;
		Vector3[] turnAxis;

		//Actions, events, delegates etc
		public Func<Vector2Int, bool> onKeyCheck;
		public Func<bool> onFinishCheck;

		private void Awake()
		{
			mover = refs.playerMover;
			playerAnimator = refs.playerAnim;
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
			if (mover.isMoving || mover.isBoosting || mover.isTurning || 
				mover.isInIntroSeq || onFinishCheck())
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

				var onePosAhead = refs.cubePos.FetchGridPos() + neighbourDirs[ffIndex];

				if (onKeyCheck(onePosAhead))
				{
					ffCube.SwitchFF(true);
					ffCube.transform.position = new Vector3
						(onePosAhead.x, transform.position.y, onePosAhead.y);
					ffCube.transform.Rotate(turnAxis[ffIndex], 90, Space.World);

					ffCube.CheckFloorInNewPos();
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
