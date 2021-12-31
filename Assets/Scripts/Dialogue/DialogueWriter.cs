using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Qbism.Dialogue
{
	public class DialogueWriter : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float letterInterval = .05f;
		[SerializeField] TextMeshProUGUI dialogueText;
		[SerializeField] DialogueManager dialogueManager;

		//States
		string fullText;

		public void StartWritingText(string incText)
		{
			StartCoroutine(WriteText(incText));
		}

		private IEnumerator WriteText(string fullText)
		{
			string currentText;

			for (int i = 0; i <= fullText.Length; i++)
			{
				currentText = fullText.Substring(0, i);
				dialogueText.text = currentText;
				yield return new WaitForSeconds(letterInterval);
			}

			dialogueManager.PulseNextButton();
		}
	}
}
