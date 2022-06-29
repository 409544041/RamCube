using Qbism.Control;
using Qbism.Serpent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.ScreenStateMachine
{
	public class SerpentScreenState : MonoBehaviour, IScreenBaseState
	{
		//Cache
		ScreenStateManager stateMngr;
		SerpCoreRefHolder scRef;

		//States
		public bool allowInput { get; set; } = true;

		public void StateEnter(ScreenStateManager ssm)
		{
			if (stateMngr == null)
			{
				stateMngr = ssm;
				scRef = stateMngr.scRef;
			}
		}

		public void HandleStickValues(Vector2 stickValue, InputDetector inputDetector)
		{
			if (!allowInput) return;

			if (stickValue.x > .5)
			{
				scRef.slRef.scroller.ScrollLeft();
				inputDetector.inputting = true;
			}
			else if (stickValue.x < -.5)
			{
				scRef.slRef.scroller.ScrollRight();
				inputDetector.inputting = true;
			}
		}

		public void HandleActionInput()
		{
			if (!allowInput) return;
			scRef.slRef.objSegChecker.StartDialogue();
		}

		public void HandleEscapeInput()
		{
			if (!allowInput) return;
			stateMngr.SwitchState(stateMngr.pauseOverlayState, ScreenStates.pauseOverlayState);
		}

		public void HandleBackInput()
		{
			scRef.slRef.mapLoader.StartLoadingWorldMap(false);
		}

		public void HandleDebugToggleHudInput()
		{
			if (!scRef.persRef.switchBoard.allowHudToggle) return;

			if (scRef.serpScreenCanvasGroup.alpha == 1)
			{
				scRef.serpScreenCanvasGroup.alpha = 0;
				scRef.persRef.hudToggler.hudVisible = false;
			}
			else
			{
				scRef.serpScreenCanvasGroup.alpha = 1;
				scRef.persRef.hudToggler.hudVisible = true;
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
	}
}
