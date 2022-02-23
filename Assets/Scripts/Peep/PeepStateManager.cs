using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Peep
{
	public class PeepStateManager : MonoBehaviour
	{
		//Config parameters
		public PeepRefHolder refs;
		[SerializeField] bool hide;

		//Cache
		public PeepNavPointManager pointManager {  get; set; }

		//States
		public IPeepBaseState currentState { get; private set; }
		string currentStateString; //just for easy reading in debug inspector
		bool hidingInitiated = false;

		private void Start()
		{
			currentState = refs.idleState;
			currentStateString = currentState.ToString();
			currentState.StateEnter(this);
		}

		private void Update()
		{
			if (!hidingInitiated && hide)
			{
				hidingInitiated = true;
				SwitchState(refs.runState);
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
