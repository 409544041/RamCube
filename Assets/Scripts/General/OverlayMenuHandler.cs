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
		public OverlayButtonHandler[] buttonHandlers;
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
			SetStateManager(); //for overlays already in scene

			canvasGroup.alpha = 0;

			foreach (var buttonHandler in buttonHandlers)
			{
				buttonHandler.button.interactable = false;
			}
		}

		public void SetStateManager() //called from refs for overlay in persref
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

			foreach (var buttonHandler in buttonHandlers)
			{
				buttonHandler.button.interactable = true;
			}


			if (gcRef != null) gcRef.pRef.playerMover.input = false;
			if (mcRef != null)
			{
				foreach (var pin in mcRef.mlRef.levelPins)
				{
					if (pin.button.enabled == true)
						pin.button.interactable = false;
				}
			}
		}

		public void SelectButton(int i)
		{
			buttonHandlers[i].SelectButton(selectedTextColor, this, screenStateMngr);

			for (int j = 0; j < buttonHandlers.Length; j++)
			{
				if (j == i) continue;
				buttonHandlers[j].DeselectButton(textColor);
			}
		}

		public void SlideSlider(float slideValue)
		{
			var slider = selectedButtonHandler.slider;
			if (slider == null) return;

			slider.value += slideValue;

			if (slider.value < 0) slider.value = 0;
			if (slider.value > 1) slider.value = 1;
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
					if (pin.button.enabled == true)
						pin.button.interactable = true;
				}
			}
		}
	}
}
