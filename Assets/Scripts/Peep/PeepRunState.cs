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
			var hidePoints = pointMngr.SortHidePointsByDistance(pointMngr.hidePoints, transform.position);
			if (!continuePrevMovement) SetDestination(hidePoints);

			if (targetDest != null)
			{
				refs.aiRich.maxSpeed = runSpeed;
				refs.aiRich.destination = targetDest.position;
			}
			else stateManager.SwitchState(refs.cowerState);
		}

		public void StateUpdate(PeepStateManager psm)
		{
			if (Vector3.Distance(transform.position, targetDest.position) <=
				refs.aiRich.endReachedDistance)
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
				else targetDest = null;
			}
		}

		public void DestinationReached()
		{
			refs.aiRich.maxSpeed = 0;
			stateManager.SwitchState(refs.hideState);
		}

		public void StateExit()
		{
			continuePrevMovement = false;
		}
	}
}
