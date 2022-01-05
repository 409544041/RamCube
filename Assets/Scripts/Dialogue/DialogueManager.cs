using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Qbism.SpriteAnimations;
using Qbism.Serpent;
using MoreMountains.Feedbacks;

namespace Qbism.Dialogue
{
	public class DialogueManager : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Canvas dialogueCanvas, backgroundCanvas;
		[SerializeField] TextMeshProUGUI charNameText;
		[SerializeField] MMFeedbacks nextButtonJuice;
		[SerializeField] Vector3[] floatingHeadPos;
		[SerializeField] bool inLevel, inSerpentScreen, inMap;
		[SerializeField] DialogueWriter writer;
		[SerializeField] DialogueFocuser focuser;

		//States
		DialogueScripOb dialogueSO;
		int dialogueIndex = 0;
		public bool inDialogue { get; set; } = false;
		GameObject[] heads = new GameObject[2];
		string[] names = new string[2];
		ExpressionHandler[] expressionHandlers = new ExpressionHandler[2];
		SegmentAnimator partnerAnimator;

		public void StartDialogue(DialogueScripOb incDialogueSO, GameObject[] objs, Vector3[] rots,
			SegmentAnimator segAnimator)
		{
			dialogueSO = incDialogueSO;
			partnerAnimator = segAnimator;
			inDialogue = true;
			nextButtonJuice.Initialization();

			SetupBackgroundCanvas();
			dialogueCanvas.GetComponent<CanvasGroup>().alpha = 1;

			for (int i = 0; i < 2; i++)
			{
				heads[i] = SpawnDialogueFloatingHeads(objs[i], rots[i], floatingHeadPos[i], focuser.nonFocusScale);
				expressionHandlers[i] = heads[i].GetComponentInChildren<ExpressionHandler>();
				names[i] = heads[i].GetComponent<M_Segments>().f_SegmentName;
				focuser.SetJuiceValues(heads[i], i);
				focuser.SetInitialFocusValues(heads[i], i);
			}

			expressionHandlers[1].SetFace(dialogueSO.partnerFirstExpression, -1);

			Dialogue();
		}

		private void Dialogue()
		{
			nextButtonJuice.StopFeedbacks();
			var charIndex = dialogueSO.dialogues[dialogueIndex].characterSpeaking;

			charNameText.text = names[charIndex];
			writer.StartWritingText(dialogueSO.dialogues[dialogueIndex].dialogueText);

			focuser.SetFocus(charIndex, heads);
			SetDialogueExpression();
		}

		private void SetDialogueExpression()
		{
			var charIndex = dialogueSO.dialogues[dialogueIndex].characterSpeaking;
			expressionHandlers[charIndex].SetFace(dialogueSO.dialogues[dialogueIndex].expression, -1);
		}

		public void NextDialogueText()
		{
			if (writer.isTyping) writer.showFullText = true;
			else
			{
				dialogueIndex++;
				if (dialogueIndex >= dialogueSO.dialogues.Length) ExitDialogue();
				else Dialogue();
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

		public void PulseNextButton()
		{
			nextButtonJuice.PlayFeedbacks();
		}

		private void ExitDialogue()
		{
			inDialogue = false;
			nextButtonJuice.StopFeedbacks();

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
