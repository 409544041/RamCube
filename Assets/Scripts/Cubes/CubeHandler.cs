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

		public Dictionary<Vector2Int, FloorCube> movFloorCubeDic =
			new Dictionary<Vector2Int, FloorCube>();

		public Dictionary<Vector2Int, FloorCube> shrunkMovFloorCubeDic =
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
				mover.onCubeShrink += DicCheckForShrinking;
				mover.onInitialFloorCubeRecord += InitialRecordCubes;
			} 

			if (cubeFF != null) cubeFF.onKeyCheck += CheckFloorCubeDicKey;

			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onFloorKeyCheck += CheckFloorCubeDicKey;
					cube.onSetFindable += SetFindableStatus;
				}
			}

			if (compAdders != null)
			{
				foreach (var adder in compAdders)
				{
					adder.onAddToMovFloorDic += AddToMovFloorCubeDic;
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

		private void DicCheckForShrinking(Vector2Int cubeToShrink)
		{
			if (floorCubeDic.ContainsKey(cubeToShrink))
				StartShrinking(cubeToShrink, floorCubeDic);
			else if (movFloorCubeDic.ContainsKey(cubeToShrink))
				StartShrinking(cubeToShrink, movFloorCubeDic);
		}

		private void StartShrinking(Vector2Int cubeToShrink, Dictionary<Vector2Int, FloorCube> dic)
		{
			if (dic[cubeToShrink].FetchType() == CubeTypes.Shrinking)
			{
				dic[cubeToShrink].GetComponent<CubeShrinker>().StartShrinking();
			}
		}

		public void FromFloorToShrunkDic(Vector2Int cubePos, FloorCube cube)
		{
			//Check movDics first
			if (movFloorCubeDic.ContainsKey(cubePos))
			{
				movFloorCubeDic.Remove(cubePos);
				shrunkMovFloorCubeDic.Add(cubePos, cube);
			}

			else if (floorCubeDic.ContainsKey(cubePos))
			{
				floorCubeDic.Remove(cubePos);
				shrunkFloorCubeDic.Add(cubePos, cube);
			}
		}

		public void FromShrunkToFloorDic(Vector2Int cubePos, FloorCube cube)
		{
			//Check movDics first
			if (shrunkMovFloorCubeDic.ContainsKey(cubePos))
			{
				shrunkMovFloorCubeDic.Remove(cubePos);
				movFloorCubeDic.Add(cubePos, cube);
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
				TriggerRecord(pair);
			}

			foreach (KeyValuePair<Vector2Int, FloorCube> pair in shrunkFloorCubeDic)
			{
				TriggerRecord(pair);
			}

			foreach (KeyValuePair<Vector2Int, FloorCube> pair in movFloorCubeDic)
			{
				TriggerRecord(pair);
			}

			foreach (KeyValuePair<Vector2Int, FloorCube> pair in shrunkMovFloorCubeDic)
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
			if(floorCubeDic.ContainsKey(cubePos) || movFloorCubeDic.ContainsKey(cubePos)) return true;
			else return false;
		}

		private void AddToMovFloorCubeDic(Vector2Int cubePos, FloorCube cube)
		{
			movFloorCubeDic.Add(cubePos, cube);
		}

		public FloorCube FetchCube(Vector2Int cubePos)
		{
			if (floorCubeDic.ContainsKey(cubePos))
				return floorCubeDic[cubePos];
			else if (movFloorCubeDic.ContainsKey(cubePos))
				return movFloorCubeDic[cubePos];
			else if (shrunkFloorCubeDic.ContainsKey(cubePos))
				return shrunkFloorCubeDic[cubePos];
			else return shrunkMovFloorCubeDic[cubePos];
		}

		private void SetFindableStatus(Vector2Int cubePos, bool value)
		{
			FetchCube(cubePos).isFindable = value;
		}

		private void OnDisable()
		{
			if (mover != null)
			{
				mover.onCubeShrink -= DicCheckForShrinking;
				mover.onInitialFloorCubeRecord -= InitialRecordCubes;
			}

			if (cubeFF != null) cubeFF.onKeyCheck -= CheckFloorCubeDicKey;

			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onFloorKeyCheck -= CheckFloorCubeDicKey;
					cube.onSetFindable -= SetFindableStatus;
				}
			}

			if (compAdders != null)
			{
				foreach (var adder in compAdders)
				{
					adder.onAddToMovFloorDic -= AddToMovFloorCubeDic;
				}
			}
		}
	}
}
