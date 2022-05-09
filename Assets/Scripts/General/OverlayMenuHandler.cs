using MoreMountains.Feedbacks;
using Qbism.WorldMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Qbism.General
{
	public class OverlayMenuHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] CanvasGroup canvasGroup;
		[SerializeField] OverlayButtonHandler[] buttonHandlers;
		[SerializeField] Color textColor, selectedTextColor;
		[SerializeField] MMFeedbacks popInJuice, popOutJuice;
		[SerializeField] GameplayCoreRefHolder gcRef;
		[SerializeField] MapCoreRefHolder mcRef;

		//States
		public bool overlayActive { get; private set; }
		OverlayButtonHandler selectedButtonHandler, prevButtonHandler;
		OverlayButtons selectedButtonType;
		

		private void Update()
		{
			overlayActive = canvasGroup.alpha == 1;

			prevButtonHandler = selectedButtonHandler;
			GameObject selected = EventSystem.current.currentSelectedGameObject;

			for (int i = 0; i < buttonHandlers.Length; i++)
			{
				var buttonHandler = buttonHandlers[i].FetchButtonHandler(selected); //Sets new selected button
				if (buttonHandler == null) continue;

				selectedButtonHandler = buttonHandler;
				break;
			}

			if (selectedButtonHandler != prevButtonHandler)
			{
				selectedButtonType = selectedButtonHandler.SelectButton(selectedTextColor);
				if (prevButtonHandler != null) prevButtonHandler.DeselectButton(textColor);
			}
		}

		public void ShowOverlay()
		{
			canvasGroup.alpha = 1;
			popInJuice.PlayFeedbacks();
			selectedButtonType = buttonHandlers[0].SelectButton(selectedTextColor);


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

		public void PressSelectedButton()
		{
			if (selectedButtonType == OverlayButtons.resume) 
				StartCoroutine(HideOverlay());
			else if (selectedButtonType == OverlayButtons.restartLevel) 
				gcRef.glRef.sceneHandler.RestartLevel();
			else if (selectedButtonType == OverlayButtons.levelSelect)
				gcRef.glRef.mapLoader.StartLoadingWorldMap(true);
			//else if (selectedButtonType == OverlayButtons.settings)
			else if (selectedButtonType == OverlayButtons.quitGame)
				Application.Quit();
		}

	}
}
