using Qbism.WorldMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Dialogue
{
	public class MapDialogueMoment : MonoBehaviour
	{
		//Config parameters
		[SerializeField] bool onLevelReturn, onDragonScreenReturn;
		[SerializeField] InGameDialogueTrigger trigger;
		[SerializeField] M_Pin mPin;
		[SerializeField] MapLogicRefHolder mlRef;

		//States
		bool triggered = false;

		private void OnEnable()
		{
			foreach (var pin in mlRef.levelPins)
			{
				pin.pinRaiser.onRaisedCheckForDialogueTriggers += TriggerDialogue;
			}
		}

		private void TriggerDialogue(string incPin)
		{
			if (incPin != mPin.f_name || !onLevelReturn || triggered) return;
			triggered = true;
			trigger.TriggerInGameDialogue();
		}

		private void OnDisble()
		{
			foreach (var pin in mlRef.levelPins)
			{
				pin.pinRaiser.onRaisedCheckForDialogueTriggers -= TriggerDialogue;
			}
		}
	}
}
