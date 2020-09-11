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

		public delegate bool KeyCheckDelegate(Vector2Int pos);
		public KeyCheckDelegate onKeyCheck;

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
			if (mover.input == false)
			{
				foreach (FeedForwardCube ffCube in feedForwardCubes)
				{
					ffCube.gameObject.SetActive(false);
				}
			}
		}

		public void ShowFeedForward()
		{
			for (int ffIndex = 0; ffIndex < feedForwardCubes.Length; ffIndex++)
			{
				var ffCube = feedForwardCubes[ffIndex];
				ffCube.transform.rotation = transform.rotation;

				var onePosAhead = mover.FetchGridPos() + neighbourDirs[ffIndex];

				if (onKeyCheck(onePosAhead))
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

		public FeedForwardCube[] FetchFFCubes()
		{
			return feedForwardCubes;
		}
	}
}
