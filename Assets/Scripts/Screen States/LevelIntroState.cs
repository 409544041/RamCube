using Qbism.Control;
using Qbism.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.ScreenStateMachine
{
	public class LevelIntroState : MonoBehaviour, IScreenBaseState
	{
		//Cache
		ScreenStateManager stateMngr;
		GameplayCoreRefHolder gcRef;
		PersistentRefHolder persRef;

		public void StateEnter(ScreenStateManager ssm)
		{
			if (stateMngr == null)
			{
				stateMngr = ssm;
				gcRef = stateMngr.gcRef;
				persRef = gcRef.persRef;
			}
		}

		public void HandleBackInput()
		{
			gcRef.glRef.mapLoader.StartLoadingWorldMap(true);
		}

		public void HandleDebugToggleHudInput()
		{
			if (!persRef.switchBoard.allowHudToggle) return;

			if (gcRef.gameplayCanvasGroup.alpha == 1)
			{
				gcRef.gameplayCanvasGroup.alpha = 0;
				persRef.hudToggler.hudVisible = false;
			}
			else
			{
				gcRef.gameplayCanvasGroup.alpha = 1;
				persRef.hudToggler.hudVisible = true;
			}
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
