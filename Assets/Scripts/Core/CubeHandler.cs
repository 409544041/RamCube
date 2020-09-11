using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using UnityEngine;

namespace Qbism.Core
{
	public class CubeHandler : MonoBehaviour
	{
		public Dictionary<Vector2Int, FloorCube> floorCubeGrid = new Dictionary<Vector2Int, FloorCube>();

		private void Awake()
		{
			LoadDictionary();
		}

		private void LoadDictionary()
		{
			var tiles = FindObjectsOfType<FloorCube>();
			foreach (FloorCube tile in tiles)
			{
				if (floorCubeGrid.ContainsKey(tile.FetchGridPos()))
					print("Overlapping tile " + tile);
				else floorCubeGrid.Add(tile.FetchGridPos(), tile);
			}
		}

		public void DropTile(Vector2Int tileToDrop)
		{
			if (floorCubeGrid[tileToDrop].FetchType() == CubeTypes.Falling)
			{
				floorCubeGrid[tileToDrop].GetComponent<Rigidbody>().isKinematic = false;
				floorCubeGrid.Remove(tileToDrop);
			}
		}

		public FloorCube FetchCube(Vector2Int tile)
		{
			return floorCubeGrid[tile];
		}
	}
}
