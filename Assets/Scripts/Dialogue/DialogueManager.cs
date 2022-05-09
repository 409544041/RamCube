using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qbism.SpriteAnimations;
using Qbism.Serpent;
using MoreMountains.Feedbacks;
using TMPro;
using Febucci.UI;
using Qbism.General;

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
		bool isTyping = false;
		ExpressionHandler[] expressionHandlers = new ExpressionHandler[2];
		SegmentAnimator partnerAnimator;
		DialogueFocuser focuser; MMFeedbacks nextButtonJuice;
		CanvasGroup dialogueCanvasGroup; TextMeshProUGUI charName; Camera cam; Canvas bgCanvas;
		CanvasGroup bgCanvasGroup; TextMeshProUGUI dialogueText; TextAnimatorPlayer typeWriter;
		MMFeedbacks textAppearJuice; ScreenStateManager screenStateMngr;

		private void Awake()
		{
			if (gcRef != null)
			{
				focuser = gcRef.glRef.dialogueFocuser; nextButtonJuice = gcRef.dialogueNextButtonJuice; 
				dialogueCanvasGroup = gcRef.dialogueCanvasGroup; charName = gcRef.characterNameText; 
				cam = gcRef.cam; bgCanvas = gcRef.bgCanvas; bgCanvasGroup = gcRef.bgCanvasGroup; 
				dialogueText = gcRef.dialogueText; typeWriter = gcRef.typeWriter;
				textAppearJuice = gcRef.textAppearJuice; screenStateMngr = gcRef.glRef.screenStateMngr;
			}
			else if (scRef != null)
			{
				focuser = scRef.slRef.dialogueFocuser; nextButtonJuice = scRef.dialogueNextButtonJuice; 
				dialogueCanvasGroup = scRef.dialogueCanvasGroup; charName = scRef.characterNameText; 
				cam = scRef.cam; bgCanvas = scRef.bgCanvas; bgCanvasGroup = scRef.bgCanvasGroup; 
				dialogueText = scRef.dialogueText; typeWriter = scRef.typeWriter;
				textAppearJuice = scRef.textAppearJuice; screenStateMngr = scRef.slRef.screenStateMngr;
			}
		}

		public void StartDialogue(DialogueScripOb incDialogueSO, GameObject[] objs, Vector3[] rots,
			SegmentAnimator segAnimator)
		{
			screenStateMngr.SwitchState(screenStateMngr.dialogueOverlayState);
			dialogueSO = incDialogueSO;
			partnerAnimator = segAnimator;
			inDialogue = true;
			nextButtonJuice.Initialization();
			dialogueIndex = 0;

			SetupBackgroundCanvas();
			dialogueCanvasGroup.alpha = 1;
			if (scRef != null) scRef.slRef.serpScreenUIHandler.SetSerpScreenAlpha(0);

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
			var charIndex = dialogueSO.dialogues[dialogueIndex].characterSpeaking;
			if (charIndex == 0) expressionHandlers[1].SetFace(dialogueSO.partnerFirstExpression, -1);
			else expressionHandlers[0].SetFace(dialogueSO.partnerFirstExpression, -1);

			Dialogue();
		}

		private void Dialogue()
		{
			nextButtonJuice.StopFeedbacks();
			textAppearJuice.PlayFeedbacks();
			var charIndex = dialogueSO.dialogues[dialogueIndex].characterSpeaking;

			charName.text = names[charIndex];
			dialogueText.text = dialogueSO.dialogues[dialogueIndex].dialogueText;

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
			if (isTyping) typeWriter.SkipTypewriter();
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

			if (gcRef != null) screenStateMngr.SwitchState(screenStateMngr.levelEndSeqState);
			if (scRef != null) screenStateMngr.SwitchState(screenStateMngr.serpentScreenState);

			inDialogue = false;
			nextButtonJuice.StopFeedbacks();

			for (int i = 0; i < heads.Length; i++)
			{
				GameObject.Destroy(heads[i]);
			}

			dialogueCanvasGroup.alpha = 0;
			bgCanvasGroup.alpha = 0;

			if (scRef != null)
			{
				scRef.slRef.serpScreenUIHandler.SetSerpScreenAlpha(1);
				var checker = scRef.slRef.objSegChecker;
				checker.DecideOnDialogueToPlay(checker.segInFocus);
				checker.segInFocus.uiHandler.ToggleUIDependingOnObjectStatus();
			}
		}

		public void SetTyping(bool value) //Called from TextAnimatorPlayer events
		{
			isTyping = value;
		}

	}
}
