using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Qbism.WorldMap
{
	[ExecuteInEditMode]
	public class EditorSetPinValues : MonoBehaviour
	{
		//States
		public int levelIndex { get; private set; }
		string levelName;
		public LevelIDs levelUnlock_1 { get; set; }
		public LevelIDs levelUnlock_2 { get; set; }
		bool hasSerpentSegment;
		public GameObject pinUI { get; set; }

		private void Awake()
		{
			LevelIDs levelID = GetComponent<LevelPin>().levelID;
			var sheetID = QbismDataSheets.levelData[levelID.ToString()];

			levelIndex = sheetID.lVL_Index;
			levelName = sheetID.level_Name;
			hasSerpentSegment = sheetID.serp_Seg;

			bool unlock1Found = false;
			bool unlock2Found = false;

			foreach (LevelIDs ID in Enum.GetValues(typeof(LevelIDs)))
			{
				if (ID.ToString() == sheetID.lVL_Unlock_1)
				{
					levelUnlock_1 = ID;
					unlock1Found = true;
				} 
				if (ID.ToString() == sheetID.lVL_Unlock_2)
				{
					levelUnlock_2 = ID;
					unlock2Found = true;
				}
			}

			if(!unlock1Found) levelUnlock_1 = LevelIDs.empty;
			if(!unlock2Found) levelUnlock_2 = LevelIDs.empty;
		}
	}
}
