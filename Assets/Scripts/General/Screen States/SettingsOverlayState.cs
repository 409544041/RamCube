using Qbism.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class SettingsOverlayState : MonoBehaviour, IScreenBaseState
	{
		//Cache
		ScreenStateManager stateMngr;
		OverlayMenuHandler menuHandler;

		public void StateEnter(ScreenStateManager ssm)
		{
			if (stateMngr == null)
			{
				stateMngr = ssm;
				if (stateMngr.gcRef != null) menuHandler = stateMngr.gcRef.settingsOverlayHandler;
				if (stateMngr.mcRef != null) menuHandler = stateMngr.mcRef.settingsOverlayHandler;
				if (stateMngr.scRef != null) menuHandler = stateMngr.scRef.settingsOverlayHandler;
			}

			menuHandler.SelectButton(0);
			menuHandler.ShowOverlay();
			//freeze rest of game?
		}

		public void HandleEscapeInput()
		{
			stateMngr.SwitchState(stateMngr.prevScreenState, stateMngr.prevStateEnum);
		}

		public void HandleStickValues(Vector2 stickValue, InputDetector inputDetector)
		{
			if (stickValue.x > .5)
			{
				menuHandler.selectedButtonHandler.SlideSlider(1);
			}
			else if (stickValue.x < -.5)
			{
				menuHandler.selectedButtonHandler.SlideSlider(-1);
			}
		}

		public void StateExit()
		{
			menuHandler.InitiateHideOverlay();
			stateMngr.persRef.settingsSaveLoad.SaveSettingsValues(menuHandler.musicSlider,
				menuHandler.sfxSlider);
		}

		public void HandleActionInput()
		{
		}
		public void HandleAnyInput()
		{
		}
		public void HandleDebugCompleteAllInput()
		{
		}
		public void HandleDebugCompleteInput()
		{
		}
		public void HandleDebugDeleteProgressInput()
		{
		}
		public void HandleDebugUnlockAllInput()
		{
		}
		public void HandleResetInput()
		{
		}
		public void HandleRewindInput()
		{
		}
		public void HandleBackInput()
		{
		}
	}
}
