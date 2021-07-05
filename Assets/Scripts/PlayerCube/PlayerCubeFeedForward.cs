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
			mover = FindObjectOfType<PlayerCubeMover>();
		}

		private void Start()
		{
			neighbourDirs = new Vector2Int[]
				{ Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

			turnAxis = new Vector3[]
				{ Vector3.right, Vector3.left, Vector3.forward, Vector3.back };

			ShowFeedForward();
		}

		private void Update()
		{
			DisableFeedForwardOnMove();
		}

		private void DisableFeedForwardOnMove()
		{
			if (mover.isMoving || mover.isBoosting || mover.isTurning)
			{
				foreach (FeedForwardCube ffCube in feedForwardCubes)
				{
					ffCube.gameObject.SetActive(false);
				}
			}
		}

		public IEnumerator ShowFeedForward()
		{
			yield return null; //This is dirty fix so laser hits before FF is shown

			for (int ffIndex = 0; ffIndex < feedForwardCubes.Length; ffIndex++)
			{
				var ffCube = feedForwardCubes[ffIndex];
				ffCube.transform.rotation = transform.rotation;

				var onePosAhead = mover.FetchGridPos() + neighbourDirs[ffIndex];

				if (onKeyCheck(onePosAhead) && onShrunkCheck(onePosAhead) == false)
				{
					ffCube.gameObject.SetActive(true);
					ffCube.transform.position = new Vector3
						(onePosAhead.x, transform.position.y, onePosAhead.y);
					ffCube.transform.Rotate(turnAxis[ffIndex], 90, Space.World);

					ffCube.GetComponent<FeedForwardCube>().CheckFloorInNewPos();
				}
				else ffCube.gameObject.SetActive(false);
			}
		}
	}
}
