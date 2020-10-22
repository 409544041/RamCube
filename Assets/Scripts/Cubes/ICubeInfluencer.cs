using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Cubes
{
	public interface ICubeInfluencer
	{
		void PrepareAction(GameObject cube);
		void PrepareActionForMoveable(Transform side, Vector3 turnAxis,
			Vector2Int posAhead, GameObject cube, Vector2Int originPos, FloorCube prevCube);
		IEnumerator ExecuteActionOnPlayer(GameObject cube);
		IEnumerator ExecuteActionOnFF(GameObject cube);
		IEnumerator ExecuteActionOnMoveable(Transform side, Vector3 turnAxis,
			Vector2Int posAhead, GameObject cube, Vector2Int originPos, FloorCube prevCube);
	}
}
