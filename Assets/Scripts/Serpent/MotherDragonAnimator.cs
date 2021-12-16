using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Qbism.Serpent
{
	public class MotherDragonAnimator : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MMFeedbacks flybyJuice;

		public void ActivateFlyByJuice()
		{
			flybyJuice.Initialization();
			flybyJuice.PlayFeedbacks();
		}
	}
}
