using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Qbism.Peep
{
	public class PeepStateManager : MonoBehaviour
	{
		//Config parameters
		public PeepIdleState idleState;
		public PeepWalkState walkState;
		public PeepRunState runState;
		public NavMeshAgent agent;
		[SerializeField] bool hide;

		//Cache
		public PeepNavPointManager pointManager {  get; set; }

		//States
		public IPeepBaseState currentState { get; private set; }
		string currentStateString; //just for easy reading in debug inspector
		bool hidingInitiated = false;

		private void Start()
		{
			currentState = idleState;
			currentStateString = currentState.ToString();
			currentState.StateEnter(this);
		}

		private void Update()
		{
			if (!hidingInitiated && hide)
			{
				hidingInitiated = true;
				SwitchState(runState);
			}
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
