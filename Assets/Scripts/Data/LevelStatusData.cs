using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelStatusData
{
	public string pin;
	public int locksLeft;
	public bool dottedAnimPlayed;
	public bool unlocked;
	public bool unlockAnimPlayed;
	public bool completed;
	public bool pathDrawn;
}
