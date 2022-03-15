using System.Collections;
using System.Collections.Generic;
using Qbism.MoveableCubes;
using UnityEngine;

namespace Qbism.Environment
{
	public class WallHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] GameLogicRefHolder glRef;
		//Cache
		MoveableCube[] moveableCubes = null;
		MoveableCubeHandler moveHandler;

		//States
		public Dictionary<Vector2Int, GameObject> wallCubeDic =
			new Dictionary<Vector2Int, GameObject>();

		private void Awake() 
		{
			moveableCubes = glRef.gcRef.movCubes; //TO DO: add movcube reffers
			moveHandler = glRef.movCubeHandler;
			LoadWallCubeDictionary();
		}

		private void OnEnable()
		{
			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onWallKeyCheck += CheckWallCubeDicKey;
					cube.onWallForCubeAheadCheck += CheckForWallAheadOfAhead;
				}
			}
		}

		private void LoadWallCubeDictionary()
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

		private bool CheckWallCubeDicKey(Vector2Int cubePos)
		{
			if (wallCubeDic.ContainsKey(cubePos)) return true;
			else return false;
		}

		public bool CheckForWallAheadOfAhead(Vector2Int posAhead, Vector2Int posAheadofAhead)
		{
			return moveHandler.moveableCubeDic[posAhead].
				CheckForWallAhead(posAhead, posAheadofAhead);
		}

		private void OnDisable()
		{
			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onWallKeyCheck -= CheckWallCubeDicKey;
					cube.onWallForCubeAheadCheck -= CheckForWallAheadOfAhead;
				}
			}
		}
	}
}
