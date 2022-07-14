using Qbism.General;
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
		[SerializeField] FocusCircleTrigger circleTrigger;
		[SerializeField] MapLogicRefHolder mlRef;

		//States
		bool triggered = false;
		LevelPinUI selectedPinUI;

		private void OnEnable()
		{
			foreach (var pin in mlRef.levelPins)
			{
				pin.pinUIJuicer.onRaisedCheckForDialogueTriggers += TriggerDialogue;
			}
		}

		private void TriggerDialogue(string incPin)
		{
			if (incPin != mPin.f_name || !onLevelReturn || triggered ||
				!mlRef.mcRef.persRef.switchBoard.triggerMapDialogue) return;

			triggered = true;
			mlRef.screenStateMngr.mapScreenState.AddRemoveNotAllowingInput(1);
			selectedPinUI = mlRef.pinTracker.selectedPin.pinUI;
			trigger.TriggerInGameDialogue(this, selectedPinUI);
		}

		public void PostDialogue()
		{
			if (circleTrigger != null) circleTrigger.TriggerFocus(selectedPinUI);
			else
			{
				mlRef.screenStateMngr.mapScreenState.AddRemoveNotAllowingInput(-1);
				mlRef.pinTracker.SelectPin(selectedPinUI);
			}
		}

		private void OnDisble()
		{
			foreach (var pin in mlRef.levelPins)
			{
				pin.pinUIJuicer.onRaisedCheckForDialogueTriggers -= TriggerDialogue;
			}
		}
	}
}
