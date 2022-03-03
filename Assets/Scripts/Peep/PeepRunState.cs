using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Qbism.Peep
{
	public class PeepRunState : MonoBehaviour, IPeepBaseState, IPeepMovement
	{
		//Config parameters
		[SerializeField] float runSpeed = 1.5f;

		//Cache
		PeepStateManager stateManager;
		PeepRefHolder refs;

		//States
		public Transform targetDest { get; private set; }
		public bool continuePrevMovement { get; set; } = false;

		public void StateEnter(PeepStateManager psm)
		{
			if (stateManager == null)
			{
				stateManager = psm;
				refs = stateManager.refs;
			}

			var pointMngr = stateManager.pointManager;
			var hidePoints = pointMngr.SortHidePointsByDistance(pointMngr.hidePoints);
			if (!continuePrevMovement) SetDestination(hidePoints);

			refs.aiPath.maxSpeed = runSpeed;
			refs.aiPath.destination = targetDest.position;
		}

		public void StateUpdate(PeepStateManager psm)
		{
			if (Vector3.Distance(transform.position, targetDest.position) <=
				refs.aiPath.endReachedDistance)
				DestinationReached();
		}

		private void SetDestination(GameObject[] points)
		{
			for (int i = 0; i < points.Length; i++)
			{
				Path path = refs.pathSeeker.StartPath(transform.position, points[i].transform.position);
				path.BlockUntilCalculated();
				if (!path.error)
				{
					targetDest = points[i].transform;
					return;
				}
			}
		}

		public void DestinationReached()
		{
			stateManager.SwitchState(refs.hideState);
		}

		public void StateExit()
		{
			continuePrevMovement = false;
		}
	}
}
