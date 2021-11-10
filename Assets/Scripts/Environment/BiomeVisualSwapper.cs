using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using Qbism.WorldMap;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Qbism.Environment
{
	public class BiomeVisualSwapper : MonoBehaviour
	{
		//Config parameters
		[SerializeField] GameObject[] meshParts;
		[SerializeField] LineRenderer lineRender;
		[SerializeField] BiomeVisualsScripOb biomeVarietySO;
		[SerializeField] bool recalculate = false, checkBiomeLocally = false;
		[SerializeField] bool isSkyBox = false, isVolume = false, isLine = false;
		//mesh and mat order should be same in scrip ob as in here

		//States
		E_Biome currentBiome;

		private void Start()
		{
			FetchBiome();

			if (isSkyBox) SetSkybox();
			else if (isVolume) SetVolume();
			else SetVisuals();
		}

		private void FetchBiome()
		{
			if (!checkBiomeLocally)
			{
				ProgressHandler progHandler = FindObjectOfType<ProgressHandler>();

				if (progHandler)
					currentBiome = progHandler.currentBiome;

				else
				{
					var bOverWriter = FindObjectOfType<BiomeOverwriter>();
					if (bOverWriter) currentBiome = E_Biome.FindEntity(entity =>
						entity.f_name == bOverWriter.biomeOverwrite.ToString());
					else Debug.LogError("Progression Handler or Biome Overwriter not Linked. Setting first biome visuals");
				}
			}
			else
			{
				M_MapBiomeIdentifier m_MapBiome = GetComponentInParent<M_MapBiomeIdentifier>();
				if (m_MapBiome) currentBiome = m_MapBiome.f_Biome;
			}
		}

		private void SetVisuals()
		{
			foreach (var biomeVariety in biomeVarietySO.biomeVarieties)
			{
				if (biomeVariety.biome.ToString() != currentBiome.f_name.ToString()) continue;

				if (!lineRender)
				{
					for (int i = 0; i < meshParts.Length; i++)
					{
						MeshFilter mFilter = meshParts[i].GetComponent<MeshFilter>();

						Material[] newMats = CreateNewMatsArray(biomeVariety, i);

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
				else
				{
					Material[] newMats = CreateNewMatsArray(biomeVariety, 0);
					lineRender.materials = newMats;
				}
			}
		}

		private static Material[] CreateNewMatsArray(BiomeVisualsScripOb.biomeVariety biomeVariety, int i)
		{
			//Create new mats array
			var newMats = new Material[biomeVariety.parts[i].mats.Length];

			//Fill new mats array with correct mats from SO
			for (int j = 0; j < newMats.Length; j++)
			{
				newMats[j] = biomeVariety.parts[i].mats[j];
			}

			return newMats;
		}

		private void SetSkybox()
		{
			foreach (var biomeVariety in biomeVarietySO.biomeVarieties)
			{
				if (biomeVariety.biome.ToString() != currentBiome.f_name.ToString()) continue;

				RenderSettings.skybox = biomeVariety.parts[0].mats[0];
			}
		}

		private void SetVolume()
		{
			//Not sure how it works is but it works
			UnityEngine.Rendering.VolumeProfile volProf = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
			if (!volProf) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
			UnityEngine.Rendering.Universal.Vignette vignette;
			if (!volProf.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));

			foreach (var biomeVariety in biomeVarietySO.biomeVarieties)
			{
				if (biomeVariety.biome.ToString() != currentBiome.f_name.ToString()) continue;

				var vigColor = biomeVariety.parts[0].mats[0].color;

				vignette.color.Override(vigColor);
			}
		}
	}
}
