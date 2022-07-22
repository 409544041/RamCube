using BansheeGz.BGDatabase;
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
			var dialogueEntity = refs.mSegments.f_Dialogues;
			var dialogueData = new DialogueData();

			dialogueData.firstExpr = dialogueEntity.f_RescueFirstExpr;

			for (int i = 0; i < dialogueEntity.f_RescueDialogue.Count; i++)
			{
				dialogueData.charIndexes[i] = dialogueEntity.f_RescueDialogue[i].f_CharIndex;
				dialogueData.expressions[i] = dialogueEntity.f_RescueDialogue[i].f_Expression;
				dialogueData.dialogues[i] = dialogueEntity.f_RescueDialogue[i].f_Text_EN;
			}

			StartDialogue(dialogueData, segAnim);
		}

		public void StartDialogue(DialogueData dialogueData, SegmentAnimator segAnim)
		{
			GameObject[] objs = new GameObject[2];
			Vector3[] rots = new Vector3[2];

			var leftEntity = E_Segments.GetEntity(11);
			objs[0] = (GameObject)leftEntity.f_Prefab;
			rots[0] = leftEntity.f_DialogueRotation;

			objs[1] = (GameObject)refs.mSegments.f_Prefab;
			rots[1] = refs.mSegments.f_DialogueRotation;

			//This here instead of in Awake bc gcRef gets assigned later via playerAnim on rescued segments
			if (refs.gcRef != null) dialogueManager = refs.gcRef.glRef.dialogueManager;
			if (refs.scRef != null) dialogueManager = refs.scRef.slRef.dialogueManager;

			dialogueManager.StartDialogue(dialogueData, objs, rots, segAnim);
		}
	}
}
