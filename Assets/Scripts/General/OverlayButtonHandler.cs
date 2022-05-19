using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using Qbism.Saving;
using UnityEngine.Analytics;

namespace Qbism.General
{
	public class OverlayButtonHandler : MonoBehaviour
	{
		//Config parameters
		public TextMeshProUGUI buttonText, valueText;
		public Button button;
		public Slider slider;
		[SerializeField] float sliderJump = .1f;
		[SerializeField] Image[] arrows;
		public string label;

		//Cache
		OverlayMenuHandler menuHandler;
		ScreenStateManager screenStateMngr;

		public void SelectButton(Color selectedColor, OverlayMenuHandler handler,
			ScreenStateManager stateMngr)
		{
			if (menuHandler == null)
			{
				menuHandler = handler;
				screenStateMngr = stateMngr;
			}

			button.Select();
			buttonText.color = selectedColor;
			if (valueText != null) valueText.color = selectedColor;
			ShowHideArrows(true);
		}

		public void DeselectButton(Color defaultColor)
		{
			buttonText.color = defaultColor;
			if (valueText != null) valueText.color = defaultColor;
			ShowHideArrows(false);
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

		public void PressResumeButton() //Called from button event
		{
			screenStateMngr.currentScreenState.HandleEscapeInput();
		}

		public void PressRestartButton() //Called from button event
		{
			menuHandler.gcRef.glRef.sceneHandler.RestartLevel();
		}

		public void PressLevelSelectButton() //Called from button event
		{
			menuHandler.gcRef.glRef.mapLoader.StartLoadingWorldMap(true);
		}

		public void PressSettingsButton() //Called from button event
		{
			screenStateMngr.SwitchState(menuHandler.screenStateMngr.settingsOverlayState,
				ScreenStates.settingsOverlayState);
		}

		public void PressBackButton() //Called from button event
		{
			if (screenStateMngr.currentStateEnum == ScreenStates.settingsOverlayState)
				screenStateMngr.SwitchState(screenStateMngr.prevScreenState, screenStateMngr.prevStateEnum);
		}

		public void PressQuitButton() //Called from button event
		{
			SendSessionTimeAnalyticsEvent();
			SendLevelExitAnalyticsEvent();
			Application.Quit();
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
