using Qbism.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class LevelIntroState : MonoBehaviour, IScreenBaseState
	{
		//Cache
		ScreenStateManager stateMngr;

		public void StateEnter(ScreenStateManager ssm)
		{
			if (stateMngr == null)
			{
				stateMngr = ssm;
			}
		}

		public void HandleBackInput()
		{
			stateMngr.gcRef.glRef.mapLoader.StartLoadingWorldMap(true);
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
		public void HandleResetInput()
		{
		}
		public void HandleRewindInput()
		{
		}
		public void HandleActionInput()
		{
		}
		public void HandleEscapeInput()
		{
		}
		public void HandleStickValues(Vector2 stickValue, InputDetector inputDetector)
		{
		}
	}
}
