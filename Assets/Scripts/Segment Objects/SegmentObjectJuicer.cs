using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Objects
{
	public class SegmentObjectJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MMFeedbacks scaleUpJuice, starScaleJuice, onlyRotationJuice;
		[SerializeField] float scaleUpOvershoot = .6f;
		public ParticleSystem fartParticles;
		public Canvas uiStar;

		public void TriggerScaleUpJuice()
		{
			var mmScaler = scaleUpJuice.GetComponent<MMFeedbackScale>();
			var startScale = mmScaler.AnimateScaleTarget.transform.localScale.x;
			mmScaler.RemapCurveZero = startScale;
			mmScaler.RemapCurveOne = startScale + scaleUpOvershoot;
			scaleUpJuice.Initialization();
			scaleUpJuice.PlayFeedbacks();
		}

		public void TriggerVFX()
		{
			starScaleJuice.Initialization();
			starScaleJuice.PlayFeedbacks();
			fartParticles.Play();
		}

		public void TriggerOnlyRotation()
		{
			onlyRotationJuice.Initialization();
			onlyRotationJuice.PlayFeedbacks();
		}
	}
}
