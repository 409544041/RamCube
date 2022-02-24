using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Peep
{
	public class PeepHideState : MonoBehaviour, IPeepBaseState
	{
		//Config parameters
		[SerializeField] float turnToDiveDur = .25f;

		//Cache
		PeepStateManager stateManager;
		PeepRefHolder refs;

		//States
		Transform[] points;
		Transform divePoint = null;

		public void StateEnter(PeepStateManager psm)
		{
			if (stateManager == null)
			{
				stateManager = psm;
				refs = stateManager.refs;
			}

			var points = refs.runState.targetDest.GetComponentsInChildren<Transform>();
			foreach (var point in points)
			{
				if (point.tag == "DivePoint") divePoint = point;
			}

			StartCoroutine(TurnToDivePoint(divePoint));
		}

		public void StateUpdate(PeepStateManager psm)
		{
		}

		private IEnumerator TurnToDivePoint(Transform divePoint)
		{
			while (refs.agent.velocity.magnitude > .1f)
			{
				yield return null;
			}

			Vector3 diveDir = (divePoint.transform.position - transform.position).normalized;

			while (!V3Equal(transform.forward, diveDir))
			{
				var newDir = Vector3.RotateTowards(transform.forward, diveDir, 200 * Time.deltaTime, 0.0f);
				var newRot = Quaternion.LookRotation(newDir);
				transform.rotation = Quaternion.Slerp(transform.rotation, newRot, 4 * Time.deltaTime);

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
			// TO DO: MMFeedbacks trigger diving vfx 
		}

		private bool V3Equal(Vector3 a, Vector3 b)
		{
			return Vector3.SqrMagnitude(a - b) < 0.05;
		}

		public void StateExit()
		{
		}
	}
}
