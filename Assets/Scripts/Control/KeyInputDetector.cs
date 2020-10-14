using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using Qbism.PlayerCube;
using Qbism.Rewind;
using Qbism.SceneTransition;
using UnityEngine;

namespace Qbism.Control
{
	public class KeyInputDetector : MonoBehaviour
	{
		//Cache
		CubeHandler handler;
		PlayerCubeMover mover;
		SceneHandler loader;
		RewindHandler rewinder;

		private void Awake()
		{
			handler = GetComponent<CubeHandler>();
			mover = FindObjectOfType<PlayerCubeMover>();
			loader = GetComponent<SceneHandler>();
			rewinder = GetComponent<RewindHandler>();
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.W))
			{
				var posAhead = mover.FetchGridPos() + Vector2Int.up;

				if(handler.floorCubeGrid.ContainsKey(posAhead)
					&& handler.FetchShrunkStatus(posAhead) == false)
					mover.HandleKeyInput(mover.up, Vector3.right);
			}

			if (Input.GetKeyDown(KeyCode.S))
			{
				var posAhead = mover.FetchGridPos() + Vector2Int.down;

				if (handler.floorCubeGrid.ContainsKey(posAhead)
					&& handler.FetchShrunkStatus(posAhead) == false)
					mover.HandleKeyInput(mover.down, Vector3.left);
			}

			if (Input.GetKeyDown(KeyCode.A))
			{
				var posAhead = mover.FetchGridPos() + Vector2Int.left;

				if (handler.floorCubeGrid.ContainsKey(posAhead)
					&& handler.FetchShrunkStatus(posAhead) == false)
					mover.HandleKeyInput(mover.left, Vector3.forward);
			}

			if (Input.GetKeyDown(KeyCode.D))
			{
				var posAhead = mover.FetchGridPos() + Vector2Int.right;

				if (handler.floorCubeGrid.ContainsKey(posAhead)
					&& handler.FetchShrunkStatus(posAhead) == false)
					mover.HandleKeyInput(mover.right, Vector3.back);
			}

			if (Input.GetKeyDown(KeyCode.R))
				loader.RestartLevel();

			if (Input.GetKeyDown(KeyCode.Return) && rewinder.rewindsAmount > 0)
				rewinder.StartRewinding();

		}
	}
}
