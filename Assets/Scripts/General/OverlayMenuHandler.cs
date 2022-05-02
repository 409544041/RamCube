using MoreMountains.Feedbacks;
using Qbism.WorldMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class OverlayMenuHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] CanvasGroup canvasGroup;
		[SerializeField] MMFeedbacks popInJuice, popOutJuice;
		[SerializeField] GameplayCoreRefHolder gcRef;
		[SerializeField] MapCoreRefHolder mcRef;

		//States
		public bool overlayActive { get; private set; }

		private void Update()
		{
			overlayActive = canvasGroup.alpha == 1;
		}

		public void ShowOverlay()
		{
			canvasGroup.alpha = 1;
			popInJuice.PlayFeedbacks();

			if (gcRef != null) gcRef.pRef.playerMover.input = false;
			if (mcRef != null)
			{
				foreach (var pin in mcRef.mlRef.levelPins)
				{
					pin.button.enabled = false;
				}
			}
		}

		public void InitiateHideOverlay()
		{
			StartCoroutine(HideOverlay());
		}

		private IEnumerator HideOverlay()
		{
			MMFeedbackScale mmScale = popOutJuice.GetComponent<MMFeedbackScale>();
			var dur = mmScale.FeedbackDuration;

			popOutJuice.PlayFeedbacks();
			yield return new WaitForSeconds(dur);
			canvasGroup.alpha = 0;

			if (gcRef != null) gcRef.pRef.playerMover.input = true;
			if (mcRef != null)
			{
				foreach (var pin in mcRef.mlRef.levelPins)
				{
					pin.button.enabled = true;
				}
			}
		}

	}
}
