using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class FFJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MMFeedbacks juice;

		public void TriggerJuice()
		{
			juice.Initialization();
			juice.PlayFeedbacks();
		}
	}
}
