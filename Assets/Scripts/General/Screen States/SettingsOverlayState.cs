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
				if (stateMngr.gcRef != null) menuHandler = stateMngr.gcRef.persRef.settingsOverlayHandler;
				if (stateMngr.mcRef != null) menuHandler = stateMngr.mcRef.persRef.settingsOverlayHandler;
				if (stateMngr.scRef != null) menuHandler = stateMngr.scRef.persRef.settingsOverlayHandler;
			}

			menuHandler.SelectTopMostButton();
			menuHandler.ShowOverlay();
			//freeze rest of game?
		}

		public void HandleEscapeInput()
		{
			if (stateMngr.gcRef != null) stateMngr.SwitchState(stateMngr.prevScreenState,
				stateMngr.prevStateEnum);
		}

		public void StateExit()
		{
			menuHandler.InitiateHideOverlay();
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
		public void HandleStickValues(Vector2 stickValue, InputDetector inputDetector)
		{
		}
	}
}
