using Qbism.Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Cubes
{
	public class WrongFinishChecker : MonoBehaviour
	{
		//Config parameters
		[SerializeField] int wrongLimit, addToLimit;
		[SerializeField] string[] validPins;
		[SerializeField] InGameDialogueTrigger dialogueTrigger;
		[SerializeField] GameLogicRefHolder glRef;

		//States
		int wrongCount = 0;
		string currentPinString;
		bool validPin = false;

		private void Awake()
		{
			currentPinString = glRef.gcRef.persRef.progHandler.currentPin.f_name;

			foreach (var pin in validPins)
			{
				if (pin == currentPinString) validPin = true;
			}
		}

		public void AddToCount()
		{
			if (!validPin || !glRef.gcRef.persRef.switchBoard.showInGameHints) return;

			wrongCount++;
			
			if (wrongCount == wrongLimit)
			{
				dialogueTrigger.TriggerInGameDialogue(null, null);
				wrongCount = 0;
				wrongLimit += addToLimit;
			}
		}
	}
}
