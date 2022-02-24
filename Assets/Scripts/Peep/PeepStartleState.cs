using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Peep
{
	public class PeepStartleState : MonoBehaviour, IPeepBaseState
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
			StartCoroutine(TriggerStartleAnim());
		}

		public void StateUpdate(PeepStateManager psm)
		{
		}

		private IEnumerator TriggerStartleAnim()
		{
			refs.peepAnim.TriggerStartle();
			var animDur = refs.animator.GetCurrentAnimatorClipInfo(0).Length;
			float timer = 0;

			while (timer < animDur)
			{
				timer += Time.deltaTime;
				yield return null;
			}

			if (stateManager.peepType == PeepTypes.scared) GoToRunState();

			else if (stateManager.peepType == PeepTypes.notScared) ContinuePrevState();
		}

		private void GoToRunState()
		{
			IPeepBaseState nextState = refs.runState;

			if (stateManager.prevState == nextState)
				refs.runState.continuePrevMovement = true;

			stateManager.SwitchState(nextState);
		}

		private void ContinuePrevState()
		{
			IPeepBaseState nextState = stateManager.prevState;

			if ((Object)stateManager.prevState == refs.walkState)
				refs.walkState.continuePrevMovement = true;
			if ((Object)stateManager.prevState == refs.runState)
				refs.runState.continuePrevMovement = true;
			if ((Object)stateManager.prevState == refs.idleState)
				refs.idleState.continueCounting = true;

			stateManager.SwitchState(nextState);
		}

		public void StateExit()
		{
		}
	}
}
