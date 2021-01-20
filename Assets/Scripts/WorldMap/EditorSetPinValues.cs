using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Qbism.WorldMap
{
	[ExecuteInEditMode]
	public class EditorSetPinValues : MonoBehaviour
	{
		//Config parameters
		public LevelIDs levelID;

		//States
		public int levelIndex { get; private set; }
		string levelName;
		LevelIDs levelUnlock_1;
		LevelIDs levelUnlock_2;
		bool hasSerpentSegment;

		private void Start()
		{
			var pinID = QbismDataSheets.levelData[levelID.ToString()];

			levelIndex = pinID.lVL_Index;
			levelName = pinID.level_Name;
			hasSerpentSegment = pinID.serp_Seg;

			foreach (LevelIDs ID in Enum.GetValues(typeof(LevelIDs)))
			{
				if(ID.ToString() == pinID.lVL_Unlock_1) levelUnlock_1 = ID;
				else 
				if (ID.ToString() == pinID.lVL_Unlock_2) levelUnlock_2 = ID;
				else levelUnlock_2 = LevelIDs.empty;
			}

		}
	}
}
