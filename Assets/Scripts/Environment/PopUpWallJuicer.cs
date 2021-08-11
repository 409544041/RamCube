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
		[SerializeField] AudioClip upClip, downClip;
		[SerializeField] AudioSource source;

		public void TriggerPopUpJuice()
		{
			popUpJuice.Initialization();
			popUpJuice.PlayFeedbacks();
		}

		public void PlayUpSFX()
		{
			source.PlayOneShot(upClip);
		}

		public void PlayDownSFX()
		{
			source.PlayOneShot(downClip);
		}
	}
}
