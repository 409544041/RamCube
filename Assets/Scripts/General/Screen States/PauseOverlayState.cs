using Qbism.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class PauseOverlayState : MonoBehaviour, IScreenBaseState
	{
		//Cache
		ScreenStateManager stateMngr;
		OverlayMenuHandler menuHandler;

		public void StateEnter(ScreenStateManager ssm)
		{
			if (stateMngr == null)
			{
				stateMngr = ssm;
				if (stateMngr.gcRef != null) menuHandler = stateMngr.gcRef.pauseOverlayHandler;
				if (stateMngr.mcRef != null) menuHandler = stateMngr.mcRef.pauzeOverlayHandler;
			}

			menuHandler.SelectTopMostButton();
			menuHandler.ShowOverlay();
			//freeze rest of game?
		}

		public void HandleActionInput()
		{
			menuHandler.PressSelectedButton();
		}

		public void HandleEscapeInput()
		{
			if (stateMngr.gcRef != null) stateMngr.SwitchState(stateMngr.levelScreenState); 
			if (stateMngr.mcRef != null) stateMngr.SwitchState(stateMngr.mapScreenState);
		}

		public void StateExit()
		{
			menuHandler.InitiateHideOverlay();
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
