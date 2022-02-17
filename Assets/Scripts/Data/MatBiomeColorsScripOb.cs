using UnityEngine;

[CreateAssetMenu(fileName = "Mat Biome Colors Scrip Obj", menuName = "ScriptableObjects/Mat Biome Colors")]

public class MatBiomeColorsScripOb : ScriptableObject
{
	public bool overwriteBaseColor;
	public bool overwriteShade;
	public bool overwriteExtraCell;
	public bool overwriteSpecular;
	public bool overwriteRim;
	public bool overwriteHeightGradient;
	public bool overwriteOutline;
	public bool overwriteTint;
	public BiomeColors[] biomeColors;

	[System.Serializable]
	public class BiomeColors
	{
		public Biomes biome;
		public Color baseColor;
		public Color shadeColor;
		public Color extraCellColor;
		[ColorUsage(true, true)]
		public Color specularColor;
		[ColorUsage(true, true)]
		public Color rimColor;
		[ColorUsage(true, true)]
		public Color heightGradientColor;
		public Color outlineColor;
		public Color tintColor;
	}
}
