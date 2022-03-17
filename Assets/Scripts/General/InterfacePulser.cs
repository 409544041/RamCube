using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Qbism.Cubes;
using Qbism.PlayerCube;
using Qbism.Rewind;
using UnityEngine;

namespace Qbism.General
{
	public class InterfacePulser : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MMFeedbacks pulser = null;

		public void InitiatePulse()
		{
			if (pulser.IsPlaying) return; 
			
			pulser.Initialization();
			pulser.PlayFeedbacks();
		}

		public void StopPulse()
		{
			pulser.StopFeedbacks();
		}
	}

}