using Qbism.Control;
using Qbism.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class DemoIntroScreenState : MonoBehaviour, IScreenBaseState
	{
		//Config parameters
		[SerializeField] DemoScreenNavigator navigator;

		//Cache
		ScreenStateManager stateMngr;

		public void StateEnter(ScreenStateManager ssm)
		{
			if (stateMngr == null)
			{
				stateMngr = ssm;
			}
		}

		public void HandleActionInput()
		{
			navigator.GoNext();
		}

		public void StateExit()
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
		public void HandleEscapeInput()
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