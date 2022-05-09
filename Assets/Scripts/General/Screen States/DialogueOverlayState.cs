using Qbism.Control;
using Qbism.Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class DialogueOverlayState : MonoBehaviour, IScreenBaseState
	{
		//Cache
		ScreenStateManager stateMngr;
		DialogueManager dialogueMngr;

		public void StateEnter(ScreenStateManager ssm)
		{
			if (stateMngr == null)
			{
				stateMngr = ssm;
				if (stateMngr.gcRef != null) dialogueMngr = stateMngr.gcRef.glRef.dialogueManager;
				if (stateMngr.scRef != null) dialogueMngr = stateMngr.scRef.slRef.dialogueManager;
			}

			//TO DO: trigger dialogue UI and start convo
		}

		public void HandleActionInput()
		{
			dialogueMngr.NextDialogueText();
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
	}
}
