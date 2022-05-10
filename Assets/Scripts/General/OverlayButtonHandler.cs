using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

namespace Qbism.General
{
	public class OverlayButtonHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] TextMeshProUGUI buttonText, valueText;
		public Button button;

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
		}

		public void DeselectButton(Color defaultColor)
		{
			buttonText.color = defaultColor;
			if (valueText != null) valueText.color = defaultColor;
		}

		public OverlayButtonHandler FetchButtonHandler(GameObject selectedGameObject)
		{
			if (selectedGameObject == this.gameObject) return this;
			else return null;
		}

		public void PressResumeButton() //Called from button event
		{
			menuHandler.InitiateHideOverlay();
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
			Application.Quit();
		}
	}
}
