using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.Cubes
{
	public class CubeHandler : MonoBehaviour
	{
		//Cache
		FeedForwardCube[] ffCubes;
		PlayerCubeFeedForward cubeFF;
		PlayerCubeMover mover;

		//States
		FloorCube currentCube = null;
		
		public Dictionary<Vector2Int, FloorCube> floorCubeGrid = new Dictionary<Vector2Int, FloorCube>();

		public event Action onLand;

		private void Awake()
		{
			mover = FindObjectOfType<PlayerCubeMover>();
			ffCubes = FindObjectsOfType<FeedForwardCube>();
			cubeFF = FindObjectOfType<PlayerCubeFeedForward>();
			LoadDictionary();
		}

		private void OnEnable() 
		{
			if (mover != null) mover.onFloorCheck += CheckFloorType;
			if (mover != null) mover.onCubeDrop += DropCube;
			if (cubeFF != null) cubeFF.onKeyCheck += CheckIfContainsKey;
			if (ffCubes != null)
			{
				foreach (FeedForwardCube ffCube in ffCubes)
				{
					ffCube.onFeedForwardFloorCheck += CheckFloorTypeForFF;
				}
			}
		}

		private void Start() 
		{

			currentCube = FetchCube(mover.FetchGridPos());
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

		public void DropCube(Vector2Int tileToDrop)
		{
			if (floorCubeGrid[tileToDrop].FetchType() == CubeTypes.Falling)
			{
				floorCubeGrid[tileToDrop].GetComponent<Rigidbody>().isKinematic = false;
				floorCubeGrid.Remove(tileToDrop);
			}
		}

		private void CheckFloorType(Vector2Int cubePos, GameObject cube)
		{
			FloorCube previousCube = null;

			if (!floorCubeGrid.ContainsKey(cubePos)) return;

			previousCube = currentCube;
			currentCube = FetchCube(cubePos);

			bool differentCubes = currentCube != previousCube;

			if (currentCube.FetchType() == CubeTypes.Boosting)
				currentCube.GetComponent<BoostCube>().PrepareBoost(cube);

			else if (currentCube.FetchType() == CubeTypes.Flipping && differentCubes)
			{
				if (onLand != null) onLand();
				currentCube.GetComponent<FlipCube>().StartFlip(cube);
			}

			else
			{
				if (differentCubes && onLand != null)
				{
					cubeFF.ShowFeedForward();
					onLand();
				}
				else cubeFF.ShowFeedForward();

				mover.input = true;
			}
		}

		private void CheckFloorTypeForFF(Vector2Int cubePos, GameObject cube)
		{
			var currentCube = FetchCube(cubePos);

			if(currentCube.FetchType() == CubeTypes.Boosting)
				currentCube.GetComponent<BoostCube>().PrepareBoost(cube);
			
			else if(currentCube.FetchType() == CubeTypes.Flipping)
				currentCube.GetComponent<FlipCube>().StartFlip(cube);
		}

		private bool CheckIfContainsKey(Vector2Int cubePos)
		{
			if(floorCubeGrid.ContainsKey(cubePos)) return true;
			else return false;
		}

		public FloorCube FetchCube(Vector2Int cubePos)
		{
			return floorCubeGrid[cubePos];
		}
	}
}
