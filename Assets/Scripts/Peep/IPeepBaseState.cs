using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Peep
{	public interface IPeepBaseState
	{
		void StateEnter(PeepStateManager psm);
		void StateUpdate(PeepStateManager psm);
		void StateFixedUpdate(PeepStateManager psm);
	}
}
