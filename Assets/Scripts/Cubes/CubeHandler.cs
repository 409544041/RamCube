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
		public FloorCube currentCube { get; set; } = null;
		
		public Dictionary<Vector2Int, FloorCube> floorCubeGrid = new Dictionary<Vector2Int, FloorCube>();

		public event Action onLand;
		public event Action onRecordStop;
		public event Action onFloorRecord;

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
			if (mover != null) mover.onCubeShrink += ShrinkCube;
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
			var cubes = FindObjectsOfType<FloorCube>();
			foreach (FloorCube cube in cubes)
			{
				if (floorCubeGrid.ContainsKey(cube.FetchGridPos()))
					print("Overlapping tile " + cube);
				else floorCubeGrid.Add(cube.FetchGridPos(), cube);
			}
		}

		public void ShrinkCube(Vector2Int cubeToShrink)
		{
			if (floorCubeGrid[cubeToShrink].FetchType() == CubeTypes.Falling)
			{
				floorCubeGrid[cubeToShrink].StartShrinking();
				floorCubeGrid.Remove(cubeToShrink);
			}
		}

		private void CheckFloorType(Vector2Int cubePos, GameObject cube)
		{
			FloorCube previousCube;

			if (!floorCubeGrid.ContainsKey(cubePos)) return;

			previousCube = currentCube;
			currentCube = FetchCube(cubePos);

			bool differentCubes = currentCube != previousCube;

			if(previousCube.FetchType() == CubeTypes.Static)
			{
				previousCube.GetComponent<StaticCube>().BecomeFallingCube(cube);
				onRecordStop();
			}

			if(previousCube.FetchType() == CubeTypes.Flipping && differentCubes)
				onFloorRecord();
				
			if (currentCube.FetchType() == CubeTypes.Boosting)
				currentCube.GetComponent<ICubeInfluencer>().PrepareAction(cube);

			else if (currentCube.FetchType() == CubeTypes.Flipping && differentCubes)
			{
				if (onLand != null) onLand();
				currentCube.GetComponent<ICubeInfluencer>().PrepareAction(cube);
			}

			else
			{
				if (differentCubes && onLand != null)
				{
					cubeFF.ShowFeedForward();
					onLand();
					onRecordStop();
					mover.PlayLandClip();
				}
				else
				{
					onRecordStop();
					cubeFF.ShowFeedForward();
				} 

				mover.input = true;
			}
		}

		private void CheckFloorTypeForFF(Vector2Int cubePos, GameObject cube)
		{
			var currentCube = FetchCube(cubePos);

			if(currentCube.FetchType() == CubeTypes.Boosting)
				currentCube.GetComponent<ICubeInfluencer>().PrepareAction(cube);
			
			else if(currentCube.FetchType() == CubeTypes.Flipping)
				currentCube.GetComponent<ICubeInfluencer>().PrepareAction(cube);
		}

		public bool CheckIfContainsKey(Vector2Int cubePos)
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
