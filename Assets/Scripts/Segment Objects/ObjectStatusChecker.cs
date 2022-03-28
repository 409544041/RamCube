using Qbism.Serpent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Objects
{
	public class ObjectStatusChecker : MonoBehaviour
	{
		//Config parameters
		[SerializeField] SerpLogicRefHolder slRef;

		//States
		DialogueScripOb dialogueToPlay;
		SegmentRefHolder segInFocus;

		public void DecideOnDialogueToPlay(SegmentRefHolder segRef)
		{
			segInFocus = segRef;
			var eSegment = segRef.mSegments;
			dialogueToPlay = null;

			if (eSegment.f_Dialogues == null)
			{
				//TO DO: Remove dialogue button with juice
				return;
			}

			if (eSegment.f_Objects != null)
			{
				for (int i = 0; i < eSegment.f_Objects.Count; i++)
				{
					var obj = eSegment.f_Objects[i];
					bool objHasQuestMarker, objQuestGiven, objFound, objReturned;

					FetchObjValues(obj.f_GameplayData, out objHasQuestMarker, out objQuestGiven,
						out objFound, out objReturned);

					if (dialogueToPlay == null) dialogueToPlay = FetchCorrectDialogue(objHasQuestMarker,
						objQuestGiven, objFound, objReturned, obj, i);
				}
			}

			//TO DO: change up fluff dialogue options based on progression
			if (dialogueToPlay == null) dialogueToPlay =
					(DialogueScripOb)eSegment.f_Dialogues.f_FluffDialogues[0].f_FluffDialogue;
		}

		public void StartDialogue()
		{
			if (dialogueToPlay != null) segInFocus.dialogueStarter.StartDialogue(dialogueToPlay, null);
		}

		private DialogueScripOb FetchCorrectDialogue(bool hasQuestMarker, bool questGiven,
			bool found, bool returned, E_Objects eObj, int i)
		{
			if (returned) return null;

			if (hasQuestMarker)
			{
				if (found)
				{
					var dialogueEntity = E_AltReturnDialogues.FindEntity(entity =>
						entity.f_ForObject == eObj);
					return (DialogueScripOb)dialogueEntity.f_AltReturnDialogue;
				}
				else
				{
					var dialogueEntity = E_QuestDialogues.FindEntity(entity =>
						entity.f_ForObject == eObj);
					return (DialogueScripOb)dialogueEntity.f_QuestDialogue;
				}
			}

			else
			{
				if (found)
				{
					if (questGiven)
					{
						var dialogueEntity = E_ReturnDialogues.FindEntity(entity =>
							entity.f_ForObject == eObj);
						return (DialogueScripOb)dialogueEntity.f_ReturnDialogue;
					}
					else
					{
						var dialogueEntity = E_AltReturnDialogues.FindEntity(entity =>
							entity.f_ForObject == eObj);
						return (DialogueScripOb)dialogueEntity.f_AltReturnDialogue;
					}
				}
				else
				{
					if (questGiven)
					{
						var dialogueEntity = E_QuestWaitingDialogues.FindEntity(entity =>
							entity.f_ForObject == eObj);
						return (DialogueScripOb)dialogueEntity.f_WaitingDialogue;
					}
					else return null;
				}
			}
		}

		private void FetchObjValues(E_ObjectsGameplayData obj, out bool hasQuestMarker, out bool questGiven,
			out bool objFound, out bool objReturned)
		{
			hasQuestMarker = (obj.f_QuestMarkerShown && !obj.f_ObjectQuestGiven);
			questGiven = obj.f_ObjectQuestGiven;
			objFound = obj.f_ObjectFound;
			objReturned = obj.f_ObjectReturned;
		}

		public bool CheckIfObjectReturned(E_Objects obj)
		{
			if (obj.f_GameplayData.f_ObjectReturned == true) return true;
			else return false;
		}
	}
}
