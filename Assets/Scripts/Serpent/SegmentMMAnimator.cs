using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Qbism.Serpent
{
	public class SegmentMMAnimator : MonoBehaviour
	{
		//Config paramters
		[SerializeField] MMFeedbacks hopMovement = null;
		[SerializeField] float hopInterval = 1f;

		//Cache
		MMFeedbackRotation hopRot = null;

		//States
		bool firstHop = true;

		private void Awake() 
		{
			hopRot = hopMovement.GetComponent<MMFeedbackRotation>();
		}

		private IEnumerator Start() 
		{
			yield return new WaitForSeconds(2f);
			StartCoroutine(TriggerHopMovement());
		}

		private IEnumerator TriggerHopMovement()
		{
			hopMovement.Initialization();
			hopMovement.PlayFeedbacks();

			if (firstHop)
			{
				firstHop = false;
				//take the total time of the MMmovement into account for hopInterval duration
				yield return new WaitForSeconds(hopInterval);
				hopRot.RemapCurveOne *= -1;
				StartCoroutine(TriggerHopMovement());
			}
		}
	}
}
