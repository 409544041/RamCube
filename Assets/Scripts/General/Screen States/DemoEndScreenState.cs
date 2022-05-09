using Qbism.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class DemoEndScreenState : MonoBehaviour, IScreenBaseState
	{
		public void StateEnter(ScreenStateManager ssm)
		{
		}

		public void HandleActionInput()
		{
		}

		public void HandleEscapeInput()
		{
		}

		public void StateExit()
		{
		}

		public void HandleStickValues(Vector2 stickValue, InputDetector inputDetector)
		{
			throw new System.NotImplementedException();
		}

		public void HandleRewindInput()
		{
			throw new System.NotImplementedException();
		}

		public void HandleResetInput()
		{
			throw new System.NotImplementedException();
		}

		public void HandleDebugCompleteInput()
		{
			throw new System.NotImplementedException();
		}

		public void HandleDebugUnlockAllInput()
		{
			throw new System.NotImplementedException();
		}

		public void HandleDebugDeleteProgressInput()
		{
			throw new System.NotImplementedException();
		}

		public void HandleDebugCompleteAllInput()
		{
			throw new System.NotImplementedException();
		}

		public void HandleAnyInput()
		{
			throw new System.NotImplementedException();
		}
	}
}
