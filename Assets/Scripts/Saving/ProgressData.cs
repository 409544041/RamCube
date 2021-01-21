using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using UnityEngine;

[System.Serializable]
public class ProgressData
{
	//States
	public Dictionary<LevelIDs, bool> savedLevelComleteDic;
	public Dictionary<LevelIDs, bool> savedLevelUnlockDic;

	public ProgressData(ProgressHandler progHandler)
	{
		savedLevelComleteDic = progHandler.levelCompleteDic;
		savedLevelUnlockDic = progHandler.levelUnlockDic;
	}
}
