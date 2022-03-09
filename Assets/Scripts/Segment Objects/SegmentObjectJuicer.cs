using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Objects
{
	public class SegmentObjectJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MMFeedbacks swallowAndFartOutJuice, scaleUpJuice;
		[SerializeField] float scaleUpOvershoot = .6f;

		//States
		bool swallowFartJuiceActivated = false;

		private void OnTriggerEnter(Collider other)
		{
			if (other.tag != "PlayerFace") return;
			if (swallowFartJuiceActivated) return;

			swallowAndFartOutJuice.PlayFeedbacks();
			swallowFartJuiceActivated = true;

			var objCollManager = FindObjectOfType<ObjectCollectManager>();
			objCollManager.objJuicer = this;
			objCollManager.InitiateShowingObjectOverlay();
		}

		public void TriggerScaleUpJuice()
		{
			var mmScaler = scaleUpJuice.GetComponent<MMFeedbackScale>();
			var startScale = mmScaler.AnimateScaleTarget.transform.localScale.x;
			mmScaler.RemapCurveZero = startScale;
			mmScaler.RemapCurveOne = startScale + scaleUpOvershoot;
			scaleUpJuice.Initialization();
			scaleUpJuice.PlayFeedbacks();
		}
	}
}
