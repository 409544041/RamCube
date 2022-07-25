using Qbism.Dialogue;
using Qbism.WorldMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Dialogue
{
	public class InGameDialogueTrigger : MonoBehaviour
	{
		//Config parameters
		[SerializeField] M_InGameDialogues mInGameDialogue;
		[SerializeField] InGameDialogueManager inGameDialogueMngr;

		public void TriggerInGameDialogue(MapDialogueMoment dialMoment, LevelPinUI selectedPinUI)
		{
			var dialogueEntity = mInGameDialogue.f_InGameDialogue;
			var segmentEntity = mInGameDialogue.f_Dialogues.f_Segment;
			var dialogueData = new DialogueData();
			dialogueData.charIndexes = new List<int>();
			dialogueData.expressions = new List<Expressions>();
			dialogueData.dialogues = new List<string>();

			for (int i = 0; i < dialogueEntity.Count; i++)
			{
				dialogueData.expressions.Add(dialogueEntity[i].f_Expression);
				dialogueData.dialogues.Add(dialogueEntity[i].f_LocalizedText);
			}

			inGameDialogueMngr.StartInGameDialogue(dialogueData, segmentEntity, dialMoment, selectedPinUI);
		}
	}
}
