using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCube : MonoBehaviour
{
	//Config parameters
	[SerializeField] CubeTypes type;

	//States
	public bool hasFallen { get; set; } = false;

	public Vector2Int FetchTileGridPos()
	{
		Vector2Int roundedPos = new Vector2Int
			(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));

		return roundedPos;
	}

	public CubeTypes FetchType()
	{
		return type;
	}
}
