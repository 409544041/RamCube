using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Qbism.General
{
	public class RewindJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MMFeedbacks postRewindJuice = null;

		//Activated by SendMessage from timebody. If method name changes, must change it there too.
		public void StartPostRewindJuice()
		{
			postRewindJuice.Initialization();
			postRewindJuice.PlayFeedbacks();
		}
	}
}
