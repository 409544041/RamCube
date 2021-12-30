using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Qbism.Dialogue
{
	public class DialogueManager : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Canvas dialogueCanvas, backgroundCanvas;
		[SerializeField] TextMeshProUGUI charNameText, dialogueText;
		[SerializeField] Image nextButton;
		[SerializeField] Vector3[] floatingHeadPos;
		[SerializeField] float headScale;

		//States
		DialogueScripOb dialogueSO;
		int dialogueIndex = 0;
		public bool inDialogue { get; set; } = false;
		GameObject leftHead, rightHead;

		public void StartDialogue(DialogueScripOb incDialogueSO, GameObject leftObj, Vector3 leftRot,
			GameObject rightObj, Vector3 rightRot)
		{
			dialogueSO = incDialogueSO;
			inDialogue = true;

			SetupBackgroundCanvas();
			dialogueCanvas.GetComponent<CanvasGroup>().alpha = 1;

			// remove input control over gameplay if it isn't already removed

			leftHead =
				SpawnDialogueFloatingHeads(leftObj, leftRot, floatingHeadPos[0], headScale);
			rightHead =
				SpawnDialogueFloatingHeads(rightObj, rightRot, floatingHeadPos[1], headScale);

			PrintDialogue();
		}

		private GameObject SpawnDialogueFloatingHeads(GameObject obj, Vector3 rot, Vector3 pos, float scale)
		{
			var spawnPos = Camera.main.ViewportToWorldPoint(pos);

			var head = Instantiate(obj, spawnPos, Quaternion.Euler(rot.x, rot.y, rot.z));

			head.transform.localScale = transform.localScale * scale;

			return head;
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
			dialogueIndex++;
			if (dialogueIndex >= dialogueSO.dialogues.Length) ExitDialogue();
			else PrintDialogue();
		}

		private void PrintDialogue()
		{
			var charIndex = dialogueSO.dialogues[dialogueIndex].characterSpeaking;
			charNameText.text = dialogueSO.characters[charIndex].ToString();
			dialogueText.text = dialogueSO.dialogues[dialogueIndex].dialogueText;
		}

		private void ExitDialogue()
		{
			inDialogue = false;
			dialogueCanvas.GetComponent<CanvasGroup>().alpha = 0;
			backgroundCanvas.GetComponent<CanvasGroup>().alpha = 0;
			// turn off dialogue canvas
			// switch player controls back using inDialogue = false
			// find a way to set played = true in dialogues database
		}

	}
}
