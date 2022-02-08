using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Peep
{
	public class PeepStateManager : MonoBehaviour
	{
		//Config parameters
		public PeepIdleState idleState;
		public PeepWalkState walkState;
		public PeepRunState runState;

		//States
		IPeepBaseState currentState;
		string currentStateString;

		private void Start()
		{
			currentState = idleState;
			currentStateString = currentState.ToString();
			currentState.StateEnter(this);
		}

		private void Update()
		{
			currentState.StateUpdate(this);
		}

		public void SwitchState(IPeepBaseState state)
		{
			currentState = state;
			currentStateString = currentState.ToString();
			state.StateEnter(this);
		}
	}
}
