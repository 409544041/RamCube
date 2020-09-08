using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCubeMover : MonoBehaviour
{
	//Config parameters
	[SerializeField] Transform center;
	public Transform up;
	public Transform down;
	public Transform left;
	public Transform right;
	public int turnStep = 9;

	//Cache
	Rigidbody rb;
	CubeHandler handler;

	//States
	public bool isInBoostPos { get; set; } = true;
	public bool input { get; set;} = true;
	public bool isBoosting { get; set; } = false;

	public Vector2Int tileAbovePos { get; private set; } = new Vector2Int(0, 1);
	public Vector2Int tileBelowPos { get; private set; } = new Vector2Int(0, -1);
	public Vector2Int tileLeftPos { get; private set; } = new Vector2Int(-1, 0);
	public Vector2Int tileRightPos { get; private set; } = new Vector2Int(1, 0);

	FloorCube currentCube = null;

	public event Action onLand;
	public event Action onLandShowFF;

	private void Awake() 
	{
		rb = GetComponent<Rigidbody>();	
		handler = FindObjectOfType<CubeHandler>();
	}

	private void Start() 
	{
		UpdatePositions();
	}

	public void HandleSwipeInput(Transform rotateAroundAxis, Vector3 direction)
	{
		if(!input) return;
		StartCoroutine(Move(rotateAroundAxis, direction));
	}

	public void HandleKeyInput(Transform side, Vector3 turnAxis)
	{
		if (!input) return;
		StartCoroutine(Move(side, turnAxis));
	}

	private IEnumerator Move(Transform side, Vector3 turnAxis)
	{
		input = false;
		rb.isKinematic = true;

		var tileToDrop = FetchCubeGridPos();

		for (int i = 0; i < (90 / turnStep); i++)
		{
			transform.RotateAround(side.position, turnAxis, turnStep);
			yield return null;
		}

		RoundPosition();
		UpdatePositions();

		handler.DropTile(tileToDrop);

		rb.isKinematic = false;

		CheckFloorInNewPos();
	}

	public void RoundPosition()
	{
		transform.position = new Vector3(Mathf.RoundToInt(transform.position.x),
			0.5f, Mathf.RoundToInt(transform.position.z));
		
		Quaternion rotation = Quaternion.Euler(Mathf.RoundToInt(transform.rotation.x), 
			Mathf.RoundToInt(transform.rotation.y), Mathf.RoundToInt(transform.rotation.z));
	}

	public void CheckFloorInNewPos()
	{
		FloorCube previousCube = null;

		if(!handler.tileGrid.ContainsKey(FetchCubeGridPos())) return;

		previousCube = currentCube;		
		currentCube = handler.FetchTile(FetchCubeGridPos());

		bool differentCubes = currentCube != previousCube;

		if(currentCube.FetchType() == CubeTypes.Boosting)
			currentCube.GetComponent<BoostCube>().PrepareBoost(this.gameObject);

		else if (currentCube.FetchType() == CubeTypes.Flipping && differentCubes)
		{
			if (onLand != null) onLand();
			currentCube.GetComponent<FlipCube>().StartFlip(this.gameObject);
		}
			
		else
		{
			if (differentCubes && onLand != null && onLandShowFF != null)
			{
				onLand();
				onLandShowFF();
			} 
			else if(onLandShowFF != null) onLandShowFF();

			input = true;
		} 
	}

	public void UpdatePositions()
	{
		center.position = transform.position;
	}

	public Vector2Int FetchCubeGridPos()
	{
		Vector2Int roundedPos = new Vector2Int
			(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));

		return roundedPos;
	}
}
