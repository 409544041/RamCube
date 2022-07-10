using Qbism.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Rewind
{
	public class GreyOutOnRewind : MonoBehaviour
	{
		//Config parameters
		[SerializeField] GreyOutButtons greyOutButton;
		[SerializeField] GameplayCoreRefHolder gcRef;

		//States
		bool rewindAllowed, prevFrameRewindAllowed;

		private void Update()
		{
			prevFrameRewindAllowed = rewindAllowed;
			rewindAllowed = gcRef.pRef.playerMover.allowRewind;

			if (!rewindAllowed && rewindAllowed != prevFrameRewindAllowed)
				greyOutButton.GrayOutButton();
			if (rewindAllowed && rewindAllowed != prevFrameRewindAllowed)
				greyOutButton.ReturnToOriginalColors();
		}
	}
}
