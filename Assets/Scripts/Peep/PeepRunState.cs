using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Peep
{
	public class PeepRunState : MonoBehaviour, IPeepBaseState
	{
		//Cache
		PeepStateManager stateManager;

		public void StateEnter(PeepStateManager psm)
		{
			if (stateManager == null) stateManager = psm;
			// trigger startled anim 
			// trigger running anim
			// find target (nearest structure / hide point)
		}

		public void StateUpdate(PeepStateManager psm)
		{
			//navmesh agent move to destination
		}

		private void OnTriggerEnter(Collider other)
		{
			//if trigger = house || bush > shrinking juice to make it look like they shrank into the house
		}
	}
}
