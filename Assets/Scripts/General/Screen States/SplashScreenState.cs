using Qbism.Control;
using Qbism.SceneTransition;
using Qbism.WorldMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Qbism.General
{
	public class SplashScreenState : MonoBehaviour, IScreenBaseState
	{
		//Config parameters
		[SerializeField] SplashSceneLoading loader;

		//Cache
		ScreenStateManager stateMngr;

		public void StateEnter(ScreenStateManager ssm)
		{
			if (stateMngr == null)
			{
				stateMngr = ssm;
			}
		}

		public void HandleAnyInput()
		{
			loader.StartSceneTransition();
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
	}
}
