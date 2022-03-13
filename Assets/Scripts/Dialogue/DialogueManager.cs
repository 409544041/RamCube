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
		[SerializeField] Vector3[] floatingHeadPos;
		[SerializeField] bool inLevel, inSerpentScreen, inMap;
		[SerializeField] GameplayCoreRefHolder gcRef;

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
			gcRef.dialogueNextButtonJuice.Initialization();

			SetupBackgroundCanvas();
			gcRef.dialogueCanvasGroup.alpha = 1;

			for (int i = 0; i < 2; i++)
			{
				heads[i] = SpawnDialogueFloatingHeads(objs[i], rots[i], 
					floatingHeadPos[i], gcRef.glRef.dialogueFocuser.nonFocusScale);
				expressionHandlers[i] = heads[i].GetComponentInChildren<ExpressionHandler>();
				names[i] = heads[i].GetComponent<M_Segments>().f_SegmentName;
				gcRef.glRef.dialogueFocuser.SetJuiceValues(heads[i], i);
				gcRef.glRef.dialogueFocuser.SetInitialFocusValues(heads[i], i);
			}

			expressionHandlers[1].SetFace(dialogueSO.partnerFirstExpression, -1);

			Dialogue();
		}

		private void Dialogue()
		{
			gcRef.dialogueNextButtonJuice.StopFeedbacks();
			var charIndex = dialogueSO.dialogues[dialogueIndex].characterSpeaking;

			gcRef.characterNameText.text = names[charIndex];
			gcRef.glRef.dialogueWriter.StartWritingText(dialogueSO.dialogues[dialogueIndex].dialogueText);

			gcRef.glRef.dialogueFocuser.SetFocus(charIndex, heads);
			SetDialogueExpression();
		}

		private void SetDialogueExpression()
		{
			var charIndex = dialogueSO.dialogues[dialogueIndex].characterSpeaking;
			expressionHandlers[charIndex].SetFace(dialogueSO.dialogues[dialogueIndex].expression, -1);
		}

		public void NextDialogueText()
		{
			if (gcRef.glRef.dialogueWriter.isTyping) 
				gcRef.glRef.dialogueWriter.showFullText = true;
			else
			{
				dialogueIndex++;
				if (dialogueIndex >= dialogueSO.dialogues.Length) StartCoroutine(ExitDialogue());
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
			gcRef.bgCanvas.transform.parent = Camera.main.transform;
			gcRef.bgCanvas.transform.rotation = Camera.main.transform.rotation;
			gcRef.bgCanvas.transform.localPosition = new Vector3(0, 0, 10);
			gcRef.bgCanvasGroup.alpha = 1;
		}

		public void PulseNextButton()
		{
			gcRef.dialogueNextButtonJuice.PlayFeedbacks();
		}

		private IEnumerator ExitDialogue()
		{
			if (inLevel) partnerAnimator.InitiateHappyWiggle();

			yield return new WaitForSeconds(.5f); //So when dialogue UI disappears animation is already playing

			inDialogue = false;
			gcRef.dialogueNextButtonJuice.StopFeedbacks();

			for (int i = 0; i < heads.Length; i++)
			{
				GameObject.Destroy(heads[i]);
			}

			gcRef.dialogueCanvasGroup.alpha = 0;
			gcRef.bgCanvasGroup.alpha = 0;

			// find a way to set played = true in dialogues database
			// probably handy for quest dialogues, seeing as how they're in arrays
		}

	}
}
