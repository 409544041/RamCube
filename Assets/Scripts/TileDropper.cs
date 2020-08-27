using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDropper : MonoBehaviour
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
		if(tileGrid[tileToDrop].FetchIsStatic()) return;
		tileGrid[tileToDrop].GetComponent<Rigidbody>().isKinematic = false;
		tileGrid[tileToDrop].GetComponent<FloorTile>().hasFallen = true;
	}

	public bool FetchTileFallen(Vector2Int tile)
	{	
		if(!tileGrid[tile]) return true;
		return tileGrid[tile].hasFallen;
	}
}
