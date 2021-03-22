using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using UnityEngine;

[System.Serializable]
public class ProgData
{
	//States
	public List<LevelStatusData> savedLevelDataList;
	public LevelIDs currentLevelID;
	public List<bool> savedSerpentDataList;

	public ProgData(ProgressHandler progHandler, SerpentProgress serpProg)
	{
		savedLevelDataList = progHandler.levelDataList;
		currentLevelID = progHandler.currentLevelID;
		savedSerpentDataList = serpProg.serpentDataList;
	}
}
