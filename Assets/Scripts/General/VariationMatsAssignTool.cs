using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class VariationMatsAssignTool : MonoBehaviour
{
	//Config parameters
	[SerializeField] MatVarCombo[] combo;

	[System.Serializable]
	public class MatVarCombo
	{
		public Material[] mats;
		public MatBiomeColorsScripOb scripOb;
	}

	private void Start()
	{
		foreach (var duo in combo)
		{
			var so = duo.scripOb;

			for (int i = 0; i < duo.mats.Length; i++)
			{
				var bc = duo.scripOb.biomeColors[i];
				var mat = duo.mats[i];

				if (so.overwriteBaseColor) bc.baseColor = mat.GetColor("_BaseColor");

				if (so.overwriteShade) bc.shadeColor = mat.GetColor("_ColorDim");

				if (so.overwriteExtraCell) bc.extraCellColor = mat.GetColor("_ColorDimExtra");

				if (so.overwriteSpecular) bc.specularColor = mat.GetColor("_FlatSpecularColor");

				if (so.overwriteRim) bc.rimColor = mat.GetColor("_FlatRimColor");

				if (so.overwriteHeightGradient) bc.heightGradientColor = mat.GetColor("_ColorGradient");

				if (so.overwriteOutline) bc.outlineColor = mat.GetColor("_OutlineColor");

				if (so.overwriteTint) bc.tintColor = mat.GetColor("_Tint");
			}
		}	
	}
}
