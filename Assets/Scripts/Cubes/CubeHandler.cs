using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.MoveableCubes;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.Cubes
{
	public class CubeHandler : MonoBehaviour
	{
		//Cache
		FeedForwardCube[] ffCubes = null;
		PlayerCubeFeedForward cubeFF = null;
		PlayerCubeMover mover = null;
		MoveableCube[] moveableCubes = null;
		MoveableCubeHandler moveHandler;

		//States
		public FloorCube currentCube { get; set; } = null;
		
		public Dictionary<Vector2Int, FloorCube> floorCubeDic = 
			new Dictionary<Vector2Int, FloorCube>();
		
		public event Action onLand;
		public event Action onRecordStop;
		public event Action<FloorCube, Vector3, Quaternion, Vector3> onInitialCubeRecording;

		private void Awake()
		{
			mover = FindObjectOfType<PlayerCubeMover>();
			ffCubes = FindObjectsOfType<FeedForwardCube>();
			cubeFF = FindObjectOfType<PlayerCubeFeedForward>();
			moveableCubes = FindObjectsOfType<MoveableCube>();
			moveHandler = GetComponent<MoveableCubeHandler>();

			LoadFloorCubeDictionary();
		}

		private void OnEnable() 
		{
			if (mover != null)
			{
				mover.onFloorCheck += CheckFloorType;
				mover.onCubeShrink += ShrinkCube;
				mover.onInitialFloorCubeRecord += InitialRecordCubes;
			} 

			if (cubeFF != null)
			{
				cubeFF.onKeyCheck += CheckFloorCubeDicKey;
				cubeFF.onShrunkCheck += FetchShrunkStatus;
			} 

			if (ffCubes != null)
			{
				foreach (FeedForwardCube ffCube in ffCubes)
				{
					ffCube.onFeedForwardFloorCheck += CheckFloorTypeForFF;
				}
			}

			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onFloorKeyCheck += CheckFloorCubeDicKey;
					cube.onComponentAdd += AddComponent;
					cube.onFloorCheck += CheckFloorTypeForMoveable;
				}
			}
		}

		private void Start() 
		{
			currentCube = FetchCube(mover.FetchGridPos());
		}

		private void LoadFloorCubeDictionary()
		{
			FloorCube[] cubes = FindObjectsOfType<FloorCube>();
			foreach (FloorCube cube in cubes)
			{
				if (floorCubeDic.ContainsKey(cube.FetchGridPos()))
					print("Overlapping cube " + cube);
				else floorCubeDic.Add(cube.FetchGridPos(), cube);
			}
		}

		public void ShrinkCube(Vector2Int cubeToShrink)
		{
			if (floorCubeDic[cubeToShrink].FetchType() == CubeTypes.Shrinking)
			{
				floorCubeDic[cubeToShrink].StartShrinking();
			}
		}

		private void CheckFloorType(Vector2Int cubePos, GameObject cube)
		{
			FloorCube previousCube;

			previousCube = currentCube;
			currentCube = FetchCube(cubePos);

			bool differentCubes = currentCube != previousCube;

			if(previousCube.FetchType() == CubeTypes.Static)
				previousCube.GetComponent<StaticCube>().BecomeFallingCube(cube);

				
			if (currentCube.FetchType() == CubeTypes.Boosting)
				currentCube.GetComponent<ICubeInfluencer>().PrepareAction(cube);

			else if ((currentCube.FetchType() == CubeTypes.Flipping ||
				currentCube.FetchType() == CubeTypes.Turning) && differentCubes)
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

				if(moveHandler.isRecording == false) mover.input = true;
			}
		}

		private void CheckFloorTypeForFF(Vector2Int cubePos, GameObject cube)
		{
			FloorCube currentCube = FetchCube(cubePos);

			if(currentCube.FetchType() == CubeTypes.Boosting ||
				currentCube.FetchType() == CubeTypes.Turning)
				currentCube.GetComponent<ICubeInfluencer>().PrepareAction(cube);
		}

		private void CheckFloorTypeForMoveable(Transform side, Vector3 turnAxis, Vector2Int posAhead,
			MoveableCube cube, Vector2Int cubePos, Vector2Int originPos, Vector2Int prevPos)
		{
			if(floorCubeDic.ContainsKey(cubePos))
			{
				FloorCube currentCube = FetchCube(cubePos);
				FloorCube prevCube = FetchCube(prevPos);

				if (currentCube.FetchType() == CubeTypes.Boosting ||
					(currentCube.FetchType() == CubeTypes.Turning))
					currentCube.GetComponent<ICubeInfluencer>().
					PrepareActionForMoveable(side, turnAxis, posAhead, cube.gameObject, originPos, prevCube);

				else if(currentCube.FetchType() == CubeTypes.Shrinking ||
					currentCube.FetchType() == CubeTypes.Static)
				{
					if(prevCube.type == CubeTypes.Boosting && 
						moveHandler.CheckMoveableCubeDicKey(posAhead))
					{
						moveHandler.ActivateMoveableCube(posAhead, turnAxis, cubePos);
						cube.hasBumpedMoveable = true;
						cube.InitiateMove(side, turnAxis, posAhead, originPos);
					}
						
					else cube.InitiateMove(side, turnAxis, posAhead, originPos);
				}
			}
			else
			{
				cube.InitiateLowering(cubePos, originPos);
			}
		}

		private void InitialRecordCubes()
		{
			foreach (KeyValuePair<Vector2Int, FloorCube> pair in floorCubeDic)
			{
				var cube = pair.Value;
				onInitialCubeRecording(cube, cube.transform.position, 
					cube.transform.rotation, cube.transform.localScale);
			}
		}

		private bool CheckFloorCubeDicKey(Vector2Int cubePos)
		{
			if(floorCubeDic.ContainsKey(cubePos)) return true;
			else return false;
		}

		private void AddComponent(Vector2Int cubePos, GameObject cube, float shrinkStep, float shrinkTimeStep)
		{
			FloorCube newFloor = cube.AddComponent<FloorCube>();
			newFloor.shrinkStep = shrinkStep;
			newFloor.timeStep = shrinkTimeStep;
			newFloor.tag = "Environment";

			AddToDictionary(cubePos, newFloor);
		}

		private void AddToDictionary(Vector2Int cubePos, FloorCube cube)
		{
			floorCubeDic.Add(cubePos, cube);
		}

		public FloorCube FetchCube(Vector2Int cubePos)
		{
			return floorCubeDic[cubePos];
		}

		public bool FetchShrunkStatus(Vector2Int cubePos)
		{
			FloorCube cube = FetchCube(cubePos);
			if (cube.hasShrunk) return true;
			else return false;
		}

		private void OnDisable()
		{
			if (mover != null)
			{
				mover.onFloorCheck -= CheckFloorType;
				mover.onCubeShrink -= ShrinkCube;
				mover.onInitialFloorCubeRecord -= InitialRecordCubes;
			}

			if (cubeFF != null)
			{
				cubeFF.onKeyCheck -= CheckFloorCubeDicKey;
				cubeFF.onShrunkCheck -= FetchShrunkStatus;
			}

			if (ffCubes != null)
			{
				foreach (FeedForwardCube ffCube in ffCubes)
				{
					ffCube.onFeedForwardFloorCheck -= CheckFloorTypeForFF;
				}
			}

			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onFloorKeyCheck -= CheckFloorCubeDicKey;
					cube.onComponentAdd -= AddComponent;
					cube.onFloorCheck -= CheckFloorTypeForMoveable;
				}
			}
		}
	}
}
