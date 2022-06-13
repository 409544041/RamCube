using Qbism.Cubes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.MoveableCubes
{
	public class MoveableCubeHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] GameplayCoreRefHolder gcRef;

		//Cache
		MoveableCube[] moveableCubes = null;

		//States
		public int movingMoveables { get; set; } = 0;
		public int moveablesMovedThisTurn { get; set; } = 0;

		public Dictionary<Vector2Int, MoveableCube> moveableCubeDic = 
			new Dictionary<Vector2Int, MoveableCube>();

		public event Action<CubeRefHolder, Vector3, Quaternion, Vector3, Quaternion, Vector3> onInitialCubeRecording;
		public event Action<bool> onSetAllowRewind;

		private void Awake() 
		{
			moveableCubes = gcRef.movCubes;
			
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

		private void LoadMoveableCubeDictionary()
		{
			var cubeRefs = gcRef.cubeRefs;
			foreach (CubeRefHolder cube in cubeRefs)
			{
				if (cube.movCube == null) continue;

				AddToMoveableDic(cube.cubePos.FetchGridPos(), cube.movCube);
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

			if (!cube.CheckForWallAhead(cubePos, posAhead))
				RemoveFromMoveableDic(cubePos);

			CalculateSide(cubePos, activatorPos, cube, ref side, ref posAhead);
			cube.InitiateMove(side, turnAxis, posAhead, originPos);;
		}

		public void CheckForMovingMoveables()
		{
			if (movingMoveables == 0)
			{
				onSetAllowRewind(true);
				gcRef.pRef.playerMover.initiatedByPlayer = true;
			}
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
			moveable.newPlayerMove = false;
			CheckForMovingMoveables();
		}

		public void InitialRecordMoveables()
		{
			foreach(KeyValuePair<Vector2Int, MoveableCube> pair in moveableCubeDic)
			{
				var cube = pair.Value;

				Quaternion faceRot = new Quaternion(0, 0, 0, 0);
				Vector3 faceScale = Vector3.one;
				if (cube.refs.movFaceMesh != null)
				{
					faceRot = cube.refs.movFaceMesh.transform.rotation;
					faceScale = cube.refs.movFaceMesh.transform.localScale;
				}

				onInitialCubeRecording(cube.refs, cube.transform.position,
					cube.transform.rotation, cube.transform.localScale, faceRot,
					faceScale);
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
				cube.newPlayerMove = false;
			}
		}

		public void InstantFinishMovingMoveables()
		{
			foreach (var cube in moveableCubes)
			{
				cube.newPlayerMove = true;
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
