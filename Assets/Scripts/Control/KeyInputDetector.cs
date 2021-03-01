﻿using System.Collections;
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

				if(handler.floorCubeDic.ContainsKey(posAhead)
					&& handler.FetchShrunkStatus(posAhead) == false)
					mover.HandleKeyInput(mover.up, Vector3.right, posAhead);
			}

			if (Input.GetKeyDown(KeyCode.S))
			{
				var posAhead = mover.FetchGridPos() + Vector2Int.down;

				if (handler.floorCubeDic.ContainsKey(posAhead)
					&& handler.FetchShrunkStatus(posAhead) == false)
					mover.HandleKeyInput(mover.down, Vector3.left, posAhead);
			}

			if (Input.GetKeyDown(KeyCode.A))
			{
				var posAhead = mover.FetchGridPos() + Vector2Int.left;

				if (handler.floorCubeDic.ContainsKey(posAhead)
					&& handler.FetchShrunkStatus(posAhead) == false)
					mover.HandleKeyInput(mover.left, Vector3.forward, posAhead);
			}

			if (Input.GetKeyDown(KeyCode.D))
			{
				var posAhead = mover.FetchGridPos() + Vector2Int.right;

				if (handler.floorCubeDic.ContainsKey(posAhead)
					&& handler.FetchShrunkStatus(posAhead) == false)
					mover.HandleKeyInput(mover.right, Vector3.back, posAhead);
			}

			if (Input.GetKeyDown(KeyCode.Return))
				loader.RestartLevel();
			
			if (Input.GetKeyDown(KeyCode.R)) 
				rewinder.StartRewinding();

		}
	}
}
