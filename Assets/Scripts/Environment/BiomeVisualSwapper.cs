using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Qbism.Environment
{
	public class BiomeVisualSwapper : MonoBehaviour
	{
		//Config parameters
		[SerializeField] GameObject[] meshParts;
		[SerializeField] BiomeVisualsScripOb biomeVarietySO;
		[SerializeField] bool recalculate = false;
		[SerializeField] bool isSkyBox = false, isVolume = false;
		//mesh and mat order should be same in scrip ob as in here

		//Cache
		ProgressHandler progHandler;

		//States
		E_Biome currentBiome;

		private void Awake() 
		{
			progHandler = FindObjectOfType<ProgressHandler>();	
		}

		private void Start()
		{
			FetchBiome();

			// if (isSkyBox) SetSkybox();
			// else if (isVolume) SetVolume();
			// else SetVisuals();
		}

		private void FetchBiome()
		{
			if (progHandler)
				currentBiome = progHandler.currentBiome;

			else
			{
				var bOverWriter = FindObjectOfType<BiomeOverwriter>();
				if (bOverWriter) currentBiome = bOverWriter.biomeOverwrite;
				else Debug.LogError("Progression Handler or Biome Overwriter not Linked. Setting first biome visuals");
			}
		}

		// private void SetVisuals()
		// {
		// 	foreach (var biomeVariety in biomeVarietySO.biomeVarieties)
		// 	{
		// 		if (biomeVariety.biome != currentBiome) continue;

		// 		for (int i = 0; i < meshParts.Length; i++)
		// 		{
		// 			MeshFilter mFilter = meshParts[i].GetComponent<MeshFilter>();

		// 			//Create new mats array
		// 			var newMats = new Material[biomeVariety.parts[i].mats.Length];

		// 			//Fill new mats array with correct mats from SO
		// 			for (int j = 0; j < newMats.Length; j++)
		// 			{
		// 				newMats[j] = biomeVariety.parts[i].mats[j];
		// 			}

		// 			meshParts[i].GetComponent<Renderer>().materials = newMats;

		// 			//Change mesh
		// 			if (biomeVariety.parts[i].mesh != null)
		// 			{
		// 				mFilter.mesh = biomeVariety.parts[i].mesh;
		// 			}

		// 			if (recalculate)
		// 			{
		// 				mFilter.mesh.RecalculateTangents();
		// 				mFilter.mesh.RecalculateNormals();
		// 			}
		// 		}
		// 	}
		// }

		// private void SetSkybox()
		// {
		// 	foreach (var biomeVariety in biomeVarietySO.biomeVarieties)
		// 	{
		// 		if (biomeVariety.biome != currentBiome) continue;

		// 		RenderSettings.skybox = biomeVariety.parts[0].mats[0];
		// 	}
		// }

		// private void SetVolume()
		// {
		// 	//Not sure how it works is but it works
		// 	UnityEngine.Rendering.VolumeProfile volProf = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
		// 	if (!volProf) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
		// 	UnityEngine.Rendering.Universal.Vignette vignette;
		// 	if (!volProf.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));

		// 	foreach (var biomeVariety in biomeVarietySO.biomeVarieties)
		// 	{
		// 		if (biomeVariety.biome != currentBiome) continue;

		// 		var vigColor = biomeVariety.parts[0].mats[0].color;

		// 		vignette.color.Override(vigColor);
		// 	}
		// }
	}
}
