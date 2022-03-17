using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Qbism.SpriteAnimations
{
	public class FaceJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MMFeedbacks faceJuice;

		public void WiggleFace()
		{
			faceJuice.Initialization();
			faceJuice.PlayFeedbacks();
		}
	}
}
