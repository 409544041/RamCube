using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishCube : MonoBehaviour
{
	//Cache
	PlayerCubeMover mover;
	CubeHandler handler;

	//States
	Vector2Int myPosition;

	private void Awake() 
	{
		mover = FindObjectOfType<PlayerCubeMover>();
		handler = FindObjectOfType<CubeHandler>();
	}

	private void OnEnable() 
	{
		if (mover != null) mover.onLand += CheckForFinish;
	}

	private void Start() 
	{
		myPosition = new Vector2Int
			(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
	}

	private void CheckForFinish()
	{
		if (handler.FetchTile(myPosition) == handler.FetchTile(mover.FetchCubeGridPos()))
		{
			if (Mathf.Approximately(Vector3.Dot(mover.transform.forward,
				transform.forward), -1)) print("SUCCESS");
			else print("LOSE");
		}
	}

	private void OnDisable()
	{
		if (mover != null) mover.onLand -= CheckForFinish;
	}
}
