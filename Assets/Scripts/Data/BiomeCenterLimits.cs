using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Biome Center Limits", menuName = "Sriptable Objects/Biome Center Limits")]
public class BiomeCenterLimits : ScriptableObject
{
	//Config parameters
	public BiomeLimit[] biomeLimits = null;

	[System.Serializable]
	public class BiomeLimit
	{
		public Biomes biome;
		public float minZ = 0;
		public float maxZ = 0;
	}
}
