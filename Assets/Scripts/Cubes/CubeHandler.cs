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
		//Config parameters
		[SerializeField] GameplayCoreRefHolder gcRef;

		//Cache
		PlayerCubeFeedForward cubeFF;
		PlayerCubeMover mover;
		MoveableCube[] moveableCubes;
		FloorComponentAdder[] compAdders;

		//States	
		public Vector2Int shrunkToFloorThisRewind { get; set; } = new Vector2Int(99, 99);

		public Dictionary<Vector2Int, FloorCube> floorCubeDic = 
			new Dictionary<Vector2Int, FloorCube>();

		public Dictionary<Vector2Int, List<FloorCube>> shrunkFloorCubeDic =
			new Dictionary<Vector2Int, List<FloorCube>>();

		public Dictionary<Vector2Int, FloorCube> movFloorCubeDic =
			new Dictionary<Vector2Int, FloorCube>();

		public Dictionary<Vector2Int, List<FloorCube>> shrunkMovFloorCubeDic =
			new Dictionary<Vector2Int, List<FloorCube>>();

		//Actions, events, delegates etc
		public event Action<CubeRefHolder, Vector3, Quaternion, Vector3, Quaternion, Vector3> onInitialCubeRecording;

		private void Awake()
		{
			mover = gcRef.pRef.playerMover;
			cubeFF = gcRef.pRef.playerFF;
			moveableCubes = gcRef.movCubes;
			compAdders = gcRef.floorCompAdders;
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
					cube.onFloorKeyCheck += CheckFloorCubeDicKey;
			}

			if (compAdders != null)
			{
				foreach (var adder in compAdders)
				{
					adder.onAddToMovFloorDic += AddToMovFloorCubeDic;
				}
			}
		}

		private void LoadFloorCubeDictionary()
		{
			var cubes = gcRef.cubeRefs;
			foreach (CubeRefHolder cube in cubes)
			{
				if (cube.floorCube == null) continue;
				AddToFloorCubeDic(cube.cubePos.FetchGridPos(), cube.floorCube);
			}

			var finish = gcRef.finishRef;
			AddToFloorCubeDic(finish.cubePos.FetchGridPos(), finish.floorCube);
		}

		private void AddToFloorCubeDic(Vector2Int pos, FloorCube cube)
		{
			if (floorCubeDic.ContainsKey(pos))
				Debug.Log("Overlapping cube " + cube + " & " + floorCubeDic[pos]);
			else floorCubeDic.Add(pos, cube);
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
				dic[cubeToShrink].refs.cubeShrink.StartShrinking();
			}
		}

		public void FromFloorToShrunkDic(Vector2Int cubePos, FloorCube cube)
		{
			//Always check movDic before checking floorDic
			if (movFloorCubeDic.ContainsKey(cubePos))
			{
				HandleFromFloorToShrunk(cubePos, cube, movFloorCubeDic, shrunkMovFloorCubeDic);
			}

			else if (floorCubeDic.ContainsKey(cubePos))
			{
				HandleFromFloorToShrunk(cubePos, cube, floorCubeDic, shrunkFloorCubeDic);
			}
		}

		public void HandleFromFloorToShrunk(Vector2Int cubePos, FloorCube cube, 
			Dictionary<Vector2Int, FloorCube> dic, Dictionary<Vector2Int, List<FloorCube>> shrunkDic)
		{
			if (dic != null) dic.Remove(cubePos);

			if (!shrunkDic.ContainsKey(cubePos))
			{
				List<FloorCube> floorCubeList = new List<FloorCube>();
				shrunkDic.Add(cubePos, floorCubeList);
			}

			if (shrunkDic[cubePos].Count > 0 && shrunkDic[cubePos][0] == cube) return;
			else shrunkDic[cubePos].Insert(0, cube);
		}

		public void FromShrunkToFloor(Vector2Int cubePos, FloorCube cube)
		{
			//Always check movDic before checking floorDic
			if (shrunkMovFloorCubeDic.ContainsKey(cubePos))
			{
				HandleFromShrunkToFLoor(cubePos, null, shrunkMovFloorCubeDic, null);
			}

			else if (shrunkFloorCubeDic.ContainsKey(cubePos))
			{
				HandleFromShrunkToFLoor(cubePos, cube, shrunkFloorCubeDic, floorCubeDic);
			}
		}

		public void HandleFromShrunkToFLoor(Vector2Int cubePos, FloorCube cube,
		Dictionary<Vector2Int, List<FloorCube>> shrunkDic, Dictionary<Vector2Int, FloorCube> dic)
		{
			//checking 'thisRewind' to make sure only the top floorcube off the
			//shrunkdic.value list is taken. Else all get taken
			if (shrunkToFloorThisRewind != cubePos)
			{
				shrunkToFloorThisRewind = cubePos;

				if (shrunkDic[cubePos].Count > 1)
					shrunkDic[cubePos].RemoveAt(0);
				else shrunkDic.Remove(cubePos);

				if (cube != null) dic.Add(cubePos, cube);
			}
		}

		private void InitialRecordCubes()
		{
			foreach (KeyValuePair<Vector2Int, FloorCube> pair in floorCubeDic)
			{
				var cube = pair.Value;
				if (cube.refs != null) TriggerRecord(cube.refs);
			}

			foreach (KeyValuePair<Vector2Int, List<FloorCube>> pair in shrunkFloorCubeDic)
			{
				foreach (var cube in pair.Value)
				{
					if (cube.refs != null) TriggerRecord(cube.refs);
				}
			}

			foreach (KeyValuePair<Vector2Int, FloorCube> pair in movFloorCubeDic)
			{
				var cube = pair.Value;
				if (cube.refs != null) TriggerRecord(cube.refs);
			}

			foreach (KeyValuePair<Vector2Int, List<FloorCube>> pair in shrunkMovFloorCubeDic)
			{
				foreach (var cube in pair.Value)
				{
					if (cube.refs != null) TriggerRecord(cube.refs);
				}
			}
		}

		private void TriggerRecord(CubeRefHolder cube)
		{
			onInitialCubeRecording(cube, cube.transform.position,
				cube.transform.rotation, cube.transform.localScale, cube.transform.rotation,
				cube.transform.localScale);
		}

		public bool CheckFloorCubeDicKey(Vector2Int cubePos)
		{
			if(floorCubeDic.ContainsKey(cubePos) || movFloorCubeDic.ContainsKey(cubePos)) return true;
			else return false;
		}

		public void AddToMovFloorCubeDic(Vector2Int cubePos, FloorCube cube)
		{
			if (!movFloorCubeDic.ContainsKey(cubePos))
				movFloorCubeDic.Add(cubePos, cube);
			else Debug.Log("movFloorCubeDic already contains cube " + cubePos);
		}

		public FloorCube FetchCube(Vector2Int cubePos, bool onlyNonShrunk)
		{
			if (!onlyNonShrunk)
			{
				if (shrunkFloorCubeDic.ContainsKey(cubePos))
					return shrunkFloorCubeDic[cubePos][0];
				else if (shrunkMovFloorCubeDic.ContainsKey(cubePos)) 
					return shrunkMovFloorCubeDic[cubePos][0];
			}

			if (floorCubeDic.ContainsKey(cubePos))
				return floorCubeDic[cubePos];
			else return movFloorCubeDic[cubePos];
		}

		public void ToggleCubeUI()
		{
			if (!gcRef.persRef.switchBoard.allowCubeUIToggle) return;

			foreach (KeyValuePair<Vector2Int, FloorCube> pair in floorCubeDic)
			{
				var cubeRef = pair.Value.refs;
				if (cubeRef != null && cubeRef.cubeUI != null)
				{
					if (cubeRef.cubeUI.debugShowCubeUI) cubeRef.cubeUI.ShowOrHideUI(false);
					cubeRef.cubeUI.debugShowCubeUI = !cubeRef.cubeUI.debugShowCubeUI;
				}
					
			}
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
					cube.onFloorKeyCheck -= CheckFloorCubeDicKey;
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
