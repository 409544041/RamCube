using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.MoveableCubes
{
	public interface IMovingCube 
	{
		IEnumerator Move(Transform side, Vector3 turnAxis, Vector2Int posAhead);
		void UpdateCenterPosition();
		void PlayLandClip();
	}
}
