using Qbism.Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class VarietyMaterialHandler : MonoBehaviour
	{
		//At map load, this script finds all biomeswappers and assigns
		//	.this to it's empty cache of this script
		//Biomeswappers check base mats of their meshes and create an empty array with length of mats
		//per basemat go over dictionary
		//if basemat == dic basemat and E_Biome = currentbiom then newMatArray[i] = dic new mat;
		//assign new mat array

		// rework visualswapper to work with above
		// delete old scriptable objects once everything works
		// change ref to used names in shader colors AFTER having put in the correct olors in the database

		public Dictionary<Material, Dictionary<Biomes, Material>> allMatsDic
			= new Dictionary<Material,Dictionary<Biomes, Material>>();

		private void Start()
		{
			for (int i = 0; i < E_BiomeMaterials.CountEntities; i++)
			{
				var biomeEntity = E_BiomeMaterials.GetEntity(i);
				var matData = (MatBiomeColorsScripOb) biomeEntity.f_BiomeMatData;
				var newMatDic = new Dictionary<Biomes, Material>();

				for (int j = 0; j < matData.biomeColors.Length; j++)
				{
					var newMat = biomeEntity.f_BaseMat;
					OverwriteMatColors(newMat, matData, j);

					newMatDic[matData.biomeColors[j].biome] = newMat;
				}

				allMatsDic[biomeEntity.f_BaseMat] = newMatDic;
			}
		}

		private void OverwriteMatColors(Material newMat, MatBiomeColorsScripOb matData, int j)
		{
			var colorData = matData.biomeColors[j];

			if (matData.overwriteBaseColor) newMat.SetColor("_BaseColor", colorData.baseColor);

			if (matData.overwriteShade)
			{
				newMat.SetColor("_ColorDim", colorData.shadeColor);
				newMat.SetColor("_ColorDimCurve", colorData.shadeColor);
				newMat.SetColor("_ColorDimSteps", colorData.shadeColor);
			}

			if (matData.overwriteExtraCell) newMat.SetColor("_ColorDimExtra", colorData.extraCellColor);

			if (matData.overwriteSpecular) newMat.SetColor("_FlatSpecularColor", colorData.specularColor);

			if (matData.overwriteRim) newMat.SetColor("_FlatRimColor", colorData.rimColor);

			if (matData.overwriteHeightGradient) newMat.SetColor("_ColorGradient", 
				colorData.heightGradientColor);

			if (matData.overwriteOutline) newMat.SetColor("_OutlineColor", colorData.outlineColor);

			if (matData.overwriteTint) newMat.SetColor("_Tint", colorData.tintColor);
		}

		public void FixMapLinks()
		{
			var swappers = FindObjectsOfType<BiomeVisualSwapper>();
			foreach (var swapper in swappers)
			{
				swapper.matHandler = this;
			}
		}
	}
}
