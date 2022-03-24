using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Objects
{
	public class SegmentObjectCollHandler : MonoBehaviour
	{
		[SerializeField] MMFeedbacks swallowAndFartOutJuice;
		[SerializeField] SegObjectRefHolder refs;

		//States
		bool swallowFartJuiceActivated = false;

		private void OnTriggerEnter(Collider other)
		{
			if (other.tag != "PlayerFace") return;
			if (swallowFartJuiceActivated) return;

			swallowAndFartOutJuice.PlayFeedbacks();
			swallowFartJuiceActivated = true;

			var objCollManager = FindObjectOfType<ObjectCollectManager>();
			objCollManager.collectableObject = this.gameObject;
			objCollManager.objJuicer = refs.juicer;
			objCollManager.objMeshTrans = refs.meshTrans;
			objCollManager.m_Object = refs.m_objects;
			objCollManager.InitiateShowingObjectOverlay();
		}
	}
}
