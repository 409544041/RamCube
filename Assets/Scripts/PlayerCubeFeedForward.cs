using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCubeFeedForward : MonoBehaviour
{
	//Config parameters
	[SerializeField] FeedForwardCube[] feedForwardCubes = null;

	//Cache
	PlayerCubeMover mover;
	CubeHandler handler;

	//States
	Vector2Int[] surroundingPos;
	Vector3[] turnAxis;

	private void Awake() 
	{
		mover = GetComponent<PlayerCubeMover>();	
		handler = FindObjectOfType<CubeHandler>();
	}

	private void OnEnable() 
	{
		if(mover != null) mover.onLandShowFF += ShowFeedForward;
	}

	private void Start() 
	{
		surroundingPos = new Vector2Int[]
			{ mover.tileAbovePos, mover.tileBelowPos, mover.tileLeftPos, mover.tileRightPos };

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

	private void ShowFeedForward()
	{
		for (int ffIndex = 0; ffIndex < feedForwardCubes.Length; ffIndex++)
		{
			var ffCube = feedForwardCubes[ffIndex];
			ffCube.transform.rotation = transform.rotation;

			var onePosAhead = mover.FetchGridPos() + surroundingPos[ffIndex];

			if (handler.floorCubeGrid.ContainsKey(onePosAhead))
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

	private void OnDisable()
	{
		if (mover != null) mover.onLandShowFF -= ShowFeedForward;
	}

}
