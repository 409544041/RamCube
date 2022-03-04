using Pathfinding;
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
		PeepRefHolder refs;

		//States
		Vector3 targetDest;
		Transform targetTrans;
		public bool continuePrevMovement { get; set; } = false;

		public void StateEnter(PeepStateManager psm)
		{
			if (stateManager == null)
			{
				stateManager = psm;
				refs = stateManager.refs;
			}

			if (!continuePrevMovement) SetDestination();

			if (targetDest != null)
			{
				refs.aiRich.maxSpeed = walkSpeed;
				refs.aiRich.destination = targetDest;
			}
			else stateManager.SwitchState(refs.idleState);
		}

		public void StateUpdate(PeepStateManager psm)
		{
			if (Vector3.Distance(transform.position, targetDest) <= 
				refs.aiRich.endReachedDistance)
				DestinationReached();
		}

		private void SetDestination()
		{
			List<Transform> targets = new List<Transform>();

			foreach (var point in stateManager.pointManager.patrolPoints)
			{
				if (Vector3.Distance(transform.position, point.transform.position) > minPatrolPointDis)
				{
					Path path = refs.pathSeeker.StartPath(transform.position, point.transform.position);
					path.BlockUntilCalculated();
					if (!path.error) targets.Add(point.transform);
				}
			}

			if (targets.Count > 0)
			{
				var i = Random.Range(0, targets.Count);
				targetTrans = targets[i];
				targetDest = targetTrans.position;
			}
			else
			{
				targetDest = new Vector3(float.PositiveInfinity, float.PositiveInfinity,
				float.PositiveInfinity);
				targetTrans = null;
			}
		}

		public void DestinationReached()
		{
			refs.aiRich.maxSpeed = 0;
			refs.idleState.pointAction = targetTrans.GetComponent<IdlePointAction>().pointAction;
			stateManager.SwitchState(refs.idleState);
		}

		public void StateExit()
		{
			continuePrevMovement = false;
		}
	}
}
