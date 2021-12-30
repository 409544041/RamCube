using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Qbism.SpriteAnimations;
using Qbism.Serpent;

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
		[SerializeField] bool inLevel, inSerpentScreen, inMap;

		//States
		DialogueScripOb dialogueSO;
		int dialogueIndex = 0;
		public bool inDialogue { get; set; } = false;
		GameObject[] heads;
		ExpressionHandler[] expressionHandlers;
		SegmentAnimator partnerAnimator;

		public void StartDialogue(DialogueScripOb incDialogueSO, GameObject leftObj, Vector3 leftRot,
			GameObject rightObj, Vector3 rightRot, SegmentAnimator segAnimator)
		{
			dialogueSO = incDialogueSO;
			partnerAnimator = segAnimator;
			inDialogue = true;

			SetupBackgroundCanvas();
			dialogueCanvas.GetComponent<CanvasGroup>().alpha = 1;

			// remove input control over gameplay if it isn't already removed
			heads = new GameObject[2];
			heads[0] = SpawnDialogueFloatingHeads(leftObj, leftRot, floatingHeadPos[0], headScale);
			heads[1] = SpawnDialogueFloatingHeads(rightObj, rightRot, floatingHeadPos[1], headScale);

			expressionHandlers = new ExpressionHandler[2];
			expressionHandlers[0] = heads[0].GetComponentInChildren<ExpressionHandler>();
			expressionHandlers[1] = heads[1].GetComponentInChildren<ExpressionHandler>();

			expressionHandlers[1].SetFace(dialogueSO.partnerFirstExpression, -1);

			PrintDialogue();
			SetDialogueExpression();
		}

		private void PrintDialogue()
		{
			var charIndex = dialogueSO.dialogues[dialogueIndex].characterSpeaking;
			charNameText.text = dialogueSO.characters[charIndex].ToString();
			dialogueText.text = dialogueSO.dialogues[dialogueIndex].dialogueText;
		}

		private void SetDialogueExpression()
		{
			var charIndex = dialogueSO.dialogues[dialogueIndex].characterSpeaking;
			expressionHandlers[charIndex].SetFace(dialogueSO.dialogues[dialogueIndex].expression, -1);
		}

		public void NextDialogueText()
		{
			dialogueIndex++;
			if (dialogueIndex >= dialogueSO.dialogues.Length) ExitDialogue();
			else
			{
				PrintDialogue();
				SetDialogueExpression();
			}
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

		private void ExitDialogue()
		{
			inDialogue = false;

			for (int i = 0; i < heads.Length; i++)
			{
				GameObject.Destroy(heads[i]);
			}

			dialogueCanvas.GetComponent<CanvasGroup>().alpha = 0;
			backgroundCanvas.GetComponent<CanvasGroup>().alpha = 0;

			if (inLevel) partnerAnimator.InitiateHappyWiggle();

			// find a way to set played = true in dialogues database
		}

	}
}
