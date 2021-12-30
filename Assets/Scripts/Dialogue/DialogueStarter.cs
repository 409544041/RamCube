using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Dialogue
{
	public class DialogueStarter : MonoBehaviour
	{
		//Config parameters
		[SerializeField] M_Segments m_segments;
		[SerializeField] M_Dialogues m_dialogues;

		public void StartRescueDialogue()
		{
			var dialogueManager = FindObjectOfType<DialogueManager>();
			var dialogueToPlay = m_dialogues.f_RescueDialogue;
			var floatingHead = m_segments.f_DialogueObject;
			var headRot = m_segments.f_DialogueRotation;

			dialogueManager.StartDialogue(dialogueToPlay, floatingHead, headRot);
		}
	}
}
