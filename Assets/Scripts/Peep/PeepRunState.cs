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
		[SerializeField] float addStopDist = .25f;

		//Cache
		PeepStateManager stateManager;
		PeepRefHolder refs;

		//States
		public Vector3 targetDest { get; private set; }
		public Transform targetTrans { get; private set; }
		public bool continuePrevMovement { get; set; } = false;
		Vector3 nodePos;
		float stopDist;

		public void StateEnter(PeepStateManager psm)
		{
			if (stateManager == null)
			{
				stateManager = psm;
				refs = stateManager.refs;
				stopDist = refs.aiRich.endReachedDistance;
				if (stateManager.brave) stopDist = refs.aiRich.endReachedDistance + addStopDist;
			}

			if (stateManager.coward)
			{
				var pointMngr = stateManager.pointManager;
				var hidePoints = pointMngr.SortHidePointsByDistance(pointMngr.hidePoints, transform.position);
				if (!continuePrevMovement) SetHideDest(hidePoints);
			}
			else if (stateManager.brave) SetAttackPos();

			if (targetDest.x < float.PositiveInfinity)
			{
				refs.aiRich.maxSpeed = runSpeed;
				refs.aiRich.destination = targetDest;
			}
			else stateManager.SwitchState(refs.cowerState);
		}

		public void StateUpdate(PeepStateManager psm)
		{
			if (Vector3.Distance(transform.position, targetDest) <= stopDist)
				DestinationReached();
		}

		private void SetHideDest(GameObject[] points)
		{
			for (int i = 0; i < points.Length; i++)
			{
				Path path = refs.pathSeeker.StartPath(transform.position, points[i].transform.position);
				path.BlockUntilCalculated();

				if (!path.error)
				{
					targetTrans = points[i].transform;
					targetDest = targetTrans.position;
					return;
				}
				else
				{
					targetDest = new Vector3(float.PositiveInfinity, float.PositiveInfinity,
						float.PositiveInfinity);
					targetTrans = null;
				}
			}
		}

		private void SetAttackPos()
		{
			GraphNode node = AstarPath.active.GetNearest(stateManager.player.transform.position).node;
			nodePos = (Vector3)node.position;

			if (node.Walkable)
			{
				targetDest = nodePos;
				targetTrans = new GameObject().transform;
				targetTrans.position = targetDest;
			}
			else
			{
				Debug.Log("Nearest node to player not walkable for	" + this.gameObject.name);
				targetDest = new Vector3(float.PositiveInfinity, float.PositiveInfinity,
				float.PositiveInfinity);
				targetTrans = null;
			}
		}

		public void DestinationReached()
		{
			refs.aiRich.maxSpeed = 0;
			if (stateManager.coward) stateManager.SwitchState(refs.hideState);
			else if (stateManager.brave) Debug.Log("Switching to attack state");
			// TO DO: Create and hook up attack state
		}

		public void StateExit()
		{
			continuePrevMovement = false;
		}
	}
}
