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
		public bool isCompleted { get; set; }


		// public Dictionary<LevelIDs, bool> levelCompleteDic =
		// 		new Dictionary<LevelIDs, bool>();

		// public Dictionary<LevelIDs, bool> levelUnlockDic =
		// 		new Dictionary<LevelIDs, bool>();

		// public Dictionary<LevelIDs, Dictionary<bool, Dictionary<bool, bool>>> levelStatusDic = 
		// 	new Dictionary<LevelIDs, Dictionary<bool, Dictionary<bool, bool>>>();
		//1st bool = completed, 2nd bool = unlocked, 3rd bool = unlock animation done

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
				levelDataList.Add(newData); //Find a way to see this list in inspector
			}
		}

		public void SetLevelToComplete()
		{
			isCompleted = true;
			foreach (LevelStatusData data in levelDataList)
			{
				if (data.levelID == currentLevelID)
					data.completed = true;	
			}

			CheckLevelsToUnlock();
			SaveProgHandlerData();
		}

		private void CheckLevelsToUnlock()
		{
			var sheetID = QbismDataSheets.levelData[currentLevelID.ToString()];

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

			SetUnlockedStatus(unlock1Found, levelToUnlock_1);
			SetUnlockedStatus(unlock2Found, levelToUnlock_2);
		}

		private void SetUnlockedStatus(bool found, LevelIDs levelToUnlock)
		{
			if (found)
			{
				foreach (LevelStatusData data in levelDataList)
				{
					if (data.levelID == levelToUnlock)
						data.unlocked = true;
				}
			}
			else levelToUnlock = LevelIDs.empty;
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
