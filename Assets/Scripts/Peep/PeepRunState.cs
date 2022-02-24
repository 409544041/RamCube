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

		public void StateEnter(PeepStateManager psm)
		{
			if (stateManager == null) stateManager = psm;
			refs = stateManager.refs;
			// TO DO: trigger startled anim 

			var pointMngr = stateManager.pointManager;
			var hidePoints = pointMngr.SortHidePointsByDistance(pointMngr.hidePoints);
			StartCoroutine(SetNavTarget(hidePoints));
		}

		public void StateUpdate(PeepStateManager psm)
		{
			if (isMoving) refs.peepMover.MoveWithSmoothRotation(runSpeed);
		}

		private IEnumerator SetNavTarget(GameObject[] points)
		{
			bool pathFound = false;

			for (int i = 0; i < points.Length; i++)
			{
				var path = new NavMeshPath();
				//var pointChecker = points[i].GetComponentInParent<FloraSpawnChecker>();

				//pointChecker.navMeshOb.carving = false;
				yield return null; //this to ensure the carving is actually turned off before the next bit

				if (refs.agent.CalculatePath(points[i].transform.position, path) 
					&& pathFound == false)
				{
					refs.agent.speed = runSpeed;
					targetDest = points[i].transform;
					refs.agent.destination = targetDest.position;
					//pointChecker.coll.enabled = true;
					pathFound = true;
					continue;
				}

				// disables the hide collider from other hidepoints that isn't the path hidepoint
				//pointChecker.coll.enabled = false;
				//pointChecker.navMeshOb.carving = true;
			}

			refs.peepMover.PrepareMove(targetDest, this);
			isMoving = true;
		}

		public void DestinationReached()
		{
			isMoving = false;
			stateManager.SwitchState(refs.hideState);
		}
	}
}
