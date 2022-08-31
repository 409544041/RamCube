using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Cubes
{
	public interface ILaserEffector
	{
		void HandleHittingPlayer(bool bulletFart, float hitDist);
		void HandleHittingPlayerInBoost(Vector3 crossPoint, bool bulletFart);
		void GoIdle();
		void Close();
		bool GetClosedStatus();
	}
}
