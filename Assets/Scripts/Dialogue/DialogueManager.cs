using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
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
		[SerializeField] float focusScale, nonFocusScale, scaleDur = .2f;
		[SerializeField] AnimationCurve scaleCurve;
		[SerializeField] bool inLevel, inSerpentScreen, inMap;
		[SerializeField] DialogueWriter writer;

		//States
		DialogueScripOb dialogueSO;
		int dialogueIndex = 0;
		public bool inDialogue { get; set; } = false;
		GameObject[] heads;
		ExpressionHandler[] expressionHandlers;
		MMFeedbacks[] scaleJuice;
		MMFeedbackScale[] mmscaler;
		SegmentAnimator partnerAnimator;
		float originalCurveOne, curveDelta;
		bool originalValuesSet = false;
		float elapsedTime = 0;

		public void StartDialogue(DialogueScripOb incDialogueSO, GameObject[] objs, Vector3[] rots,
			SegmentAnimator segAnimator)
		{
			dialogueSO = incDialogueSO;
			partnerAnimator = segAnimator;
			inDialogue = true;
			nextButtonJuice.Initialization();

			SetupBackgroundCanvas();
			dialogueCanvas.GetComponent<CanvasGroup>().alpha = 1;

			//TO DO: remove input control over gameplay if it isn't already removed

			heads = new GameObject[2];
			expressionHandlers = new ExpressionHandler[2];
			scaleJuice = new MMFeedbacks[2];
			mmscaler = new MMFeedbackScale[2];

			for (int i = 0; i < 2; i++)
			{
				heads[i] = SpawnDialogueFloatingHeads(objs[i], rots[i], floatingHeadPos[i], nonFocusScale);
				expressionHandlers[i] = heads[i].GetComponentInChildren<ExpressionHandler>();
				scaleJuice[i] = heads[i].GetComponent<SegmentScroll>().scrollJuice;
				scaleJuice[i].Initialization();
				mmscaler[i] = scaleJuice[i].GetComponent<MMFeedbackScale>();
			}

			expressionHandlers[1].SetFace(dialogueSO.partnerFirstExpression, -1);

			Dialogue();
		}

		private void Dialogue()
		{
			nextButtonJuice.StopFeedbacks();
			var charIndex = dialogueSO.dialogues[dialogueIndex].characterSpeaking;

			charNameText.text = dialogueSO.characters[charIndex].ToString();
			writer.StartWritingText(dialogueSO.dialogues[dialogueIndex].dialogueText);

			SetFocusScale(charIndex);
			SetDialogueExpression();
		}

		private void SetFocusScale(int charIndex)
		{
			for (int i = 0; i < heads.Length; i++)
			{
				var bigScale = new Vector3(focusScale, focusScale, focusScale);
				var smallScale = new Vector3(nonFocusScale, nonFocusScale, nonFocusScale);

				if (i == charIndex && heads[i].transform.localScale != bigScale)
				{
					StartCoroutine(ScaleHead(i, heads[i], bigScale, true));
				}
					

				else if (i != charIndex && heads[i].transform.localScale != smallScale)
				{
					StartCoroutine(ScaleHead(i, heads[i], smallScale, false));
				}
			}
		}

		private IEnumerator ScaleHead(int i, GameObject head, Vector3 targetScale, bool scaleUp)
		{
			var startScale = head.transform.localScale;
			elapsedTime = 0;

			while (!Mathf.Approximately(head.transform.localScale.x, targetScale.x))
			{
				elapsedTime += Time.deltaTime;
				var percentageComplete = elapsedTime / scaleDur;

				head.transform.localScale = Vector3.Lerp(startScale, targetScale,
					scaleCurve.Evaluate(percentageComplete));

				yield return null;
			}

			TriggerScaleJuice(i, scaleUp);
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

		private void TriggerScaleJuice(int i, bool scaleUp)
		{
			if (originalValuesSet == false)
			{
				originalCurveOne = mmscaler[i].RemapCurveOne;
				curveDelta = originalCurveOne - mmscaler[i].RemapCurveZero;
				originalValuesSet = true;
			}

			if (scaleUp) mmscaler[i].RemapCurveOne = originalCurveOne;
			else mmscaler[i].RemapCurveOne = mmscaler[i].RemapCurveZero - curveDelta;

			scaleJuice[i].PlayFeedbacks();
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
