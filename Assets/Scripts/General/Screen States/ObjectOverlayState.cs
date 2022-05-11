using Qbism.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class ObjectOverlayState : MonoBehaviour, IScreenBaseState
	{
		//Cache
		ScreenStateManager stateMngr;
		GameLogicRefHolder glRef;

		public void StateEnter(ScreenStateManager ssm)
		{
			if (stateMngr == null)
			{
				stateMngr = ssm;
				glRef = stateMngr.gcRef.glRef;
			}
			//TO DO: activate object overlay ui
		}

		public void HandleActionInput()
		{
			glRef.mapLoader.StartLoadingWorldMap(true);
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
		public void HandleEscapeInput()
		{
		}
		public void HandleStickValues(Vector2 stickValue, InputDetector inputDetector)
		{
		}
		public void HandleBackInput()
		{
		}
	}
}
