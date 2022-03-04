using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Peep
{
	public class PeepInvestigateState : MonoBehaviour, IPeepBaseState
	{
		//Config parameters
		[SerializeField] Vector2 turnDelayMinMax;

		//Cache
		PeepStateManager stateManager;
		PeepRefHolder refs;
		public GameObject investigateObject { get; set; }

		//States
		Vector3 playerDir;
		Vector2 playerDirV2;
		Vector2 peepDirV2;

		public void StateEnter(PeepStateManager psm)
		{
			if (stateManager == null)
			{
				stateManager = psm;
				refs = stateManager.refs;
			}

			refs.expressionHandler.SetQuestionExprSignal();

			SetDirections();

			if (FetchAngle() < 45) TriggerReaction();
			else StartCoroutine(TurnTowardsPlayer());
		}

		public void StateUpdate(PeepStateManager psm)
		{
			SetDirections();
		}

		private float FetchAngle()
		{
			var angle = Vector3.Angle(playerDirV2, peepDirV2);
			return angle;
		}

		private IEnumerator TurnTowardsPlayer()
		{
			var delay = Random.Range(turnDelayMinMax.x, turnDelayMinMax.y);
			yield return new WaitForSeconds(delay);

			while (FetchAngle() > 10)
			{
				var newDir = Vector3.RotateTowards(transform.forward, 
					new Vector3(playerDir.x, transform.forward.y, playerDir.z), 200 * Time.deltaTime, 0.0f);
				var newRot = Quaternion.LookRotation(newDir);
				transform.rotation = Quaternion.Slerp(transform.rotation, newRot, 3 * Time.deltaTime);

				yield return null;
			}

			TriggerReaction();
		}

		private void TriggerReaction()
		{
			if (stateManager.peepType == PeepTypes.scared)
			{
				StartCoroutine(TriggerAnim("Startle"));
				refs.expressionHandler.SetShockExprSignal();
			}
			else
			{
				StartCoroutine(TriggerAnim("Shrug"));
			}
		}

		private IEnumerator TriggerAnim(string animTrigger)
		{
			refs.peepAnim.TriggerAnim(animTrigger);
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

		private void SetDirections()
		{
			playerDir = (investigateObject.transform.position - transform.position).normalized;
			playerDirV2 = new Vector2(playerDir.x, playerDir.z);
			peepDirV2 = new Vector2(transform.forward.x, transform.forward.z);
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
