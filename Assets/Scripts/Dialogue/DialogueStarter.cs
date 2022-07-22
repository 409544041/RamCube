using Qbism.Serpent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Dialogue
{
	public class DialogueStarter : MonoBehaviour
	{
		//Config parameters
		[SerializeField] SegmentRefHolder refs;

		//Cache
		DialogueManager dialogueManager;

		public void StartRescueDialogue(SegmentAnimator segAnim)
		{
			//var dialogueToPlay = (DialogueScripOb)refs.mSegments.f_Dialogues.f_RescueDialogue;
			//StartDialogue(dialogueToPlay, segAnim);
		}

		public void StartDialogue(DialogueScripOb dialogueToPlay, SegmentAnimator segAnim)
		{
			GameObject[] objs = new GameObject[2];
			Vector3[] rots = new Vector3[2];

			var leftEntity = E_Segments.FindEntity(entity =>
				entity.f_name == dialogueToPlay.characters[0].ToString());
			objs[0] = (GameObject)leftEntity.f_Prefab;
			rots[0] = leftEntity.f_DialogueRotation;

			objs[1] = (GameObject)refs.mSegments.f_Prefab;
			rots[1] = refs.mSegments.f_DialogueRotation;

			//This here instead of in Awake bc gcRef gets assigned later via playerAnim on rescued segments
			if (refs.gcRef != null) dialogueManager = refs.gcRef.glRef.dialogueManager;
			if (refs.scRef != null) dialogueManager = refs.scRef.slRef.dialogueManager;

			dialogueManager.StartDialogue(dialogueToPlay, objs, rots, segAnim);
		}
	}
}
