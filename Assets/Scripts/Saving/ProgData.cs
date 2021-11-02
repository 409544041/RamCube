using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using UnityEngine;
using BansheeGz.BGDatabase;

[System.Serializable]
public class ProgData
{
	//States
	public List<LevelStatusData> savedLevelData;
	public string savedCurrentPin;
	public List<bool> savedSerpentDataList;

	public ProgData(List<LevelStatusData> levelDataList, string currentPin,
		List<bool> serpentDataList)
	{
		savedLevelData = levelDataList;
		savedCurrentPin = currentPin;
		savedSerpentDataList = serpentDataList;
	}
}
