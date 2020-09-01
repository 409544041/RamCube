using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour
{
	//Config parameters
	[SerializeField] TileTypes type;

	//States
	public bool hasFallen { get; set; } = false;

	public Vector2Int FetchTileGridPos()
	{
		return new Vector2Int
			(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
	}

	public TileTypes FetchType()
	{
		return type;
	}
}
