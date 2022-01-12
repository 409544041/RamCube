using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Qbism.Environment
{
	public class PopUpWallJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MMFeedbacks popUpJuice, burrowJuice;
		[SerializeField] AudioClip upClip, downClip;
		[SerializeField] AudioSource source;

		//States
		bool burroweActivated;

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

		public void Burrow()
		{
			//To avoid walls with multiple wallTagTransforms to call this multiple times
			if (burroweActivated) return;

			burrowJuice.Initialization();
			burrowJuice.PlayFeedbacks();

			burroweActivated = true;
		}
	}
}
