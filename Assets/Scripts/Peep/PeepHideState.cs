using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Peep
{
	public class PeepHideState : MonoBehaviour, IPeepBaseState
	{
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

			var points = refs.runState.targetTrans.GetComponentsInChildren<Transform>();
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
			while (refs.aiRich.velocity.magnitude > .1f)
			{
				yield return null; //ensure turning doesn't start when still moving
			}

			var diveDir = (divePoint.transform.position - transform.position).normalized;
			var diveDirV2 = new Vector2(diveDir.x, diveDir.z);

			while (FetchAngle(diveDirV2) > 10)
			{
				var newDir = Vector3.RotateTowards(refs.transform.forward, diveDir, 
					200 * Time.deltaTime, 0.0f);

				var newRot = Quaternion.LookRotation(newDir);

				refs.transform.rotation = Quaternion.Slerp(refs.transform.rotation, 
					newRot, 4 * Time.deltaTime);

				yield return null;
			}

			InitiateHidingJuice(divePoint);
		}

		private float FetchAngle(Vector2 diveDirV2)
		{
			var dirV2 = new Vector2(refs.transform.forward.x, refs.transform.forward.z);
			var angle = Vector2.Angle(diveDirV2, dirV2);
			return angle;
		}

		private void InitiateHidingJuice(Transform divePoint)
		{
			var mmPos = refs.hideJuice.GetComponents<MMFeedbackPosition>();
			MMFeedbackPosition mmPosToDest = null;

			foreach (var mm in mmPos)
			{
				if (mm.Label == "PositionToDest") mmPosToDest = mm;
			}

			refs.aiRich.enabled = false;

			refs.peepAnim.TriggerAnim("Dive");

			mmPosToDest.DestinationPositionTransform = divePoint;
			refs.hideJuice.Initialization();
			refs.hideJuice.PlayFeedbacks();
		}

		public void StateExit()
		{
		}
	}
}
