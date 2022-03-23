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

		public void StartRescueDialogue(SegmentAnimator segAnim)
		{
			var dialogueToPlay = (DialogueScripOb)m_segments.f_Dialogues.f_RescueDialogue;
			StartDialogue(dialogueToPlay, segAnim);
		}

		private void StartDialogue(DialogueScripOb dialogueToPlay, SegmentAnimator segAnim)
		{
			var dialogueManager = FindObjectOfType<DialogueManager>();

			GameObject[] objs = new GameObject[2];
			Vector3[] rots = new Vector3[2];

			var leftEntity = E_Segments.FindEntity(entity =>
				entity.f_name == dialogueToPlay.characters[0].ToString());
			objs[0] = (GameObject)leftEntity.f_Prefab;
			rots[0] = leftEntity.f_DialogueRotation;

			objs[1] = (GameObject)m_segments.f_Prefab;
			rots[1] = m_segments.f_DialogueRotation;

			dialogueManager.StartDialogue(dialogueToPlay, objs, rots, segAnim);
		}
	}
}
