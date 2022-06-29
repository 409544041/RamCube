using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qbism.SpriteAnimations;
using Qbism.Serpent;
using MoreMountains.Feedbacks;
using TMPro;
using Febucci.UI;
using Qbism.ScreenStateMachine;
using Qbism.General;

namespace Qbism.Dialogue
{
	public class DialogueManager : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Vector3[] floatingHeadPos;
		[SerializeField] float firstTextDelay = .25f;
		[SerializeField] GameplayCoreRefHolder gcRef;
		[SerializeField] SerpCoreRefHolder scRef;

		//Cache
		SegmentAnimator partnerAnimator;
		DialogueFocuser focuser; MMFeedbacks nextButtonJuice;
		CanvasGroup dialogueCanvasGroup; TextMeshProUGUI charName; Camera cam;
		TextMeshProUGUI dialogueText; TextAnimatorPlayer typeWriter;
		MMFeedbacks textAppearJuice; ScreenStateManager screenStateMngr; GaussianCanvas gCanvas;

		//States
		DialogueScripOb dialogueSO;
		int dialogueIndex;
		GameObject[] heads = new GameObject[2];
		string[] names = new string[2];
		bool isTyping = false;
		ExpressionHandler[] expressionHandlers = new ExpressionHandler[2];
		bool exiting = false;


		private void Awake()
		{
			if (gcRef != null)
			{
				focuser = gcRef.glRef.dialogueFocuser; nextButtonJuice = gcRef.dialogueNextButtonJuice; 
				dialogueCanvasGroup = gcRef.dialogueCanvasGroup; charName = gcRef.characterNameText; 
				cam = gcRef.cam; dialogueText = gcRef.dialogueText; typeWriter = gcRef.typeWriter; 
				textAppearJuice = gcRef.textAppearJuice; screenStateMngr = gcRef.glRef.screenStateMngr;
				gCanvas = gcRef.gausCanvas;
			}
			else if (scRef != null)
			{
				focuser = scRef.slRef.dialogueFocuser; nextButtonJuice = scRef.dialogueNextButtonJuice; 
				dialogueCanvasGroup = scRef.dialogueCanvasGroup; charName = scRef.characterNameText; 
				cam = scRef.cam; dialogueText = scRef.dialogueText; typeWriter = scRef.typeWriter; 
				textAppearJuice = scRef.textAppearJuice; screenStateMngr = scRef.slRef.screenStateMngr;
				gCanvas = scRef.gausCanvas;
			}
		}

		public void StartDialogue(DialogueScripOb incDialogueSO, GameObject[] objs, Vector3[] rots,
			SegmentAnimator segAnimator)
		{
			screenStateMngr.SwitchState(screenStateMngr.dialogueOverlayState, ScreenStates.dialogueOverlayState);
			dialogueSO = incDialogueSO;
			partnerAnimator = segAnimator;
			nextButtonJuice.Initialization();
			textAppearJuice.Initialization();
			dialogueIndex = 0;
			exiting = false;

			gCanvas.SetUpGaussianCanvas();
			textAppearJuice.PlayFeedbacks();
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
					if (segRef.dragonAnim != null) segRef.dragonAnim.DragonSmile();
				}
			}
			var charIndex = dialogueSO.dialogues[dialogueIndex].characterSpeaking;
			if (charIndex == 0)
			{
				if (expressionHandlers[1] != null)
					expressionHandlers[1].SetFace(dialogueSO.partnerFirstExpression, -1);
			}
			else
			{
				if (expressionHandlers[0] != null)
					expressionHandlers[0].SetFace(dialogueSO.partnerFirstExpression, -1);
			}

			StartCoroutine(Dialogue());
		}

		private IEnumerator Dialogue()
		{
			nextButtonJuice.StopFeedbacks();
			textAppearJuice.PlayFeedbacks();
			var charIndex = dialogueSO.dialogues[dialogueIndex].characterSpeaking;

			charName.text = names[charIndex];

			focuser.SetFocus(charIndex, heads);
			SetDialogueExpression();

			if (dialogueIndex == 0)
			{
				dialogueText.text = " ";
				yield return new WaitForSeconds(firstTextDelay);
			}

			dialogueText.text = dialogueSO.dialogues[dialogueIndex].dialogueText;
		}

		private void SetDialogueExpression()
		{
			var charIndex = dialogueSO.dialogues[dialogueIndex].characterSpeaking;
			if (expressionHandlers[charIndex] != null) expressionHandlers[charIndex].
					SetFace(dialogueSO.dialogues[dialogueIndex].expression, -1);
		}

		public void NextDialogueText()
		{
			if (isTyping) typeWriter.SkipTypewriter();
			else
			{
				dialogueIndex++;
				if (dialogueIndex >= dialogueSO.dialogues.Length) StartCoroutine(ExitDialogue());
				else StartCoroutine(Dialogue());
			}
		}

		private GameObject SpawnDialogueFloatingHeads(GameObject obj, Vector3 rot, Vector3 pos, 
			float scale)
		{
			var spawnPos = cam.ViewportToWorldPoint(pos);

			var head = Instantiate(obj, spawnPos, Quaternion.identity);
			head.transform.parent = cam.transform;
			head.transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);
			head.transform.localScale = transform.localScale * scale;

			return head;
		}

		public void PulseNextButton() //Called from TextAnimatorPlayer events
		{
			nextButtonJuice.PlayFeedbacks();
		}

		private IEnumerator ExitDialogue()
		{
			if (exiting) yield break;
			exiting = true;

			if (gcRef != null && partnerAnimator != null)
			{
				partnerAnimator.InitiateHappyWiggle();
				yield return new WaitForSeconds(.5f); //So when dialogue UI disappears animation is already playing
			}
			
			if (gcRef != null && partnerAnimator == null) //means this is dragon head
			{
				exiting = true;
				gcRef.glRef.mapLoader.StartLoadingWorldMap(true);
				yield break;
			}

			dialogueText.text = " ";

			if (gcRef != null) screenStateMngr.SwitchState(screenStateMngr.levelEndSeqState,
				ScreenStates.levelEndSeqState);
			if (scRef != null) screenStateMngr.SwitchState(screenStateMngr.serpentScreenState,
				ScreenStates.serpentScreenState);

			nextButtonJuice.StopFeedbacks();

			for (int i = 0; i < heads.Length; i++)
			{
				GameObject.Destroy(heads[i]);
			}

			
			dialogueCanvasGroup.alpha = 0;
			gCanvas.TurnOffGaussianCanvas();

			if (scRef != null)
			{
				scRef.slRef.serpScreenUIHandler.SetSerpScreenAlpha(1);

				var checker = scRef.slRef.objSegChecker;
				checker.DecideOnDialogueToPlay(checker.segInFocus);

				if (checker.segInFocus.uiHandler != null)
					checker.segInFocus.uiHandler.ToggleUIDependingOnObjectStatus();

				var serpUIHandler = scRef.slRef.serpScreenUIHandler;
				var objs = checker.segInFocus.mSegments.f_Objects;
				serpUIHandler.ShowObjectUI(objs);
			}
		}

		public void SetTyping(bool value) //Called from TextAnimatorPlayer events
		{
			isTyping = value;
		}

	}
}
