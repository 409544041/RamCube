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

		//States
		public int movingMoveables { get; set; } = 0;
		public int moveablesMovedThisTurn { get; set; } = 0;

		public Dictionary<Vector2Int, MoveableCube> moveableCubeDic = 
			new Dictionary<Vector2Int, MoveableCube>();

		public event Action<MoveableCube, Vector3, Quaternion, Vector3> onInitialCubeRecording;
		public event Action<bool> onSetPlayerInput;

		private void Awake() 
		{
			moveableCubes = FindObjectsOfType<MoveableCube>(); //TO DO: add movcube reffers
			
			LoadMoveableCubeDictionary(); 
		}

		private void OnEnable() 
		{
			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onMoveableKeyCheck += CheckMoveableCubeDicKey;
					cube.onStartMovingMoveable += StartMovingMoveable;
					cube.onStopMovingMoveable += StopMovingMoveables;
				}
			}
		}

		public void LoadMoveableCubeDictionary()
		{
			MoveableCube[] cubes = FindObjectsOfType<MoveableCube>();
			foreach (MoveableCube cube in cubes)
			{
				var cubePos = cube.cubePoser.FetchGridPos();
				if (moveableCubeDic.ContainsKey(cubePos))
					Debug.Log("Overlapping moveable cube " + cubePos);
				else moveableCubeDic.Add(cubePos, cube);
			}
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

		public void ActivateMoveableCube(Vector2Int cubePos,
			Vector3 turnAxis, Vector2Int activatorPos)
		{
			var cube = FetchMoveableCube(cubePos);
			Transform side = null;
			Vector2Int posAhead = new Vector2Int(0, 0);
			Vector2Int originPos = cubePos;

			CalculateSide(cubePos, activatorPos, cube, ref side, ref posAhead);
			cube.InitiateMove(side, turnAxis, posAhead, originPos);

			if (!cube.CheckForWallAhead(cubePos, posAhead)) 
				RemoveFromMoveableDic(cubePos);
		}

		public void CheckForMovingMoveables()
		{
			if (movingMoveables == 0) onSetPlayerInput(true);
		}

		public void StartMovingMoveable(Vector2Int posAhead, Vector3 turnAxis,
			Vector2Int pos)
		{
			moveableCubeDic[posAhead].ApplyOrderOfMovement(moveablesMovedThisTurn);
			moveablesMovedThisTurn++;
			movingMoveables++;
			ActivateMoveableCube(posAhead, turnAxis, pos);
		}

		public void StopMovingMoveables(Vector2Int cubePos, MoveableCube moveable,
			bool becomeFloor)
		{
			movingMoveables--;
			if (!becomeFloor) AddToMoveableDic(cubePos, moveable);
			CheckForMovingMoveables();
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

		private MoveableCube FetchMoveableCube(Vector2Int cubePos)
		{
			return moveableCubeDic[cubePos];
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

		public void ResetMovedMoveables()
		{
			moveablesMovedThisTurn = 0;

			foreach (var cube in moveableCubes)
			{
				cube.orderOfMovement = -1;
			}
		}

		private void OnDisable()
		{
			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onMoveableKeyCheck -= CheckMoveableCubeDicKey;
					cube.onStartMovingMoveable -= StartMovingMoveable;
					cube.onStopMovingMoveable -= StopMovingMoveables;
				}
			}
		}
	}
}
