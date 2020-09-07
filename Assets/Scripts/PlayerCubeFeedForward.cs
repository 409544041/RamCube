using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCubeFeedForward : MonoBehaviour
{
	//Config parameters
	[SerializeField] GameObject[] feedForwardCubes;

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
		if(mover != null) mover.onLand += ShowFeedForward;
	}

	private void Start() 
	{
		surroundingPos = new Vector2Int[]
			{ mover.tileAbovePos, mover.tileBelowPos, mover.tileLeftPos, mover.tileRightPos };

		turnAxis = new Vector3[]
			{ Vector3.right, Vector3.left, Vector3.forward, Vector3.back };
	}

	private void Update() 
	{
		DisableFeedForwardOnMove();
	}

	private void DisableFeedForwardOnMove()
	{
		if (mover.input == false)
		{
			foreach (GameObject ffCube in feedForwardCubes)
			{
				ffCube.SetActive(false);
			}
		}
	}

	private void ShowFeedForward()
	{
		for (int ffIndex = 0; ffIndex < feedForwardCubes.Length; ffIndex++)
		{
			var ffCube = feedForwardCubes[ffIndex];
			ffCube.transform.rotation = transform.rotation;

			var onePosAhead = mover.FetchCubeGridPos() + surroundingPos[ffIndex];

			if (handler.tileGrid.ContainsKey(onePosAhead))
			{
				ffCube.SetActive(true);
				ffCube.transform.position = new Vector3 
					(onePosAhead.x, transform.position.y, onePosAhead.y);
				ffCube.transform.Rotate(turnAxis[ffIndex], 90, Space.World); 

				ffCube.GetComponent<FeedForwardCube>().CheckFloorInNewPos();
			}	
			else ffCube.SetActive(false);
		}
	}

	private void OnDisable()
	{
		if (mover != null) mover.onLand -= ShowFeedForward;
	}

}
