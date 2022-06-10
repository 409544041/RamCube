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
		//Config parameters
		[SerializeField] SplashSceneLoading loader;
		[SerializeField] FeatureSwitchBoard switchBoard;

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
		}

		public void HandleAnyInput()
		{
			if (!switchBoard.isPublicDemo && !anyKeyPressed)
			{
				loader.StartSceneTransition();
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
