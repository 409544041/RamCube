using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Qbism.Dialogue
{
	public class DialogueManager : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Canvas dialogueCanvas, backgroundCanvas;
		[SerializeField] TextMeshProUGUI charNameText, dialogueText;
		[SerializeField] Vector3[] floatingHeadPos;
		[SerializeField] float headScale;

		//States
		DialogueScripOb dialogueSO;
		int dialogueIndex = 0;
		bool inDialogue = false;

		public void StartDialogue(ScriptableObject incDialogueSO, Object floatingHead, Vector3 headRot)
		{
			dialogueSO = (DialogueScripOb)incDialogueSO;
			print("Starting a convo lol");

			// show canvas
			SetupBackgroundCanvas();
			dialogueCanvas.GetComponent<CanvasGroup>().alpha = 1;


			// remove input control over gameplay if it isn't already removed

			// place floating heads at correct location


			// inDialogue = true (a check for input control)
			inDialogue = true;

			// printDialogue first dialogue text
			var charIndex = dialogueSO.dialogues[dialogueIndex].characterSpeaking;
			charNameText.text = dialogueSO.characters[charIndex].ToString();
			dialogueText.text = dialogueSO.dialogues[dialogueIndex].dialogueText;
		}

		private void SetupBackgroundCanvas()
		{
			backgroundCanvas.transform.parent = Camera.main.transform;
			backgroundCanvas.transform.rotation = Camera.main.transform.rotation;
			backgroundCanvas.transform.localPosition = new Vector3(0, 0, 10);
			backgroundCanvas.GetComponent<CanvasGroup>().alpha = 1;
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
