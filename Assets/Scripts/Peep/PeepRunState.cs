using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Qbism.Environment;
using MoreMountains.Feedbacks;

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
		Transform targetDest;
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

		private void OnTriggerEnter(Collider other)
		{
			//if trigger = house || bush > shrinking juice to make it look like they shrank into the house
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
			var points = targetDest.GetComponentsInChildren<Transform>();
			Transform divePoint = null;

			foreach (var point in points)
			{
				if (point.tag == "DivePoint") divePoint = point;
			}

			StartCoroutine(TurnToDivePoint(divePoint));
			// MMFeedbacks trigger diving vfx 
			// Switch state to hidden state
		}

		private IEnumerator TurnToDivePoint(Transform divePoint)
		{
			Vector3 diveDir = (divePoint.transform.position - transform.position).normalized;

			while (!V3Equal(transform.forward, diveDir))
			{
				var newDir = Vector3.RotateTowards(transform.forward, diveDir, 200 * Time.deltaTime, 0.0f);
				var newRot = Quaternion.LookRotation(newDir);
				transform.rotation = Quaternion.Slerp(transform.rotation, newRot, 2 * Time.deltaTime);

				yield return null;
			}

			InitiateHidingJuice(divePoint);
		}

		private void InitiateHidingJuice(Transform divePoint)
		{
			var mmPos = refs.hideJuice.GetComponents<MMFeedbackPosition>();
			MMFeedbackPosition mmPosToDest = null;

			foreach (var mm in mmPos)
			{
				if (mm.Label == "PositionToDest") mmPosToDest = mm;
			}
			
			refs.agent.enabled = false;

			mmPosToDest.DestinationPositionTransform = divePoint;
			refs.hideJuice.Initialization();
			refs.hideJuice.PlayFeedbacks();
		}

		private bool V3Equal(Vector3 a, Vector3 b)
		{
			return Vector3.SqrMagnitude(a - b) < 0.1;
		}
	}
}
