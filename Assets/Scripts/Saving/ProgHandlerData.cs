using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using UnityEngine;

[System.Serializable]
public class ProgHandlerData
{
	//States
	public List<ProgressHandler.LevelStatusData> savedLevelDataList;

	public ProgHandlerData(ProgressHandler progHandler)
	{
		savedLevelDataList = progHandler.levelDataList;
	}
}
