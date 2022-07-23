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
		DialogueData dialogueToPlay;
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
				var obj1 = eSegment.f_Objects[0];
				var obj2 = eSegment.f_Objects[1];
				bool obj1Found, obj1Returned, obj2Found, obj2Returned;

				FetchObjValues(obj1.f_GameplayData, out obj1Found, out obj1Returned);
				FetchObjValues(obj2.f_GameplayData, out obj2Found, out obj2Returned);

				if (dialogueToPlay == null) dialogueToPlay =
					FetchCorrectDialogue(obj1Found, obj1Returned, obj1,
					obj2Found, obj2Returned, obj2, eSegment);
			}

			//TO DO: switch this up to a robuster system allowing for multiple fluff texts?
			else if (eSegment.Entity == E_Segments.GetEntity(0))
			{
				var dialogueData = new DialogueData();
				dialogueData.charIndexes = new List<int>();
				dialogueData.expressions = new List<Expressions>();
				dialogueData.dialogues = new List<string>();
				dialogueData.firstExpr = eSegment.f_Dialogues.f_FluffDialogues[0].f_FirstExpr;
				var fluffDialogue = eSegment.f_Dialogues.f_FluffDialogues[0].f_FluffDialogue;
				
				for (int i = 0; i < fluffDialogue.Count; i++)
				{
					dialogueData.charIndexes.Add(fluffDialogue[i].f_CharIndex);
					dialogueData.expressions.Add(fluffDialogue[i].f_Expression);
					dialogueData.dialogues.Add(fluffDialogue[i].f_LocalizedText);
				}

				dialogueToPlay = dialogueData;
			}

			//TO DO: change up fluff dialogue options based on progression
		}

		public void StartDialogue()
		{
			if (dialogueToPlay != null)
			{
				segInFocus.dialogueStarter.StartDialogue(dialogueToPlay, null);

				if (checkObjectReturned)
				{
					var objGameplayEntity = E_ObjectsGameplayData.FindEntity(entity =>
					entity.f_Object == objToCheck);

					objGameplayEntity.f_ObjectReturned = true;
				}
			}
		}

		private DialogueData FetchCorrectDialogue(bool found1, bool returned1, E_Objects eObj1,
			bool found2, bool returned2, E_Objects eObj2, M_Segments eSegment)
		{
			var dialogueEntity = E_Dialogues.FindEntity(entity =>
				entity.f_Segment == eSegment.Entity);
			
			var dialogueData = new DialogueData();
			dialogueData.charIndexes = new List<int>();
			dialogueData.expressions = new List<Expressions>();
			dialogueData.dialogues = new List<string>();

			if (!found1)
			{
				dialogueData.firstExpr = dialogueEntity.f_FluffDialogues[0].f_FirstExpr;
				var fluffDialogue = dialogueEntity.f_FluffDialogues[0].f_FluffDialogue;

				for (int i = 0; i < fluffDialogue.Count; i++)
				{
					dialogueData.charIndexes.Add(fluffDialogue[i].f_CharIndex);
					dialogueData.expressions.Add(fluffDialogue[i].f_Expression);
					dialogueData.dialogues.Add(fluffDialogue[i].f_LocalizedText);
				}

				return dialogueData;
			}

			else if (found1 && !returned1)
			{
				objToCheck = eObj1;
				checkObjectReturned = true;

				dialogueData.firstExpr = dialogueEntity.f_ReturnDialogues[0].f_FirstExpr;
				var returnDialogue = dialogueEntity.f_ReturnDialogues[0].f_ReturnDialogue;

				for (int i = 0; i < returnDialogue.Count; i++)
				{
					dialogueData.charIndexes.Add(returnDialogue[i].f_CharIndex);
					dialogueData.expressions.Add(returnDialogue[i].f_Expression);
					dialogueData.dialogues.Add(returnDialogue[i].f_LocalizedText);
				}

				return dialogueData;
			}

			else if (!found2)
			{
				dialogueData.firstExpr = dialogueEntity.f_FluffDialogues[1].f_FirstExpr;
				var fluffDialogue = dialogueEntity.f_FluffDialogues[1].f_FluffDialogue;

				for (int i = 0; i < fluffDialogue.Count; i++)
				{
					dialogueData.charIndexes.Add(fluffDialogue[i].f_CharIndex);
					dialogueData.expressions.Add(fluffDialogue[i].f_Expression);
					dialogueData.dialogues.Add(fluffDialogue[i].f_LocalizedText);
				}

				return dialogueData;
			}

			else if (found2 && !returned2)
			{
				objToCheck = eObj2;
				checkObjectReturned = true;

				dialogueData.firstExpr = dialogueEntity.f_ReturnDialogues[1].f_FirstExpr;
				var returnDialogue = dialogueEntity.f_ReturnDialogues[1].f_ReturnDialogue;

				for (int i = 0; i < returnDialogue.Count; i++)
				{
					dialogueData.charIndexes.Add(returnDialogue[i].f_CharIndex);
					dialogueData.expressions.Add(returnDialogue[i].f_Expression);
					dialogueData.dialogues.Add(returnDialogue[i].f_LocalizedText);
				}

				return dialogueData;
			}

			else
			{
				dialogueData.firstExpr = dialogueEntity.f_FluffDialogues[2].f_FirstExpr;
				var fluffDialogue = dialogueEntity.f_FluffDialogues[2].f_FluffDialogue;

				for (int i = 0; i < fluffDialogue.Count; i++)
				{
					dialogueData.charIndexes.Add(fluffDialogue[i].f_CharIndex);
					dialogueData.expressions.Add(fluffDialogue[i].f_Expression);
					dialogueData.dialogues.Add(fluffDialogue[i].f_LocalizedText);
				}

				return dialogueData;
			}
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
