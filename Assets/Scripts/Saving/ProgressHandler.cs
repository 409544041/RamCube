using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Saving
{
	public class ProgressHandler : MonoBehaviour
	{
		//States
		public LevelIDs currentLevelID { get; set; }

		public List<LevelStatusData> levelDataList;

		[System.Serializable]
		public class LevelStatusData
		{
			public LevelIDs levelID;
			public bool unlocked;
			public bool unlockAnimPlayed;
			public bool completed;
		}

		private void Awake() 
		{
			BuildLevelDataList();
			LoadProgHandlerData();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.L)) LoadProgHandlerData(); //TO DO: Temporary solution just to test. Fix later.
			if (Input.GetKeyDown(KeyCode.P)) WipeProgress();
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

		public void SetLevelToComplete(LevelIDs id, bool value)
		{
			foreach (LevelStatusData data in levelDataList)
			{
				if (data.levelID == id)
					data.completed = value;	
			}

			CheckLevelsToUnlock(id);
			SaveProgHandlerData();
		}

		private void CheckLevelsToUnlock(LevelIDs incomingID)
		{
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
					data.unlocked = value;
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

		public void SaveProgHandlerData()
		{
			SavingSystem.SaveProgHandlerData(this);
		}

		public void LoadProgHandlerData()
		{
			ProgHandlerData data = SavingSystem.LoadProgHandlerData();

			levelDataList = data.savedLevelDataList;
		}

		public void WipeProgress()
		{
			for (int i = 0; i < levelDataList.Count - 1; i++)
			{
				levelDataList[i].unlocked = false;
				levelDataList[i].unlockAnimPlayed = false;
				levelDataList[i].completed = false;				
			}
			SavingSystem.SaveProgHandlerData(this);
		}
	}
}
