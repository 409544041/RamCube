using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloraSpawnChanceScripOb", menuName = "ScriptableObjects/Flora Spawn Chance")]
public class FloraSpawnChanceScripOb : ScriptableObject
{
	//Config parameters
	public int[] chanceWeight;
}