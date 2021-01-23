using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class FirstPinAlwaysActive : MonoBehaviour
	{
		//Cache
		LevelPin pin;

		private void Awake() 
		{
			pin = GetComponent<LevelPin>();
		}
		private void Start() 
		{
			ActivateFirstPinValues();
		}
		
		private void ActivateFirstPinValues()
		{
			ProgressHandler progHandler = FindObjectOfType<ProgressHandler>();
			if(!pin.unlocked)
			{
				pin.unlocked = true;
				progHandler.SetUnlockedStatus(pin.levelID, true);
			} 
			if(!pin.unlockAnimPlayed) 
			{
				pin.unlockAnimPlayed = true;
				progHandler.SetUnlockAnimPlayedStatus(pin.levelID, true);
			}

			GetComponent<ClickableObject>().canClick = true;
		}
	}
}

