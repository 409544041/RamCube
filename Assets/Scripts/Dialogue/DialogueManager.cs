using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Dialogue
{
	public class DialogueManager : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Canvas dialogueCanvas;
		[SerializeField] Transform[] floatingHeadPos;
		[SerializeField] float headScale;

		//States
		DialogueScripOb dialogueSO;
		int dialogueIndex = 0;

		public void StartDialogue(ScriptableObject incDialogueSO, Object floatingHead, Vector3 headRot)
		{
			dialogueSO = (DialogueScripOb)incDialogueSO;
			print("Starting a convo lol");
			// remove input control over gameplay if it isn't already removed
			// show canvas
			// place floating heads at correct location
			// inDialogue = true (a check for input control)
			// printDialogue first dialogue text
		}

		public void NextDialogueText()
		{
			// dialogueindex++
			// if dialogue index >= dialogueSO.dialogues length, ExitDialogue
			// else printDialogue next dialogueSO.dialogues

		}

		private void PrintDialogue()
		{
			// canvas text = dialogueSO text
		}

		private void ExitDialogue()
		{
			// turn off dialogue canvas
			// switch player controls back using inDialogue = false
			// find a way to set played = true in dialogues database
		}

	}
}
