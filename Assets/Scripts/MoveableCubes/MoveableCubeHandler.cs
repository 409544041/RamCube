using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.MoveableCubes
{
	public class MoveableCubeHandler : MonoBehaviour
	{
		//Cache
		MoveableCube[] moveableCubes = null;

		public Dictionary<Vector2Int, MoveableCube> moveableCubeDic = 
			new Dictionary<Vector2Int, MoveableCube>();

		public Dictionary<Vector3Int, GameObject> wallCubeDic =
			new Dictionary<Vector3Int, GameObject>();

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
					cube.onDictionaryRemove += RemoveFromDictionary;
				}
			}
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

		public void ActivateMoveableCube(Vector2Int cubePos, Vector3 turnAxis, Vector2Int playerPos)
		{
			var cube = FetchMoveableCube(cubePos);
			Transform side = null;
			Vector2Int posAhead = new Vector2Int(0, 0);

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

			if (cube.canMove) cube.InitiateMove(side, turnAxis, posAhead);
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

		private void RemoveFromDictionary(Vector2Int pos)
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
					cube.onDictionaryRemove -= RemoveFromDictionary;
				}
			}
		}
	}
}
