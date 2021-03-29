using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelStatusData
{
	public LevelIDs levelID;
	public int locks;
	public bool dottedAnimPlayed;
	public bool unlocked;
	public bool unlockAnimPlayed;
	public bool completed;
	public bool pathDrawn;
}
