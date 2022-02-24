using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Qbism.Peep
{
	public class PeepWalkState : MonoBehaviour, IPeepBaseState, IPeepMovement
	{
		//Config parameters
		[SerializeField] float walkSpeed = 2;
		[SerializeField] float minPatrolPointDis = 1;

		//Cache
		PeepStateManager stateManager;
		NavMeshAgent agent;
		PeepRefHolder refs;

		//States
		Transform targetDest;
		bool isMoving;
		public bool continuePrevMovement { get; set; } = false;

		public void StateEnter(PeepStateManager psm)
		{
			if (stateManager == null)
			{
				stateManager = psm;
				refs = stateManager.refs;
				agent = refs.agent;
			}

			if (!continuePrevMovement) SetDestination();

			agent.speed = walkSpeed;
			agent.destination = targetDest.position;
			refs.peepMover.PrepareMove(targetDest, this);
			isMoving = true;
		}

		public void StateUpdate(PeepStateManager psm)
		{
			if (isMoving) refs.peepMover.MoveWithSmoothRotation(walkSpeed);
			//somehow agent.move and agent.destination work good together. Probably shouldn't though.
		}

		private void SetDestination()
		{
			List<Transform> targets = new List<Transform>();

			foreach (var point in stateManager.pointManager.patrolPoints)
			{
				var newPath = new NavMeshPath();

				if (Vector3.Distance(transform.position, point.transform.position) > minPatrolPointDis &&
					refs.agent.CalculatePath(point.transform.position, newPath))
					targets.Add(point.transform);
			}

			var i = Random.Range(0, targets.Count);
			targetDest = targets[i];
		}

		public void DestinationReached()
		{
			isMoving = false;
			refs.idleState.pointAction = targetDest.GetComponent<IdlePointAction>().pointAction;
			stateManager.SwitchState(refs.idleState);
		}

		public void StateExit()
		{
			isMoving = false;
			agent.isStopped = true;
			continuePrevMovement = false;
		}
	}
}
