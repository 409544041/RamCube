using MoreMountains.Feedbacks;
using Qbism.Saving;
using Qbism.ScreenStateMachine;
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
		public Color textColor, selectedTextColor, inactiveTextColor;
		[SerializeField] MMFeedbacks popInJuice, popOutJuice;
		[SerializeField] bool isSettingsOverlay;
		[SerializeField] float selectedButtonSize = 1;
		public GameplayCoreRefHolder gcRef;
		public MapCoreRefHolder mcRef;
		public SerpCoreRefHolder scRef;
		public SplashRefHolder splashRef;
		

		//Cache
		public PersistentRefHolder persRef { get; private set; }
		public ScreenStateManager screenStateMngr { get; private set;}

		//States
		public bool overlayActive { get; private set; }
		public OverlayButtonHandler selectedButtonHandler { get; private set; }
		OverlayButtonHandler prevButtonHandler;
		GaussianCanvas gausCanvas;
		public Slider musicSlider { get; private set; } public Slider sfxSlider { get; private set; }
		public OverlayButtonHandler displayButton { get; private set; } 
		private void Awake()
		{
			if (gcRef != null)
			{
				screenStateMngr = gcRef.glRef.screenStateMngr;
				persRef = gcRef.persRef; gausCanvas = gcRef.gausCanvas;
			}
			else if (mcRef != null)
			{
				screenStateMngr = mcRef.mlRef.screenStateMngr;
				persRef = mcRef.persRef; gausCanvas = mcRef.gausCanvas;
			}
			else if (scRef != null)
			{
				screenStateMngr = scRef.slRef.screenStateMngr;
				persRef = scRef.persRef; gausCanvas = scRef.gausCanvas;
			}
			else if (splashRef != null)
			{
				screenStateMngr = splashRef.screenStateMngr;
				persRef = splashRef?.persRef; gausCanvas = splashRef.gausCanvas;
			}

			LoadSettingsData();

			canvasGroup.alpha = 0;

			SetButtonsInteractable(false);

			foreach (var button in buttonHandlers)
			{
				button.menuHandler = this;

				if (gcRef != null)
				{
					button.mapLoader = gcRef.glRef.mapLoader;
					button.gcRef = gcRef;
				}
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
					screenStateMngr);
				if (prevButtonHandler != null) prevButtonHandler.DeselectButton(textColor);
			}
		}

		private void LoadSettingsData()
		{
			if (isSettingsOverlay)
			{
				foreach (var buttonHandler in buttonHandlers)
				{
					if (buttonHandler.label == "display") displayButton = buttonHandler;

					if (buttonHandler.slider == null) continue;
					if (buttonHandler.label == "musicVolume") musicSlider = buttonHandler.slider;
					if (buttonHandler.label == "sfxVolume") sfxSlider = buttonHandler.slider;
				}

				persRef.settingsSaveLoad.AssignLoadedSettingsValues(musicSlider, sfxSlider,
					displayButton);
			}
		}

		public void ShowOverlay()
		{
			if (gausCanvas != null) gausCanvas.SetUpGaussianCanvas();
			canvasGroup.alpha = 1;
			popInJuice.PlayFeedbacks();

			SetButtonsInteractable(true);


			if (gcRef != null) gcRef.pRef.playerMover.SetAllowInput(false);

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
			buttonHandlers[i].SelectButton(selectedTextColor, selectedButtonSize, screenStateMngr);

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
			if (gausCanvas != null) gausCanvas.TurnOffGaussianCanvas();
			popOutJuice.PlayFeedbacks();
			yield return new WaitForSeconds(dur);
			canvasGroup.alpha = 0;

			if (gcRef != null) gcRef.pRef.playerMover.SetAllowInput(true);

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
