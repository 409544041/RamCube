using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.MoveableCubes
{
	public class MoveableCubeHandler : MonoBehaviour
	{
		//Cache
		MoveableCube[] moveableCubes = null;
		FloorComponentAdder[] compAdders = null;

		//States
		public int movingMoveables { get; set; } = 0;

		public Dictionary<Vector2Int, MoveableCube> moveableCubeDic = 
			new Dictionary<Vector2Int, MoveableCube>();

		public Dictionary<Vector2Int, GameObject> wallCubeDic =
			new Dictionary<Vector2Int, GameObject>(); //TO DO: Move this to a separate script

		public event Action<MoveableCube, Vector3, Quaternion, Vector3> onInitialCubeRecording;
		public event Action<bool> onSetPlayerInput;

		private void Awake() 
		{
			moveableCubes = FindObjectsOfType<MoveableCube>();
			compAdders = FindObjectsOfType<FloorComponentAdder>();
			
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
					cube.onMoveableKeyCheck += CheckMoveableCubeDicKey;
					cube.onActivateOtherMoveable += ActivateMoveableCube;
					cube.onAddToMovDic += AddToMoveableDic;
					cube.onRemoveFromMovDic += RemoveFromMoveableDic;
					cube.onMovingMoveablesCheck += CheckForMovingMoveables;
					cube.onEditMovingMoveables += AddOrRemoveFromMovingMovables;
				}
			}

			if (compAdders != null)
			{
				foreach (var adder in compAdders)
				{
					adder.onRemoveFromMovDic += RemoveFromMoveableDic;
				}
			}
		}

		public void LoadMoveableCubeDictionary()
		{
			MoveableCube[] cubes = FindObjectsOfType<MoveableCube>();
			foreach (MoveableCube cube in cubes)
			{
				var cubePos = cube.FetchGridPos();
				if (moveableCubeDic.ContainsKey(cubePos))
					Debug.Log("Overlapping moveable cube " + cubePos);
				else moveableCubeDic.Add(cubePos, cube);
			}
		}

		private void LoadWallCubeDictionary() //This is used by moveable cubes to detect walls
		{
			GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

			foreach (var wall in walls)
			{
				Vector2Int wallPos = new Vector2Int(Mathf.RoundToInt(wall.transform.position.x),
					Mathf.RoundToInt(wall.transform.position.z));

				if (!wallCubeDic.ContainsKey(wallPos))
					wallCubeDic.Add(wallPos, wall);
			}
		}

		public void ActivateMoveableCube(Vector2Int cubePos, Vector3 turnAxis, Vector2Int activatorPos)
		{
			var cube = FetchMoveableCube(cubePos);
			Transform side = null;
			Vector2Int posAhead = new Vector2Int(0, 0);
			Vector2Int originPos = cubePos;

			CalculateSide(cubePos, activatorPos, cube, ref side, ref posAhead);

			cube.InitiateMove(side, turnAxis, posAhead, originPos);
		}

		private void CalculateSide(Vector2Int cubePos, Vector2Int activatorPos, MoveableCube cube, ref Transform side, ref Vector2Int posAhead)
		{
			if (CheckDeltaY(cubePos, activatorPos) > 0)
			{
				side = cube.up;
				posAhead = cubePos + Vector2Int.up;
			}
			else if (CheckDeltaY(cubePos, activatorPos) < 0)
			{
				side = cube.down;
				posAhead = cubePos + Vector2Int.down;
			}
			else if (CheckDeltaX(cubePos, activatorPos) < 0)
			{
				side = cube.left;
				posAhead = cubePos + Vector2Int.left;
			}
			else if (CheckDeltaX(cubePos, activatorPos) > 0)
			{
				side = cube.right;
				posAhead = cubePos + Vector2Int.right;
			}
		}

		public void CheckForMovingMoveables()
		{
			if (movingMoveables == 0) onSetPlayerInput(true);
		}

		private void AddOrRemoveFromMovingMovables(int Value)
		{
			movingMoveables += Value;
		}

		public void InitialRecordMoveables()
		{
			foreach(KeyValuePair<Vector2Int, MoveableCube> pair in moveableCubeDic)
			{
				var cube = pair.Value;
				onInitialCubeRecording(cube, cube.transform.position,
					cube.transform.rotation, cube.transform.localScale);
			}
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

		public bool CheckWallCubeDicKey(Vector2Int cubePos)
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
			if (!moveableCubeDic.ContainsKey(pos))
				moveableCubeDic.Add(pos, cube);
			else Debug.Log("moveableDic already contains cube " + pos);
		}

		public void RemoveFromMoveableDic(Vector2Int pos)
		{
			if (moveableCubeDic.ContainsKey(pos))
				moveableCubeDic.Remove(pos);
		}

		private void OnDisable()
		{
			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onWallKeyCheck -= CheckWallCubeDicKey;
					cube.onMoveableKeyCheck -= CheckMoveableCubeDicKey;
					cube.onActivateOtherMoveable -= ActivateMoveableCube;
					cube.onAddToMovDic -= AddToMoveableDic;
					cube.onRemoveFromMovDic -= RemoveFromMoveableDic;
					cube.onMovingMoveablesCheck -= CheckForMovingMoveables;
					cube.onEditMovingMoveables -= AddOrRemoveFromMovingMovables;
				}
			}

			if (compAdders != null)
			{
				foreach (var adder in compAdders)
				{
					adder.onRemoveFromMovDic -= RemoveFromMoveableDic;
				}
			}
		}
	}
}
