using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Peep
{
	public class PeepStateManager : MonoBehaviour
	{
		//Config parameters
		public PeepRefHolder refs;
		public PeepBravery peepBravery;
		public PeepJobs peepJob;
		[SerializeField] bool runToHide, startle;

		//Cache
		public PeepNavPointManager pointManager {  get; set; }
		public GameObject player { get; set; }

		//States
		public IPeepBaseState currentState { get; private set; }
		public IPeepBaseState prevState { get; private set; }
		string currentStateString; //just for easy reading in debug inspector
		string prevStateString;
		bool runTriggered = false;
		bool startleTriggered = false;
		public bool coward { get; private set; } = false;
		public bool indifferent { get; private set; } = false;
		public bool brave { get; private set; } = false;

		private void Start()
		{
			if (peepBravery == PeepBravery.coward) coward = true;
			if (peepBravery == PeepBravery.indifferent) indifferent = true;
			if (peepBravery == PeepBravery.brave) brave = true;

			if (peepJob == PeepJobs.balloon) currentState = refs.balloonIdleState;
			else currentState = refs.idleState;
			currentStateString = currentState.ToString();
			currentState.StateEnter(this);
		}

		private void Update()
		{
			if (!runTriggered && runToHide)
			{
				runTriggered = true;
				SwitchState(refs.runState);
			}

			if (!startleTriggered && startle)
			{
				startleTriggered = true;
				SwitchState(refs.investigateState);
			}

			currentState.StateUpdate(this);
		}

		public void SwitchState(IPeepBaseState state)
		{
			prevState = currentState;
			currentState = state;
			currentStateString = currentState.ToString();
			prevStateString = prevState.ToString();
			prevState.StateExit();
			state.StateEnter(this);
		}
	}
}
