using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Cubes
{
	public class Magnet : MonoBehaviour, ILaserEffector
	{
		//Config parameters
		[SerializeField] LaserRefHolder refs;
		
		//States
		public bool isClosed { get; set; } = false;

		public void GoIdle()
		{
			throw new System.NotImplementedException();
		}

		public void HandleHittingPlayer(bool bulletFart, float hitDist)
		{
			throw new System.NotImplementedException();
		}

		public void HandleHittingPlayerInBoost(Vector3 crossPoint, bool bulletFart)
		{
			throw new System.NotImplementedException();
		}

		public bool GetClosedStatus()
		{
			return isClosed;
		}
	}
}
