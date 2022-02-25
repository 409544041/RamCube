using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Peep
{
	public class PeepInvestigateState : MonoBehaviour, IPeepBaseState
	{
		//Cache
		PeepStateManager stateManager;
		PeepRefHolder refs;
		public GameObject player { get; set; }

		public void StateEnter(PeepStateManager psm)
		{
			if (stateManager == null)
			{
				stateManager = psm;
				refs = stateManager.refs;
			}

			var playerDir = (player.transform.position - transform.position).normalized;
			var playerDirV2 = new Vector2(playerDir.x, playerDir.z);

			if (FetchAngle(playerDirV2) < 45) TriggerReaction();
			else StartCoroutine(TurnTowardsPlayer(playerDirV2, playerDir));
		}

		public void StateUpdate(PeepStateManager psm)
		{
		}

		private float FetchAngle(Vector2 playerDirV2)
		{
			var dirV2 = new Vector2(transform.forward.x, transform.forward.z);
			var angle = Vector2.Angle(playerDirV2, dirV2);
			return angle;
		}

		private IEnumerator TurnTowardsPlayer(Vector2 playerDirV2, Vector3 playerDir)
		{
			while (FetchAngle(playerDirV2) > 10)
			{
				var newDir = Vector3.RotateTowards(transform.forward, playerDir, 200 * Time.deltaTime, 0.0f);
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
				//trigger expression vfx
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
