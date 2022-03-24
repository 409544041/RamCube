using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qbism.Serpent;
using TMPro;

namespace Qbism.Dialogue
{
	public class DialogueWriter : MonoBehaviour
	{
		//Config parameters
		[SerializeField] GameplayCoreRefHolder gcRef;
		[SerializeField] SerpCoreRefHolder scRef;
		[SerializeField] DialogueManager dialogueManager;

		//States
		public bool isTyping { get; private set; } = false;
		public bool showFullText { get; set; } = false;
		TextMeshProUGUI dialogueText;

		private void Awake()
		{
			if (gcRef != null) dialogueText = gcRef.dialogueText;
			if (scRef != null) dialogueText = scRef.dialogueText;
		}

		public void StartWritingText(string incText)
		{
			StartCoroutine(WriteText(incText));
		}

		private IEnumerator WriteText(string fullText)
		{
			string currentText;
			isTyping = true;

			for (int i = 0; i <= fullText.Length; i++)
			{
				if (showFullText)
				{
					dialogueText.text = fullText;
					break;
				}
				else
				{
					currentText = fullText.Substring(0, i);
					dialogueText.text = currentText;
					yield return null;
				}
			}

			showFullText = false;
			isTyping = false;
			dialogueManager.PulseNextButton();
		}
	}
}
