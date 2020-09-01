using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHandler : MonoBehaviour
{
	Dictionary<Vector2Int, FloorTile> tileGrid = new Dictionary<Vector2Int, FloorTile>();

	private void Start() 
	{
		LoadDictionary();
	}

	private void LoadDictionary()
	{
		var tiles = FindObjectsOfType<FloorTile>();
		foreach (FloorTile tile in tiles)
		{
			if(tileGrid.ContainsKey(tile.FetchTileGridPos()))
				print("Overlapping tile " + tile);
			else tileGrid.Add(tile.FetchTileGridPos(), tile);
		}
	}

	public void DropTile(Vector2Int tileToDrop)
	{
		if(tileGrid[tileToDrop].FetchType() == TileTypes.Static) return;
		tileGrid[tileToDrop].GetComponent<Rigidbody>().isKinematic = false;
		tileGrid[tileToDrop].GetComponent<FloorTile>().hasFallen = true;
	}

	public FloorTile FetchTile(Vector2Int tile)
	{
		return tileGrid[tile];
	}
}
