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
		bool isMoving;
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
			if (!continuePrevMovement) SetNavTarget(hidePoints);

			refs.agent.speed = runSpeed;
			refs.agent.destination = targetDest.position;
			refs.peepMover.PrepareMove(targetDest, this);
			isMoving = true;
		}

		public void StateUpdate(PeepStateManager psm)
		{
			if (isMoving) refs.peepMover.MoveWithSmoothRotation(runSpeed);
		}

		private void SetNavTarget(GameObject[] points)
		{
			for (int i = 0; i < points.Length; i++)
			{
				var newPath = new NavMeshPath();

				if (refs.agent.CalculatePath(points[i].transform.position, newPath))
				{
					targetDest = points[i].transform;
					return;
				}
			}
		}

		public void DestinationReached()
		{
			isMoving = false;
			stateManager.SwitchState(refs.hideState);
		}

		public void StateExit()
		{
			isMoving = false;
			refs.agent.isStopped = true;
			continuePrevMovement = false;
		}
	}
}
