using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Objects
{
	public class SegmentObjectJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MMFeedbacks swallowAndFartOutJuice, scaleUpJuice, starScaleJuice;
		[SerializeField] float scaleUpOvershoot = .6f;
		public Canvas uiStar;

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

		public void TriggerStarScaleJuice()
		{
			starScaleJuice.Initialization();
			starScaleJuice.PlayFeedbacks();
		}
	}
}
