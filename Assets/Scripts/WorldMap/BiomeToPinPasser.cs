using Qbism.Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class BiomeToPinPasser : MonoBehaviour
	{
		private void Awake()
		{
			E_Biome biome = (E_Biome)GetComponent<M_Biome>().Entity;

			var visSwappers = GetComponentsInChildren<BiomeVisualSwapper>();

			foreach (var swapper in visSwappers)
			{
				swapper.currentBiome = biome;
			}
		}
	}
}
