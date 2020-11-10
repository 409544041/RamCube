using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.MoveableCubes
{
	public interface IActiveCube 
	{
		void RoundPosition();
		Vector2Int FetchGridPos();
	}
}
