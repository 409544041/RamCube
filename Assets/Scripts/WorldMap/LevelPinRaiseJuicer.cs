using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class LevelPinRaiseJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MMFeedbacks raiseJuice;
		[SerializeField] ParticleSystem raisedDustFX;

		public void PlayRaiseJuice()
		{
			raiseJuice.Initialization();
			raiseJuice.PlayFeedbacks();
		}

		public void StopRaiseJuice()
		{
			raiseJuice.StopFeedbacks();
			raisedDustFX.Play();
		}
	}
}
