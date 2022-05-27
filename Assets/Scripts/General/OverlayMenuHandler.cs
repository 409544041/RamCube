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
		[SerializeField] bool isSettingsOverlay;
		[SerializeField] float selectedButtonSize = 1;
		public GameplayCoreRefHolder gcRef;
		public MapCoreRefHolder mcRef;
		public SerpCoreRefHolder scRef;

		//Cache
		public ScreenStateManager screenStateMngr;
		PersistentRefHolder persRef;

		//States
		public bool overlayActive { get; private set; }
		public OverlayButtonHandler selectedButtonHandler { get; private set; }
		OverlayButtonHandler prevButtonHandler;
		public Slider musicSlider { get; private set; }  public Slider sfxSlider { get; private set; }

		private void Awake()
		{
			if (gcRef != null)
			{
				screenStateMngr = gcRef.glRef.screenStateMngr;
				persRef = gcRef.persRef;
			}
			if (mcRef != null)
			{
				screenStateMngr = mcRef.mlRef.screenStateMngr;
				persRef = mcRef.persRef;
			}
			if (scRef != null)
			{
				screenStateMngr = scRef.slRef.screenStateMngr;
				persRef = scRef.persRef;
			}

			LoadSettingsData();

			canvasGroup.alpha = 0;

			SetButtonsInteractable(false);
		}

		private void LoadSettingsData()
		{
			if (isSettingsOverlay)
			{
				foreach (var buttonHandler in buttonHandlers)
				{
					if (buttonHandler.slider == null) continue;

					if (buttonHandler.label == "musicVolume") musicSlider = buttonHandler.slider;
					if (buttonHandler.label == "sfxVolume") sfxSlider = buttonHandler.slider;
				}

				persRef.settingsSaveLoad.AssignLoadedSettingsValues(musicSlider, sfxSlider);
			}
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
				selectedButtonHandler.SelectButton(selectedTextColor, selectedButtonSize,
					this, null, screenStateMngr);
				if (prevButtonHandler != null) prevButtonHandler.DeselectButton(textColor);
			}
		}

		public void ShowOverlay()
		{
			canvasGroup.alpha = 1;
			popInJuice.PlayFeedbacks();

			SetButtonsInteractable(true);


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
			buttonHandlers[i].SelectButton(selectedTextColor, selectedButtonSize,
				this, null, screenStateMngr);

			for (int j = 0; j < buttonHandlers.Length; j++)
			{
				if (j == i) continue;
				buttonHandlers[j].DeselectButton(textColor);
			}
		}

		private void SetButtonsInteractable(bool value)
		{
			foreach (var buttonHandler in buttonHandlers)
			{
				buttonHandler.button.interactable = value;
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

			SetButtonsInteractable(false);

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
