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

		public void WiggleFace(int repeats)
		{
			var mmPos = faceJuice.GetComponent<MMFeedbackPosition>();
			mmPos.Timing.NumberOfRepeats = repeats;
		
			faceJuice.Initialization();
			faceJuice.PlayFeedbacks();
		}
	}
}
