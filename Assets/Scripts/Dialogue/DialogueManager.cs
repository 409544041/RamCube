using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qbism.SpriteAnimations;
using Qbism.Serpent;
using MoreMountains.Feedbacks;
using TMPro;

namespace Qbism.Dialogue
{
	public class DialogueManager : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Vector3[] floatingHeadPos;
		[SerializeField] GameplayCoreRefHolder gcRef;
		[SerializeField] SerpCoreRefHolder scRef;

		//States
		DialogueScripOb dialogueSO;
		int dialogueIndex;
		public bool inDialogue { get; set; } = false;
		GameObject[] heads = new GameObject[2];
		string[] names = new string[2];
		ExpressionHandler[] expressionHandlers = new ExpressionHandler[2];
		SegmentAnimator partnerAnimator;
		DialogueFocuser focuser; DialogueWriter writer; MMFeedbacks nextButtonJuice;
		CanvasGroup dialogueCanvasGroup; TextMeshProUGUI charName; Camera cam; Canvas bgCanvas;
		CanvasGroup bgCanvasGroup;

		private void Awake()
		{
			if (gcRef != null)
			{
				focuser = gcRef.glRef.dialogueFocuser; writer = gcRef.glRef.dialogueWriter;
				nextButtonJuice = gcRef.dialogueNextButtonJuice; dialogueCanvasGroup = gcRef.dialogueCanvasGroup;
				charName = gcRef.characterNameText; cam = gcRef.cam; bgCanvas = gcRef.bgCanvas;
				bgCanvasGroup = gcRef.bgCanvasGroup;
			}
			else if (scRef != null)
			{
				focuser = scRef.slRef.dialogueFocuser; writer = scRef.slRef.dialogueWriter;
				nextButtonJuice = scRef.dialogueNextButtonJuice; dialogueCanvasGroup = scRef.dialogueCanvasGroup;
				charName = scRef.characterNameText; cam = scRef.cam; bgCanvas = scRef.bgCanvas;
				bgCanvasGroup = scRef.bgCanvasGroup;
			}
		}

		public void StartDialogue(DialogueScripOb incDialogueSO, GameObject[] objs, Vector3[] rots,
			SegmentAnimator segAnimator)
		{
			dialogueSO = incDialogueSO;
			partnerAnimator = segAnimator;
			inDialogue = true;
			nextButtonJuice.Initialization();
			dialogueIndex = 0;

			SetupBackgroundCanvas();
			dialogueCanvasGroup.alpha = 1;
			if (scRef != null) scRef.serpScreenCanvasGroup.alpha = 0;

			for (int i = 0; i < 2; i++)
			{
				heads[i] = SpawnDialogueFloatingHeads(objs[i], rots[i], 
					floatingHeadPos[i], focuser.nonFocusScale);

				var segRef = heads[i].GetComponent<SegmentRefHolder>();

				if (segRef != null)
				{
					expressionHandlers[i] = segRef.exprHandler;
					names[i] = segRef.mSegments.f_SegmentName;
					focuser.SetJuiceValues(segRef, i);
					focuser.SetInitialFocusValues(segRef, i);
				}
			}

			expressionHandlers[1].SetFace(dialogueSO.partnerFirstExpression, -1);

			Dialogue();
		}

		private void Dialogue()
		{
			nextButtonJuice.StopFeedbacks();
			var charIndex = dialogueSO.dialogues[dialogueIndex].characterSpeaking;

			charName.text = names[charIndex];
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
			if (writer.isTyping) 
				writer.showFullText = true;
			else
			{
				dialogueIndex++;
				if (dialogueIndex >= dialogueSO.dialogues.Length) StartCoroutine(ExitDialogue());
				else Dialogue();
			}
		}

		private GameObject SpawnDialogueFloatingHeads(GameObject obj, Vector3 rot, Vector3 pos, float scale)
		{
			var spawnPos = cam.ViewportToWorldPoint(pos);

			var head = Instantiate(obj, spawnPos, Quaternion.identity);
			head.transform.parent = cam.transform;
			head.transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);
			head.transform.localScale = transform.localScale * scale;

			return head;
		}

		private void SetupBackgroundCanvas()
		{
			bgCanvas.transform.parent = cam.transform;
			bgCanvas.transform.rotation = cam.transform.rotation;
			bgCanvas.transform.localPosition = new Vector3(0, 0, 10);
			bgCanvasGroup.alpha = 1;
		}

		public void PulseNextButton()
		{
			nextButtonJuice.PlayFeedbacks();
		}

		private IEnumerator ExitDialogue()
		{
			if (gcRef != null) partnerAnimator.InitiateHappyWiggle();

			yield return new WaitForSeconds(.5f); //So when dialogue UI disappears animation is already playing

			inDialogue = false;
			nextButtonJuice.StopFeedbacks();

			for (int i = 0; i < heads.Length; i++)
			{
				GameObject.Destroy(heads[i]);
			}

			dialogueCanvasGroup.alpha = 0;
			bgCanvasGroup.alpha = 0;
			if (scRef != null) scRef.serpScreenCanvasGroup.alpha = 1;
		}

	}
}
