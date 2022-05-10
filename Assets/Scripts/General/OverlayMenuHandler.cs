using MoreMountains.Feedbacks;
using Qbism.Saving;
using Qbism.SceneTransition;
using Qbism.Serpent;
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
		public GameplayCoreRefHolder gcRef;
		public MapCoreRefHolder mcRef;
		public SerpCoreRefHolder scRef;

		//Cache
		public ScreenStateManager screenStateMngr;

		//States
		public bool overlayActive { get; private set; }
		OverlayButtonHandler selectedButtonHandler, prevButtonHandler;

		private void Awake()
		{
			if (gcRef != null) screenStateMngr = gcRef.glRef.screenStateMngr;
			if (mcRef != null) screenStateMngr = mcRef.mlRef.screenStateMngr;
			if (scRef != null) screenStateMngr = scRef.slRef.screenStateMngr;
		}

		public void FixLinks()
		{
			if (gcRef != null) screenStateMngr = gcRef.glRef.screenStateMngr;
			if (mcRef != null) screenStateMngr = mcRef.mlRef.screenStateMngr;
			if (scRef != null) screenStateMngr = scRef.slRef.screenStateMngr;
		}

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
				selectedButtonHandler.SelectButton(selectedTextColor, this, screenStateMngr);
				if (prevButtonHandler != null) prevButtonHandler.DeselectButton(textColor);
			}
		}

		public void ShowOverlay()
		{
			canvasGroup.alpha = 1;
			popInJuice.PlayFeedbacks();
			buttonHandlers[0].SelectButton(selectedTextColor, this, screenStateMngr);

			foreach (var buttonHandler in buttonHandlers)
			{
				buttonHandler.button.interactable = true;
			}


			if (gcRef != null) gcRef.pRef.playerMover.input = false;
			if (mcRef != null)
			{
				foreach (var pin in mcRef.mlRef.levelPins)
				{
					pin.button.enabled = false;
				}
			}
		}

		public void SelectTopMostButton()
		{
			buttonHandlers[0].SelectButton(selectedTextColor, this, screenStateMngr);
			for (int i = 1; i < buttonHandlers.Length; i++)
			{
				buttonHandlers[i].DeselectButton(textColor);
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

			foreach (var buttonHandler in buttonHandlers)
			{
				buttonHandler.button.interactable = false;
			}

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
