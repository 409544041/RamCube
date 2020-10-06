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
			if (Input.GetKeyDown(KeyCode.W) &&
				handler.floorCubeGrid.ContainsKey(mover.FetchGridPos() + Vector2Int.up))
				mover.HandleKeyInput(mover.up, Vector3.right);

			if (Input.GetKeyDown(KeyCode.S) &&
				handler.floorCubeGrid.ContainsKey(mover.FetchGridPos() + Vector2Int.down))
				mover.HandleKeyInput(mover.down, Vector3.left);

			if (Input.GetKeyDown(KeyCode.A) &&
				handler.floorCubeGrid.ContainsKey(mover.FetchGridPos() + Vector2Int.left))
				mover.HandleKeyInput(mover.left, Vector3.forward);

			if (Input.GetKeyDown(KeyCode.D) &&
				handler.floorCubeGrid.ContainsKey(mover.FetchGridPos() + Vector2Int.right))
				mover.HandleKeyInput(mover.right, Vector3.back);

			if (Input.GetKeyDown(KeyCode.R))
				loader.RestartLevel();

			if (Input.GetKeyDown(KeyCode.Return))
				rewinder.StartRewinding();

		}
	}
}
