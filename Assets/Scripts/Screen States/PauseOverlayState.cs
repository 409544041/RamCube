using Qbism.Control;
using Qbism.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.ScreenStateMachine
{
	public class PauseOverlayState : MonoBehaviour, IScreenBaseState
	{
		//Cache
		ScreenStateManager stateMngr;
		OverlayMenuHandler menuHandler;

		//States
		CanvasGroup canvasToTurnOff;

		public void StateEnter(ScreenStateManager ssm)
		{
			if (stateMngr == null)
			{
				stateMngr = ssm;
				if (stateMngr.gcRef != null)
				{
					menuHandler = stateMngr.gcRef.pauseOverlayHandler;
					canvasToTurnOff = stateMngr.gcRef.gameplayCanvasGroup;
				}
				if (stateMngr.mcRef != null)
				{
					menuHandler = stateMngr.mcRef.pauseOverlayHandler;
					canvasToTurnOff = stateMngr.mcRef.mapCanvasGroup;
				}
				if (stateMngr.scRef != null)
				{
					menuHandler = stateMngr.scRef.pauseOverlayHandler;
					canvasToTurnOff = stateMngr.scRef.serpScreenCanvasGroup;
				}
			}

			SelectCorrectButton();
			menuHandler.ShowOverlay();
			canvasToTurnOff.alpha = 0;
			//freeze rest of game?
		}

		private void SelectCorrectButton()
		{
			if (stateMngr.prevStateEnum != ScreenStates.settingsOverlayState)
				menuHandler.SelectButton(0);
			else
			{
				for (int i = 0; i < menuHandler.buttonHandlers.Length; i++)
				{
					var button = menuHandler.buttonHandlers[i];
					if (button.label != "settings") continue;
					menuHandler.SelectButton(i);
				}
			}
		}

		public void HandleEscapeInput()
		{
			if (stateMngr.gcRef != null) stateMngr.SwitchState(stateMngr.levelScreenState,
				ScreenStates.levelScreenState);
			if (stateMngr.mcRef != null)
			{
				stateMngr.SwitchState(stateMngr.mapScreenState, ScreenStates.mapScreenState);
				stateMngr.mcRef.mlRef.pinTracker.selectedPin.pinUI.SelectPinUI();
			}
			if (stateMngr.scRef != null) stateMngr.SwitchState(stateMngr.serpentScreenState,
				ScreenStates.serpentScreenState);

			canvasToTurnOff.alpha = 1;
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
		public void HandleBackInput()
		{
		}
		public void HandleDebugToggleHudInput()
		{
		}
	}
}
