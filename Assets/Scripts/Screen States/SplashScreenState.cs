using Qbism.Control;
using Qbism.General;
using Qbism.SceneTransition;
using Qbism.WorldMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Qbism.ScreenStateMachine
{
	public class SplashScreenState : MonoBehaviour, IScreenBaseState
	{
		//Cache
		ScreenStateManager stateMngr;

		//States
		bool anyKeyPressed = false;

		public void StateEnter(ScreenStateManager ssm)
		{
			if (stateMngr == null)
			{
				stateMngr = ssm;
			}

			stateMngr.splashRef.menuHandler.ReselectSelectedButton();
		}

		public void HandleAnyInput()
		{
			if (!stateMngr.splashRef.persRef.switchBoard.isPublicDemo && !anyKeyPressed)
			{
				stateMngr.splashRef.splashSceneLoading.StartSceneTransition();
				anyKeyPressed = true;
			}
		}

		public void StateExit()
		{
		}

		public void HandleActionInput()
		{
		}
		public void HandleRewindInput()
		{
		}
		public void HandleResetInput()
		{
		}
		public void HandleEscapeInput()
		{
			stateMngr.SwitchState(stateMngr.settingsOverlayState, ScreenStates.settingsOverlayState);
		}
		public void HandleDebugCompleteInput()
		{
		}
		public void HandleDebugUnlockAllInput()
		{
		}
		public void HandleDebugDeleteProgressInput()
		{
		}
		public void HandleDebugCompleteAllInput()
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
