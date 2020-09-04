using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeHandler : MonoBehaviour
{
	public Dictionary<Vector2Int, FloorCube> tileGrid = new Dictionary<Vector2Int, FloorCube>();

	private void Start() 
	{
		LoadDictionary();
	}

	private void LoadDictionary()
	{
		var tiles = FindObjectsOfType<FloorCube>();
		foreach (FloorCube tile in tiles)
		{
			if(tileGrid.ContainsKey(tile.FetchTileGridPos()))
				print("Overlapping tile " + tile);
			else tileGrid.Add(tile.FetchTileGridPos(), tile);
		}
	}

	public void DropTile(Vector2Int tileToDrop)
	{
		if(tileGrid[tileToDrop].FetchType() == CubeTypes.Falling )
		{
			tileGrid[tileToDrop].GetComponent<Rigidbody>().isKinematic = false;
			tileGrid.Remove(tileToDrop);
		} 
	}

	public FloorCube FetchTile(Vector2Int tile)
	{
		return tileGrid[tile];
	}
}
