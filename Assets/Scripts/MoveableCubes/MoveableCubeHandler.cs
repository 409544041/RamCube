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

		public Dictionary<Vector2Int, MoveableCube> moveableCubeDic = 
			new Dictionary<Vector2Int, MoveableCube>();

		public Dictionary<Vector3Int, GameObject> wallCubeDic =
			new Dictionary<Vector3Int, GameObject>();

		public event Action<MoveableCube> onRecordStart;
		public event Action onRecordStop;
		public event Action<bool> onSetPlayerInput;
		public event Action<MoveableCube, Vector3, Quaternion, Vector3> onInitialCubeRecording;

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
					cube.onMoveableKeyCheck += CheckMoveableCubeDicKey;
					cube.onActivateOtherMoveable += ActivateMoveableCube;
					cube.onMovingCheck += FetchMovingStatus;
				}
			}
		}

		private void Update()
		{
			CheckForMovingMoveables();
		}

		public void LoadMoveableCubeDictionary()
		{
			MoveableCube[] cubes = FindObjectsOfType<MoveableCube>();
			foreach (MoveableCube cube in cubes)
			{
				if(cube.isDocked) continue;

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

		public bool CheckForMovingMoveables()
		{
			if (moveableCubeDic.Count == 0) return false;

			int amountNotMoving = 0;

			foreach (KeyValuePair<Vector2Int, MoveableCube> pair in moveableCubeDic)
			{
				var cube = pair.Value;

				if (!cube.isMoving) amountNotMoving++;
			}

			if (amountNotMoving == moveableCubeDic.Count)
				return false;
			else return true;
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

		public void StartRecordingMoveables()
		{
			foreach (KeyValuePair<Vector2Int, MoveableCube> pair in moveableCubeDic)
			{
				var cube = pair.Value;
				onRecordStart(cube);
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

		private bool FetchMovingStatus(Vector2Int cubePos)
		{
			return moveableCubeDic[cubePos].isMoving;
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
					cube.onMovingCheck -= FetchMovingStatus;
				}
			}
		}
	}
}
