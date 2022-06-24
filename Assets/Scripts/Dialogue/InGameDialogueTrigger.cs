using Qbism.Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Dialogue
{
	public class InGameDialogueTrigger : MonoBehaviour
	{
		//Config parameters
		[SerializeField] InGameDialogueScripOb scripOb;
		[SerializeField] InGameDialogueManager inGameDialogueMngr;

		public void TriggerInGameDialogue()
		{
			inGameDialogueMngr.StartInGameDialogue(scripOb);
		}
	}
}
