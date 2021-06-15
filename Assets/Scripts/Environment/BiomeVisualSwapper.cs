using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using UnityEngine;

namespace Qbism.Environment
{
	public class BiomeVisualSwapper : MonoBehaviour
	{
		//Config parameters
		[SerializeField] GameObject[] meshParts;
		[SerializeField] BiomeVisualsScripOb biomeVarietySO;
		[SerializeField] bool recalculate = false;
		//mesh and mat order should be same in scrip ob as in here

		//Cache
		ProgressHandler progHandler;

		//States
		Biomes currentBiome = Biomes.Biome01;

		private void Awake() 
		{
			progHandler = FindObjectOfType<ProgressHandler>();	
		}

		private void Start() 
		{
			if (progHandler)
				currentBiome = progHandler.currentBiome;

			else
			{
				var bOverWriter = FindObjectOfType<BiomeOverwriter>();
				if (bOverWriter) currentBiome = bOverWriter.biomeOverwrite;
				else Debug.LogError("Progression Handler is not Linked. Setting first biome visuals");
			}

			SetVisuals();
		}

		private void SetVisuals()
		{
			foreach (var biomeVariety in biomeVarietySO.biomeVarieties)
			{
				if (biomeVariety.biome != currentBiome) continue;

				for (int i = 0; i < meshParts.Length; i++)
				{
					MeshFilter mFilter = meshParts[i].GetComponent<MeshFilter>();

					//Create new mats array
					var newMats = new Material[biomeVariety.parts[i].mats.Length];

					//Fill new mats array with correct mats from SO
					for (int j = 0; j < newMats.Length; j++)
					{
						newMats[j] = biomeVariety.parts[i].mats[j];
					}

					meshParts[i].GetComponent<Renderer>().materials = newMats;

					//Change mesh
					if (biomeVariety.parts[i].mesh != null)
					{
						mFilter.mesh = biomeVariety.parts[i].mesh;
					}

					if (recalculate)
					{
						mFilter.mesh.RecalculateTangents();
						mFilter.mesh.RecalculateNormals();
					}
				}
			}
		}
	}
}
