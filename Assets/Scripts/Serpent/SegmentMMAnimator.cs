using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Qbism.Serpent
{
	public class SegmentMMAnimator : MonoBehaviour
	{
		//Config paramters
		[Header ("Hop Look Around")]
		[SerializeField] MMFeedbacks hopAnim = null;
		[SerializeField] float hopInterval = 1f;
		[Header ("Landing Squish")]
		[SerializeField] MMFeedbacks squishAnim = null;

		//Cache
		MMFeedbackRotation hopRot = null;
		MMFeedbackPosition hopPos = null;

		//States
		bool firstHop = true;

		private void Awake() 
		{
			hopRot = hopAnim.GetComponent<MMFeedbackRotation>();
			hopPos = hopAnim.GetComponent<MMFeedbackPosition>();
		}

		private IEnumerator TriggerHopAnim()
		{
			hopAnim.Initialization();
			hopAnim.PlayFeedbacks();

			if (firstHop)
			{
				firstHop = false;
				//take the total time of the MMmovement into account for hopInterval duration
				yield return new WaitForSeconds(hopInterval);
				hopRot.RemapCurveOne *= -1;
				StartCoroutine(TriggerHopAnim());
			}
		}

		private void TriggerSquishAnim()
		{
			squishAnim.Initialization();
			squishAnim.PlayFeedbacks();
		}
	}
}
