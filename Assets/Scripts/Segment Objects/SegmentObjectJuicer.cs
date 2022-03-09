using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Objects
{
	public class SegmentObjectJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MMFeedbacks swallowAndFartOutJuice;

		//States
		bool swallowFartJuiceActivated = false;

		private void OnTriggerEnter(Collider other)
		{
			if (other.tag != "PlayerFace") return;
			if (swallowFartJuiceActivated) return;

			swallowAndFartOutJuice.PlayFeedbacks();
			swallowFartJuiceActivated = true;
		}
	}
}
