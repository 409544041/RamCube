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

			if (cubeFF != null) cubeFF.onKeyCheck += CheckFloorCubeDicKey;

			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onFloorKeyCheck += CheckFloorCubeDicKey;
					cube.onSetFindable += SetFindableStatus;
					cube.onDicRemove += RemoveFromDictionary;
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
				if (shrunkFloorCubeDic.ContainsKey(cube.FetchGridPos())) continue;

				Vector2Int pos = cube.FetchGridPos();
				if (floorCubeDic.ContainsKey(pos))
					Debug.Log("Overlapping cube " + cube + " & " + floorCubeDic[pos]);
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

		public void FromFloorToShrunkDic(Vector2Int cubePos, FloorCube cube)
		{
			if (floorCubeDic.ContainsKey(cubePos))
			{
				floorCubeDic.Remove(cubePos);
				shrunkFloorCubeDic.Add(cubePos, cube);
			}
		}

		public void FromShrunkToFloorDic(Vector2Int cubePos, FloorCube cube)
		{
			if (shrunkFloorCubeDic.ContainsKey(cubePos))
			{
				shrunkFloorCubeDic.Remove(cubePos);
				floorCubeDic.Add(cubePos, cube);
			}
		}

		private void InitialRecordCubes()
		{
			foreach (KeyValuePair<Vector2Int, FloorCube> pair in floorCubeDic)
			{
				TriggerRecord(pair);
			}

			foreach (KeyValuePair<Vector2Int, FloorCube> pair in shrunkFloorCubeDic)
			{
				TriggerRecord(pair);
			}
		}

		private void TriggerRecord(KeyValuePair<Vector2Int, FloorCube> pair)
		{
			var cube = pair.Value;

			onInitialCubeRecording(cube, cube.transform.position,
				cube.transform.rotation, cube.transform.localScale);
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
			floorCubeDic.Remove(cubePos); //TO DO: Remove bc new dic system?
		}

		public FloorCube FetchCube(Vector2Int cubePos)
		{
			if (floorCubeDic.ContainsKey(cubePos))
				return floorCubeDic[cubePos];
			else return shrunkFloorCubeDic[cubePos];
		}

		private void SetFindableStatus(Vector2Int cubePos, bool value)
		{
			FetchCube(cubePos).isFindable = value;
		}

		private void OnDisable()
		{
			if (mover != null)
			{
				mover.onCubeShrink -= ShrinkCube;
				mover.onInitialFloorCubeRecord -= InitialRecordCubes;
			}

			if (cubeFF != null) cubeFF.onKeyCheck -= CheckFloorCubeDicKey;

			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onFloorKeyCheck -= CheckFloorCubeDicKey;
					cube.onSetFindable -= SetFindableStatus;
					cube.onDicRemove -= RemoveFromDictionary;
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
