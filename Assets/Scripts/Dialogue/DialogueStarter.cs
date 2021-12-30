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

			var dialogueToPlay = (DialogueScripOb) m_dialogues.f_RescueDialogue;

			var leftEntity = E_Segments.FindEntity(entity =>
				entity.f_name == dialogueToPlay.characters[0].ToString());
			var leftObj = (GameObject) leftEntity.f_DialogueObject;
			var leftRot = leftEntity.f_DialogueRotation;

			var rightObj = (GameObject) m_segments.f_DialogueObject;
			var rightRot = m_segments.f_DialogueRotation;

			dialogueManager.StartDialogue(dialogueToPlay, leftObj, leftRot, rightObj, rightRot);
		}
	}
}
