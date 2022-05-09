using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Peep
{
	public class PeepCowerState : MonoBehaviour, IPeepBaseState
	{
		//Cache
		PeepStateManager stateManager;
		PeepRefHolder refs;

		public void StateEnter(PeepStateManager psm)
		{
			if (stateManager == null)
			{
				stateManager = psm;
				refs = stateManager.refs;
			}
			//TO DO: activate cowering animation
			refs.aiRich.maxSpeed = 0;
			print("Cowering");
		}

		public void StateUpdate(PeepStateManager psm)
		{
		}

		public void StateExit()
		{
		}
	}

}