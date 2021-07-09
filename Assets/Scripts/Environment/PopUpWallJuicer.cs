using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Qbism.Environment
{
	public class PopUpWallJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MMFeedbacks popUpJuice;

		public void TriggerPopUpJuice()
		{
			popUpJuice.Initialization();
			popUpJuice.PlayFeedbacks();
		}
	}
}
