using Qbism.Control;
using Qbism.Dialogue;
using Qbism.Serpent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.ScreenStateMachine
{
	public class DialogueOverlayState : MonoBehaviour, IScreenBaseState
	{
		//Cache
		ScreenStateManager stateMngr;
		DialogueManager dialogueMngr;
		CanvasGroup dialogueCanvasGroup;
		InGameDialogueManager inGameDialogueMngr;
		CanvasGroup inGameDialogueCanvasGroup;

		public void StateEnter(ScreenStateManager ssm)
		{
			if (stateMngr == null)
			{
				stateMngr = ssm;

				if (stateMngr.gcRef != null)
				{
					dialogueMngr = stateMngr.gcRef.glRef.dialogueManager;
					dialogueCanvasGroup = stateMngr.gcRef.dialogueCanvasGroup;
					inGameDialogueMngr = stateMngr.gcRef.glRef.inGameDialogueManager;
					inGameDialogueCanvasGroup = stateMngr.gcRef.inGameDialogueCanvasGroup;
				}

				if (stateMngr.scRef != null)
				{
					dialogueMngr = stateMngr.scRef.slRef.dialogueManager;
					dialogueCanvasGroup = stateMngr.scRef.dialogueCanvasGroup;
				}
			}

			//TO DO: trigger dialogue UI and start convo
		}

		public void HandleActionInput()
		{
			if (inGameDialogueMngr != null && inGameDialogueCanvasGroup.alpha == 1)
				inGameDialogueMngr.NextDialogueText();
			else dialogueMngr.NextDialogueText();
		}

		public void StateExit()
		{
			//TO DO: exit dialogue UI
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
		public void HandleAnyInput()
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
