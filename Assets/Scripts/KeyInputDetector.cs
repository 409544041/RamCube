using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputDetector : MonoBehaviour
{
	//Cache
	CubeHandler handler;
	PlayerCubeMover mover;
	SceneHandler loader;

	private void Awake() 
	{
		handler = GetComponent<CubeHandler>();
		mover = FindObjectOfType<PlayerCubeMover>();
		loader = GetComponent<SceneHandler>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.W) &&
			handler.floorCubeGrid.ContainsKey(mover.FetchGridPos() + mover.tileAbovePos))
			mover.HandleKeyInput(mover.up, Vector3.right);

		if (Input.GetKeyDown(KeyCode.S) &&
			handler.floorCubeGrid.ContainsKey(mover.FetchGridPos() + mover.tileBelowPos))
			mover.HandleKeyInput(mover.down, Vector3.left);

		if (Input.GetKeyDown(KeyCode.A) &&
			handler.floorCubeGrid.ContainsKey(mover.FetchGridPos() + mover.tileLeftPos))
			mover.HandleKeyInput(mover.left, Vector3.forward);

		if (Input.GetKeyDown(KeyCode.D) &&
			handler.floorCubeGrid.ContainsKey(mover.FetchGridPos() + mover.tileRightPos))
			mover.HandleKeyInput(mover.right, Vector3.back);

		if (Input.GetKeyDown(KeyCode.R))
			loader.RestartLevel();
	}
}
