using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.MoveableCubes
{
	public class MoveableCubeHandler : MonoBehaviour
	{
		//States
		public bool isRecording { get; set; }= false;

		//Cache
		MoveableCube[] moveableCubes = null;

		public Dictionary<Vector2Int, MoveableCube> moveableCubeDic = 
			new Dictionary<Vector2Int, MoveableCube>();

		public Dictionary<Vector3Int, GameObject> wallCubeDic =
			new Dictionary<Vector3Int, GameObject>();

		public event Action onRecordStart;
		public event Action onRecordStop;
		public event Action<bool> onSetPlayerInput;

		private void Awake() 
		{
			moveableCubes = FindObjectsOfType<MoveableCube>();
			
			LoadMoveableCubeDictionary();
			LoadWallCubeDictionary();
		}

		private void OnEnable() 
		{
			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onWallKeyCheck += CheckWallCubeDicKey;
					cube.onDictionaryRemove += RemoveFromMoveableDic;
					cube.onDictionaryAdd += AddToMoveableDic;
				}
			}
		}

		private void Update()
		{
			CheckForRecording();
			print(isRecording);
		}

		private void LoadMoveableCubeDictionary()
		{
			MoveableCube[] cubes = FindObjectsOfType<MoveableCube>();
			foreach (MoveableCube cube in cubes)
			{
				if (moveableCubeDic.ContainsKey(cube.FetchGridPos()))
					print("Overlapping cube " + cube);
				else moveableCubeDic.Add(cube.FetchGridPos(), cube);
			}
		}

		private void LoadWallCubeDictionary()
		{
			GameObject[] cubes = GameObject.FindGameObjectsWithTag("Wall");
			foreach (GameObject cube in cubes)
			{
				Vector3Int cubePos = new Vector3Int(Mathf.RoundToInt(cube.transform.position.x),
					Mathf.RoundToInt(cube.transform.position.y),
					Mathf.RoundToInt(cube.transform.position.z));

				if (wallCubeDic.ContainsKey(cubePos))
					print("Overlapping cube " + cube);
				else wallCubeDic.Add(cubePos, cube);
			}
		}

		private void CheckForRecording()
		{
			if (moveableCubeDic.Count == 0) return;

			int amountNotMoving = 0;
			
			foreach (KeyValuePair<Vector2Int, MoveableCube> pair in moveableCubeDic)
			{
				var cube = pair.Value;
				if (cube.isMoving && isRecording == false)
				{
					isRecording = true;
					onRecordStart();
					return;
				}
				else if (!cube.isMoving)
				{
					amountNotMoving++;
				}
			}
			print("Amount not moving = " + amountNotMoving);
			print("Dic count = " + moveableCubeDic.Count);

			if (amountNotMoving == moveableCubeDic.Count && isRecording == true)
			{
				isRecording = false;
				onSetPlayerInput(true);
				onRecordStop();
			}
		}

		public void ActivateMoveableCube(Vector2Int cubePos, Vector3 turnAxis, Vector2Int playerPos)
		{
			var cube = FetchMoveableCube(cubePos);
			Transform side = null;
			Vector2Int posAhead = new Vector2Int(0, 0);
			Vector2Int originPos = cubePos;

			if (CheckDeltaY(cubePos, playerPos) > 0)
			{
				side = cube.up;
				posAhead = cubePos + Vector2Int.up;
			}
			else if (CheckDeltaY(cubePos, playerPos) < 0)
			{
				side = cube.down;
				posAhead = cubePos + Vector2Int.down;
			}
			else if (CheckDeltaX(cubePos, playerPos) < 0)
			{
				side = cube.left;
				posAhead = cubePos + Vector2Int.left;
			} 
			else if (CheckDeltaX(cubePos, playerPos) > 0)
			{
				side = cube.right;
				posAhead = cubePos + Vector2Int.right;
			} 

			if (cube.canMove) cube.InitiateMove(side, turnAxis, posAhead, originPos);
		}

		public int CheckDeltaX(Vector2Int posA, Vector2Int posB)
		{
			return posA.x - posB.x;
		}

		public int CheckDeltaY(Vector2Int posA, Vector2Int posB)
		{
			return posA.y - posB.y;
		}

		public bool CheckMoveableCubeDicKey(Vector2Int cubePos)
		{
			if (moveableCubeDic.ContainsKey(cubePos)) return true;
			else return false;
		}

		public bool CheckWallCubeDicKey(Vector3Int cubePos)
		{
			if (wallCubeDic.ContainsKey(cubePos)) return true;
			else return false;
		}

		private MoveableCube FetchMoveableCube(Vector2Int cubePos)
		{
			return moveableCubeDic[cubePos];
		}

		public void AddToMoveableDic(Vector2Int pos, MoveableCube cube)
		{
			moveableCubeDic.Add(pos, cube);
		}

		public void RemoveFromMoveableDic(Vector2Int pos)
		{
			moveableCubeDic.Remove(pos);
		}

		private void OnDisable()
		{
			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onWallKeyCheck -= CheckWallCubeDicKey;
					cube.onDictionaryRemove -= RemoveFromMoveableDic;
					cube.onDictionaryAdd -= AddToMoveableDic;
				}
			}
		}
	}
}
