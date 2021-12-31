using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Qbism.Dialogue
{
	public class DialogueWriter : MonoBehaviour
	{
		//Config parameters
		[SerializeField] TextMeshProUGUI dialogueText;
		[SerializeField] DialogueManager dialogueManager;

		//States
		string fullText;
		public bool isTyping { get; private set; } = false;
		public bool showFullText { get; set; } = false;

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
