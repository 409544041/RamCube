using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Qbism.Peep
{
	public class PeepWalkState : MonoBehaviour, IPeepBaseState
	{
		//Config parameters
		[SerializeField] float minPatrolPointDis = 1;
		[SerializeField] NavMeshAgent agent;

		//Cache
		PeepStateManager stateManager;
		public PeepPatrolPointManager pointManager { get; set; }

		//States
		Transform nexTarget;

		public void StateEnter(PeepStateManager psm)
		{
			if (stateManager == null) stateManager = psm;

			List<Transform> targets = new List<Transform>();

			foreach (var point in pointManager.patrolPoints)
			{
				if (Vector3.Distance(transform.position, point.transform.position) > minPatrolPointDis)
					targets.Add(point.transform);
			}

			var i = Random.Range(0, targets.Count);
			nexTarget = targets[i];
			agent.destination = nexTarget.position;

			// rotate peep to new destination direction
			// activate walking anim
		}

		public void StateUpdate(PeepStateManager psm)
		{
			//transform.LookAt(nexTarget.position);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "PatrolPoint")
			{
				stateManager.idleState.pointAction = other.GetComponent<IdlePointAction>().pointAction;
				stateManager.SwitchState(stateManager.idleState);
			}
		}
	}
}
