using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Peep
{
	public class PeepBalloonIdleState : MonoBehaviour, IPeepBaseState
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

			refs.peepAnim.SetAnimLayerValue(1, 1);
		}

		public void StateUpdate(PeepStateManager psm)
		{
		}

		public void StateExit()
		{
		}
	}
}
