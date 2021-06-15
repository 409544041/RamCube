using UnityEngine;

[CreateAssetMenu(fileName = "Biome Visuals Scip Obj", menuName = "ScriptableObjects/Biome Variety")]

public class BiomeVisualsScripOb : ScriptableObject
{
	//Config Parameters
	public biomeVariety[] biomeVarieties;

	[System.Serializable]
	public class biomeVariety
	{
		public Biomes biome;
		public objectPart[] parts;
	}

	[System.Serializable]
	public class objectPart
	{
		public Mesh mesh;
		public Material[] mats;
	}
}
