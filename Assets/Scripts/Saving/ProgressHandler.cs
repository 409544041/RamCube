using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.WorldMap;
using UnityEngine;

namespace Qbism.Saving
{
	public class ProgressHandler : MonoBehaviour
	{
		//Cache
		SerpentProgress serpProg = null;
		PinSelectionTracker pinSelTrack = null;
		LevelPinInitiator initiator = null;

		//States
		public LevelIDs currentLevelID { get; set; }
		public bool currentHasSerpent { get ; set ; }
		public List<LevelStatusData> levelDataList;
		public List<LevelPin> levelPinList;

		//Actions, events, delegates etc
		public event Action<LevelPin> onSetUIComplete;
		public event Action<LevelPin, bool> onShowOrHideUI;
		public event Action<LevelIDs> onSelectPinUI;

		private void Awake() 
		{
			serpProg = GetComponent<SerpentProgress>();
			BuildLevelDataList();
			LoadProgHandlerData();
		}

		private void BuildLevelDataList()
		{
			foreach (LevelIDs ID in Enum.GetValues(typeof(LevelIDs)))
			{
				LevelStatusData newData = new LevelStatusData();
				newData.levelID = ID;
				levelDataList.Add(newData);
			}
		}

		public void FixDelegateLinks()
		{
			initiator = FindObjectOfType<LevelPinInitiator>();
			if (initiator != null) initiator.onPinInitation += InitiatePins;

			pinSelTrack = FindObjectOfType<PinSelectionTracker>();
			if (pinSelTrack != null) pinSelTrack.onSavedPinFetch += FetchCurrentPin;
		}

		public void BuildLevelPinList()
		{
			foreach (LevelPin pin in FindObjectsOfType<LevelPin>())
			{
				levelPinList.Add(pin);
			}

			levelPinList.Sort(PinsByID);
		}

		private int PinsByID(LevelPin A, LevelPin B)
		{
			if(A.levelID < B.levelID) return -1;
			else if(A.levelID > B.levelID) return 1;
			return 0;
		}

		public void InitiatePins() //Done every time world map is loaded
		{
			levelPinList.Clear();
			BuildLevelPinList();
			HandleLevelPins();
		}

		public void HandleLevelPins()
		{
			for (int i = 0; i < levelPinList.Count; i++)
			{
				if (levelPinList[i].levelID != levelDataList[i].levelID)
				{
					Debug.LogError("levelPinList " + levelPinList[i].levelID + " and levelDataList "
					+ levelDataList[i].levelID + " are not in the same order.");
					continue;
				}

				if(levelDataList[i].levelID == LevelIDs.a_01)
				{
					if (!levelDataList[i].unlocked) levelDataList[i].unlocked = true;
					if (!levelDataList[i].unlockAnimPlayed) levelDataList[i].unlockAnimPlayed = true; 
				}

				EditorSetPinValues pinValues = levelPinList[i].GetComponent<EditorSetPinValues>();
				LevelIDs unlock1ID = pinValues.levelUnlock_1;
				LevelIDs unlock2ID = pinValues.levelUnlock_2;
				LevelStatusData unlock1Data = FetchUnlockStatusData(unlock1ID);
				LevelStatusData unlock2Data = FetchUnlockStatusData(unlock2ID);
				
				var sheetLocks = pinValues.locks;
				var savedLocks = levelDataList[i].locks;
				var unlockAnimPlayed = levelDataList[i].unlockAnimPlayed;
				var unlocked = levelDataList[i].unlocked;
				var completed = levelDataList[i].completed;
				var pathDrawn = levelDataList[i].pathDrawn;
				
				levelPinList[i].justCompleted = false;
				bool lessLocks = (sheetLocks > savedLocks) && savedLocks != 0;

				levelPinList[i].CheckRaiseStatus(unlocked, unlockAnimPlayed);
				levelPinList[i].CheckPathStatus(unlock1Data, unlock2Data, completed);

				onSelectPinUI(currentLevelID);

				if(unlockAnimPlayed) onShowOrHideUI(levelPinList[i], true);
				else onShowOrHideUI(levelPinList[i], false);

				if (completed) onSetUIComplete(levelPinList[i]);

				if(unlocked && !unlockAnimPlayed)
				{
					levelPinList[i].InitiateRaising(unlocked, unlockAnimPlayed);
					levelDataList[i].unlockAnimPlayed = true;
				}

				if(completed && !pathDrawn)
				{
					levelPinList[i].justCompleted = true;
					levelDataList[i].pathDrawn = true;
				}
			}

			SaveProgData();
		}

		private LevelStatusData FetchUnlockStatusData(LevelIDs ID)
		{
			foreach (LevelStatusData data in levelDataList)
			{
				if (data.levelID == ID)
				{
					return data;
				} 
			}
			Debug.LogWarning("Can't fetch unlock data");
			return null;
		}

		public void SetLevelToComplete(LevelIDs id, bool value)
		{
			foreach (LevelStatusData data in levelDataList)
			{
				if (data.levelID == id)
					data.completed = value;	
			}

			CheckLevelsToUnlock(id);
		}

		private void CheckLevelsToUnlock(LevelIDs incomingID)
		{
			//Need to get this from sheets bc EditorSetPinValues is not available during gameplay
			var sheetID = QbismDataSheets.levelData[incomingID.ToString()];

			LevelIDs levelToUnlock_1 = LevelIDs.empty;
			LevelIDs levelToUnlock_2 = LevelIDs.empty;
			bool unlock1Found = false;
			bool unlock2Found = false;

			foreach (LevelIDs ID in Enum.GetValues(typeof(LevelIDs)))
			{
				if (ID.ToString() == sheetID.lVL_Unlock_1)
				{
					levelToUnlock_1 = ID;
					unlock1Found = true;
				}
				if (ID.ToString() == sheetID.lVL_Unlock_2)
				{
					levelToUnlock_2 = ID;
					unlock2Found = true;
				}
			}

			if(unlock1Found) SetUnlockedStatus(levelToUnlock_1, true);
			else levelToUnlock_1 = LevelIDs.empty;

			if(unlock2Found) SetUnlockedStatus(levelToUnlock_2, true);
			else levelToUnlock_2 = LevelIDs.empty;
		}

		public void SetUnlockedStatus(LevelIDs id, bool value)
		{
			foreach (LevelStatusData data in levelDataList)
			{
				if (data.levelID == id)
				{
					if(data.locks > 0) data.locks--;
					if(data.locks == 0) data.unlocked = value;
				}	
			}			
		}

		public void SetUnlockAnimPlayedStatus(LevelIDs id, bool value)
		{
			foreach (LevelStatusData data in levelDataList)
			{
				if (data.levelID == id)
					data.unlockAnimPlayed = value;
			}
		}

		public void SetCurrentData(LevelIDs id, bool serpent)
		{
			currentLevelID = id;
			foreach (LevelStatusData data in levelDataList)
			{
				if(data.levelID != id) continue;
				
				if(data.completed) currentHasSerpent = false;
				else currentHasSerpent = serpent;
			}
		}

		private void FetchCurrentPin()
		{
			foreach (LevelPin pin in levelPinList)
			{
				if (pin.levelID != currentLevelID) continue;
				pinSelTrack.selectedPin = pin;
				pinSelTrack.currentBiome = pin.GetComponent<EditorSetPinValues>().biome;
			}
		}

		public void SaveProgData()
		{
			SavingSystem.SaveProgData(this, serpProg);
		}

		public void LoadProgHandlerData()
		{
			ProgData data = SavingSystem.LoadProgData();

			levelDataList = data.savedLevelDataList;
			currentLevelID = data.currentLevelID;
		}

		public void WipeProgData() //TO DO: Make this debug only
		{
			for (int i = 0; i < levelDataList.Count; i++)
			{
				var sheetID = QbismDataSheets.levelData[levelDataList[i].levelID.ToString()];
				//If errors when deleting: Check if sheet.count and leveldatalist.count are similar and that sheet has an 'empty'
				levelDataList[i].locks = sheetID.locks;
				levelDataList[i].unlocked = false;
				levelDataList[i].unlockAnimPlayed = false;
				levelDataList[i].completed = false;		
				levelDataList[i].pathDrawn = false;		
			}

			currentLevelID = LevelIDs.a_01;

			for (int i = 0; i < serpProg.serpentDataList.Count; i++)
			{
				serpProg.serpentDataList[i] = false;
			}

			SavingSystem.SaveProgData(this, serpProg);
		}

		private void OnDisable()
		{
			if (initiator != null) initiator.onPinInitation -= InitiatePins;
			if (pinSelTrack != null) pinSelTrack.onSavedPinFetch -= FetchCurrentPin;
		}
	}
}
