using Qbism.Serpent;
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

		public void StartRescueDialogue(SegmentAnimator animator)
		{
			var dialogueManager = FindObjectOfType<DialogueManager>();

			var dialogueToPlay = (DialogueScripOb) m_dialogues.f_RescueDialogue;

			GameObject[] objs = new GameObject[2];
			Vector3[] rots = new Vector3[2];

			var leftEntity = E_Segments.FindEntity(entity =>
				entity.f_name == dialogueToPlay.characters[0].ToString());
			objs[0] = (GameObject) leftEntity.f_Prefab;
			rots[0] = leftEntity.f_DialogueRotation;

			objs[1] = (GameObject) m_segments.f_Prefab;
			rots[1] = m_segments.f_DialogueRotation;

			dialogueManager.StartDialogue(dialogueToPlay, objs, rots, animator);
		}
	}
}
