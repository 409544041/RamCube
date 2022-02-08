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
		public float burrowTime = .3f; //Should find a way to get total dur of a feedback
		[SerializeField] WallRefHolder wallRef;

		//States
		bool burrowActivated;

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
			if (burrowActivated) return;

			burrowJuice.Initialization();
			burrowJuice.PlayFeedbacks();

			if (wallRef.expressHandler != null)
				wallRef.expressHandler.SetFace(Expressions.toothyLaugh, -1);

			burrowActivated = true;
			wallRef.navMeshOb.enabled = false;
		}
	}
}
