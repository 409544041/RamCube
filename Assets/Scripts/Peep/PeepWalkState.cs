using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Peep
{
	public class PeepWalkState : MonoBehaviour, IPeepBaseState
	{
		//Config parameters
		[SerializeField] float walkSpeed = 2;
		[SerializeField] float minPatrolPointDis = 1;

		//Cache
		PeepStateManager stateManager;

		//States
		Transform nexTarget;

		public void StateEnter(PeepStateManager psm)
		{
			if (stateManager == null) stateManager = psm;

			List<Transform> targets = new List<Transform>();

			foreach (var point in stateManager.pointManager.patrolPoints)
			{
				if (Vector3.Distance(transform.position, point.transform.position) > minPatrolPointDis)
					targets.Add(point.transform);
			}

			var i = Random.Range(0, targets.Count);
			nexTarget = targets[i];

			stateManager.refs.agent.speed = walkSpeed;
			stateManager.refs.agent.destination = nexTarget.position;

			// activate walking anim
		}

		public void StateUpdate(PeepStateManager psm)
		{
			if (stateManager.refs.agent.velocity.magnitude > .025f &&
				stateManager.refs.agent.velocity.magnitude < .1f)
				transform.rotation = 
					Quaternion.LookRotation(stateManager.refs.agent.velocity.normalized);
		}

		private void OnTriggerEnter(Collider other)
		{
			if ((Object)stateManager.currentState != this) return;

			if (other.tag == "PatrolPoint")
			{
				stateManager.refs.idleState.pointAction = other.GetComponent<IdlePointAction>().pointAction;
				stateManager.SwitchState(stateManager.refs.idleState);
			}
		}
	}
}
