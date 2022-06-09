using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using Qbism.Saving;
using UnityEngine.Analytics;
using MoreMountains.Feedbacks;
using Qbism.ScreenStateMachine;

namespace Qbism.General
{
	public class OverlayButtonHandler : MonoBehaviour
	{
		//Config parameters
		public TextMeshProUGUI buttonText, valueText;
		public Button button;
		public Slider slider;
		[SerializeField] ButtonScroller scroller;
		[SerializeField] float sliderJump = .1f;
		[SerializeField] Image[] arrows;
		public string label;
		[SerializeField] MMFeedbacks buttonSelectJuice, buttonPressJuice;

		//Cache
		OverlayMenuHandler menuHandler;
		SplashMenuHandler splashMenuHandler;
		ScreenStateManager screenStateMngr;

		//States
		Vector3 buttonOriginalSize;

		private void Awake()
		{
			buttonOriginalSize = transform.localScale;
		}

		public void SelectButton(Color selectedColor, float selectSize, OverlayMenuHandler handler, 
			SplashMenuHandler splashHandler, ScreenStateManager stateMngr)
		{
			if (menuHandler == null)
			{
				menuHandler = handler;
				splashMenuHandler = splashHandler;
				screenStateMngr = stateMngr;
			}

			button.Select();
			buttonText.color = selectedColor;
			transform.localScale = Vector3.one * selectSize;
			if (valueText != null) valueText.color = selectedColor;
			ShowHideArrows(true);
			if (buttonSelectJuice != null) buttonSelectJuice.PlayFeedbacks();
		}

		public void DeselectButton(Color defaultColor)
		{
			buttonText.color = defaultColor;
			if (valueText != null) valueText.color = defaultColor;
			ShowHideArrows(false);
			transform.localScale = buttonOriginalSize;
		}

		private void ShowHideArrows(bool value)
		{
			if (arrows != null)
			{
				foreach (var arrow in arrows)
				{
					arrow.enabled = value;
				}
			}
		}

		public OverlayButtonHandler FetchButtonHandler(GameObject selectedGameObject)
		{
			if (selectedGameObject == this.gameObject) return this;
			else return null;
		}

		public void SlideSlider(int slideValue)
		{
			var value = slideValue * sliderJump;
			if (slider == null) return;

			slider.value += value;

			if (slider.value < slider.minValue) slider.value = slider.minValue;
			if (slider.value > slider.maxValue) slider.value = slider.maxValue;
		}

		public void ScrollButton(int scrollValue)
		{
			if (scroller == null) return;
			scroller.Scroll(scrollValue);
		}

		public void PressResumeButton() //Called from button event
		{
			if (buttonPressJuice != null) buttonPressJuice.PlayFeedbacks();

			screenStateMngr.currentScreenState.HandleEscapeInput();
		}

		public void PressRestartButton() //Called from button event
		{
			if (buttonPressJuice != null) buttonPressJuice.PlayFeedbacks();

			menuHandler.gcRef.glRef.sceneHandler.RestartLevel();
		}

		public void PressLevelSelectButton() //Called from button event
		{
			if (buttonPressJuice != null) buttonPressJuice.PlayFeedbacks();

			menuHandler.gcRef.glRef.mapLoader.StartLoadingWorldMap(true);
		}

		public void PressSettingsButton() //Called from button event
		{
			if (buttonPressJuice != null) buttonPressJuice.PlayFeedbacks();

			screenStateMngr.SwitchState(menuHandler.screenStateMngr.settingsOverlayState,
				ScreenStates.settingsOverlayState);
		}

		public void PressBackButton() //Called from button event
		{
			if (buttonPressJuice != null) buttonPressJuice.PlayFeedbacks();

			if (screenStateMngr.currentStateEnum == ScreenStates.settingsOverlayState)
				screenStateMngr.SwitchState(screenStateMngr.prevScreenState, screenStateMngr.prevStateEnum);
		}

		public void PressQuitButton() //Called from button event
		{
			if (buttonPressJuice != null) buttonPressJuice.PlayFeedbacks();
			button.interactable = false;

			SendSessionTimeAnalyticsEvent();
			SendLevelExitAnalyticsEvent();
			Application.Quit();
		}

		public void PressPlayButton() //Called from button event
		{
			if (buttonPressJuice != null) buttonPressJuice.PlayFeedbacks();

			splashMenuHandler.splashLoader.StartSceneTransition();
			button.interactable = false;
		}

		private static void SendSessionTimeAnalyticsEvent()
		{
			var persRef = FindObjectOfType<PersistentRefHolder>();
			persRef.sessionTimer.StopCountingTimer();

			Analytics.CustomEvent(AnalyticsEvents.SessionTime, new Dictionary<string, object>
			{
				{AnalyticsEvents.TimeWindow, persRef.sessionTimer.timeWindow}
			});
		}

		private void SendLevelExitAnalyticsEvent()
		{
			var gcRef = FindObjectOfType<GameplayCoreRefHolder>();
			if (gcRef != null)
			{
				Analytics.CustomEvent(AnalyticsEvents.LevelExit, new Dictionary<string, object>()
				{
					{AnalyticsEvents.Level, gcRef.persRef.progHandler.debugCurrentPin},
				});
			}

		}
	}
}
