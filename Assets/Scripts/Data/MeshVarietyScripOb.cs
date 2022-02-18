using UnityEngine;

[CreateAssetMenu(fileName = "Biome Visuals Scip Obj", menuName = "ScriptableObjects/Biome Variety")]

public class MeshVarietyScripOb : ScriptableObject
{
	//Config Parameters
	public meshVariety[] meshVarieties;

	[System.Serializable]
	public class meshVariety
	{
		public Biomes biome;
		public Mesh[] meshes;
	}
}
