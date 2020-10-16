using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.MoveableCubes
{
	public interface IActiveCube 
	{
		void RoundPosition();
		void CheckFloorInNewPos();
		Vector2Int FetchGridPos();
	}
}
