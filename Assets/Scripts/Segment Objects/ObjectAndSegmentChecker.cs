using Qbism.Serpent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGDatabase;

namespace Qbism.Objects
{
	public class ObjectAndSegmentChecker : MonoBehaviour
	{
		//Config parameters
		[SerializeField] SerpLogicRefHolder slRef;

		//States
		DialogueScripOb dialogueToPlay;
		public SegmentRefHolder segInFocus { get; private set; }
		bool checkObjectReturned;
		E_Objects objToCheck;

		public void DecideOnDialogueToPlay(SegmentRefHolder segRef)
		{
			segInFocus = segRef;
			var eSegment = segRef.mSegments;
			dialogueToPlay = null;
			checkObjectReturned = false;

			if (eSegment.f_Dialogues == null)
			{
				slRef.serpScreenUIHandler.FadeOutSpeechElement();
				return;
			}
			else slRef.serpScreenUIHandler.ResetSpeechElementColor();

			if (eSegment.f_Objects != null)
			{
				for (int i = 0; i < eSegment.f_Objects.Count; i++)
				{
					var obj = eSegment.f_Objects[i];
					bool objFound, objReturned;

					FetchObjValues(obj.f_GameplayData, out objFound, out objReturned);

					if (dialogueToPlay == null) dialogueToPlay = 
							FetchCorrectDialogue(objFound, objReturned, obj, i);
				}
			}

			//TO DO: change up fluff dialogue options based on progression
			if (dialogueToPlay == null) dialogueToPlay =
					(DialogueScripOb)eSegment.f_Dialogues.f_FluffDialogues[0].f_FluffDialogue;
		}

		public void StartDialogue()
		{
			if (dialogueToPlay != null)
			{
				segInFocus.dialogueStarter.StartDialogue(dialogueToPlay, null);

				var objGameplayEntity = E_ObjectsGameplayData.FindEntity(entity =>
					entity.f_Object == objToCheck);

				if (checkObjectReturned) objGameplayEntity.f_ObjectReturned = true;
			}
		}

		private DialogueScripOb FetchCorrectDialogue(bool found, bool returned, E_Objects eObj, int i)
		{
			if (returned) return null;

			if (!found)
			{
				var dialogueEntity = E_QuestDialogues.FindEntity(entity =>
						entity.f_ForObject == eObj);
				objToCheck = eObj;
				return (DialogueScripOb)dialogueEntity.f_QuestDialogue;
			}
			else if (found && !returned)
			{
				var dialogueEntity = E_ReturnDialogues.FindEntity(entity =>
							entity.f_ForObject == eObj);
				objToCheck = eObj;
				checkObjectReturned = true;
				return (DialogueScripOb)dialogueEntity.f_ReturnDialogue;
			}
			else return null;
		}

		private void FetchObjValues(E_ObjectsGameplayData obj, out bool objFound, out bool objReturned)
		{
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
