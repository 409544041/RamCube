using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Qbism.MoveableCubes;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.Cubes
{
	public class CubeHandler : MonoBehaviour
	{
		//Cache
		PlayerCubeFeedForward cubeFF = null;
		PlayerCubeMover mover = null;
		MoveableCube[] moveableCubes = null;
		FloorComponentAdder[] compAdders = null;

		//States		
		public Dictionary<Vector2Int, FloorCube> floorCubeDic = 
			new Dictionary<Vector2Int, FloorCube>();

		public Dictionary<Vector2Int, FloorCube> shrunkFloorCubeDic =
			new Dictionary<Vector2Int, FloorCube>();

		//Actions, events, delegates etc
		public event Action<FloorCube, Vector3, Quaternion, Vector3> onInitialCubeRecording;

		private void Awake()
		{
			mover = FindObjectOfType<PlayerCubeMover>();
			cubeFF = FindObjectOfType<PlayerCubeFeedForward>();
			moveableCubes = FindObjectsOfType<MoveableCube>();
			compAdders = FindObjectsOfType<FloorComponentAdder>();
			LoadFloorCubeDictionary();
		}

		private void OnEnable() 
		{
			if (mover != null)
			{
				mover.onCubeShrink += ShrinkCube;
				mover.onInitialFloorCubeRecord += InitialRecordCubes;
			} 

			if (cubeFF != null)
			{
				cubeFF.onKeyCheck += CheckFloorCubeDicKey;
				cubeFF.onShrunkCheck += FetchShrunkStatus;
			} 

			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onFloorKeyCheck += CheckFloorCubeDicKey;
					cube.onShrunkCheck += FetchShrunkStatus;
					cube.onSetFindable += SetFindableStatus;
					cube.onDicRemove += RemoveFromDictionary;
					cube.onSetShrunk += SetShrunkStatus;
				}
			}

			if (compAdders != null)
			{
				foreach (var adder in compAdders)
				{
					adder.onAddToDic += AddToDictionary;
				}
			}
		}

		public void LoadFloorCubeDictionary()
		{
			FloorCube[] cubes = FindObjectsOfType<FloorCube>();
			foreach (FloorCube cube in cubes)
			{
				Vector2Int pos = cube.FetchGridPos();
				if (floorCubeDic.ContainsKey(pos))
					print("Overlapping cube " + cube + " & " + floorCubeDic[pos]);
				else floorCubeDic.Add(cube.FetchGridPos(), cube);
			}
		}

		public void ShrinkCube(Vector2Int cubeToShrink)
		{
			if (floorCubeDic[cubeToShrink].FetchType() == CubeTypes.Shrinking)
			{
				floorCubeDic[cubeToShrink].GetComponent<CubeShrinker>().StartShrinking();
			}
		}

		public void FloorCubeDicSwap(Vector2Int cubePos, FloorCube cube)
		{
			if (floorCubeDic.ContainsKey(cubePos))
			{
				floorCubeDic.Remove(cubePos);
				shrunkFloorCubeDic.Add(cubePos, cube);
			}
			else if (shrunkFloorCubeDic.ContainsKey(cubePos))
			{
				shrunkFloorCubeDic.Remove(cubePos);
				floorCubeDic.Add(cubePos, cube);
			}
		}

		private void InitialRecordCubes()
		{
			foreach (KeyValuePair<Vector2Int, FloorCube> pair in floorCubeDic)
			{
				var cube = pair.Value;
				CubeShrinker shrinker = cube.GetComponent<CubeShrinker>();
				
				onInitialCubeRecording(cube, cube.transform.position, 
					cube.transform.rotation, cube.transform.localScale);
			}
		}

		public bool CheckFloorCubeDicKey(Vector2Int cubePos)
		{
			if(floorCubeDic.ContainsKey(cubePos)) return true;
			else return false;
		}

		public void AddToDictionary(Vector2Int cubePos, FloorCube cube)
		{
			floorCubeDic.Add(cubePos, cube);
		}

		private void RemoveFromDictionary(Vector2Int cubePos)
		{
			floorCubeDic.Remove(cubePos);
		}

		public FloorCube FetchCube(Vector2Int cubePos)
		{
			return floorCubeDic[cubePos];
		}

		public bool FetchShrunkStatus(Vector2Int cubePos)
		{
			FloorCube cube = FetchCube(cubePos);
			CubeShrinker shrinker = cube.GetComponent<CubeShrinker>();

			if (shrinker && shrinker.hasShrunk) return true;
			else return false;
		}

		private void SetFindableStatus(Vector2Int cubePos, bool value)
		{
			FetchCube(cubePos).isFindable = value;
		}

		private void SetShrunkStatus(Vector2Int cubePos, bool value)
		{
			if(FetchCube(cubePos).type == CubeTypes.Shrinking)
				FetchCube(cubePos).GetComponent<CubeShrinker>().hasShrunk = value;
		}

		private void OnDisable()
		{
			if (mover != null)
			{
				mover.onCubeShrink -= ShrinkCube;
				mover.onInitialFloorCubeRecord -= InitialRecordCubes;
			}

			if (cubeFF != null)
			{
				cubeFF.onKeyCheck -= CheckFloorCubeDicKey;
				cubeFF.onShrunkCheck -= FetchShrunkStatus;
			}

			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onFloorKeyCheck -= CheckFloorCubeDicKey;
					cube.onShrunkCheck -= FetchShrunkStatus;
					cube.onSetFindable -= SetFindableStatus;
					cube.onDicRemove -= RemoveFromDictionary;
					cube.onSetShrunk -= SetShrunkStatus;
				}
			}

			if (compAdders != null)
			{
				foreach (var adder in compAdders)
				{
					adder.onAddToDic -= AddToDictionary;
				}
			}
		}
	}
}
