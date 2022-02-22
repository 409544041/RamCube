using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Peep
{
	public class PeepIdleState : MonoBehaviour, IPeepBaseState
	{
		//Config parameters
		public Vector2 idleTimeMinMax = new Vector2(4, 8);

		//Cache
		PeepStateManager stateManager;

		//States
		float idleTimer { get; set; } = 0;
		float timeToIdle;
		public IdlePointActions pointAction { get; set; }

		public void StateEnter(PeepStateManager psm)
		{
			if (stateManager == null) stateManager = psm;

			ResetIdleTime();
		}

		public void StateUpdate(PeepStateManager psm)
		{
			idleTimer += Time.deltaTime;
			if (idleTimer > timeToIdle) stateManager.SwitchState(stateManager.refs.walkState);
		}

		private void ResetIdleTime()
		{
			idleTimer = 0;
			timeToIdle = Random.Range(idleTimeMinMax.x, idleTimeMinMax.y);
		}
	}
}
