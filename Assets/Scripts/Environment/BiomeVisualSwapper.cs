using System.Collections;
using System.Collections.Generic;
using Qbism.General;
using Qbism.Saving;
using UnityEngine;

namespace Qbism.Environment
{
	public class BiomeVisualSwapper : MonoBehaviour
	{
		//Config parameters
		[SerializeField] GameObject[] meshParts;
		[SerializeField] LineRenderer lineRender;
		[SerializeField] ParticleSystem particle;
		[SerializeField] BiomeVisualsScripOb biomeVarietySO;
		[SerializeField] bool recalculate = false, checkBiomeLocally = false;
		[SerializeField] bool isSkyBox = false, isVolume = false, isLine = false, isParticle = false;
		//mesh and mat order should be same in scrip ob as in here

		//Cache
		public VarietyMaterialHandler matHandler { get; set; }

		//States
		E_Biome currentBiome;

		private void Start()
		{
			FetchBiome();

			foreach (var biomeVariety in biomeVarietySO.biomeVarieties)
			{
				if (biomeVariety.biome.ToString() != currentBiome.f_name.ToString()) continue;

				if (isSkyBox) RenderSettings.skybox = biomeVariety.parts[0].mats[0];
				else if (isVolume) SetVolume(biomeVariety);
				else if (isParticle) SetParticle(biomeVariety);
				else SetVisuals(biomeVariety);
			}
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
				var m_biomeID = GetComponentInParent<M_BiomeIdentifier>();
				if (m_biomeID) currentBiome = m_biomeID.f_Biome;
			}
		}

		private void SetVisuals(BiomeVisualsScripOb.biomeVariety biomeVariety)
		{
			if (!lineRender)
			{
				for (int i = 0; i < meshParts.Length; i++)
				{
					MeshFilter mFilter = meshParts[i].GetComponent<MeshFilter>();

					Material[] oldMats = meshParts[i].GetComponent<Renderer>().materials;
					Material[] newMats = CreateNewMatsArray(biomeVariety, i, oldMats);

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
				Material[] oldMats = lineRender.materials;
				Material[] newMats = CreateNewMatsArray(biomeVariety, 0, oldMats);
				lineRender.materials = newMats;
				//Create new version of create new array for this
			}
		}

		private Material[] CreateNewMatsArray(BiomeVisualsScripOb.biomeVariety biomeVariety, int i,
			Material[] oldMats)
		{
			//Create new mats array
			var newMats = new Material[biomeVariety.parts[i].mats.Length];

			//Fill new mats array with correct mats from SO
			for (int j = 0; j < newMats.Length; j++)
			{
				newMats[j] = matHandler.allMatsDic[oldMats[j]][currentBiome.f_BiomeEnum];
			}

			return newMats;
		}

		private void SetVolume(BiomeVisualsScripOb.biomeVariety biomeVariety)
		{
			//Not sure how it works is but it works
			UnityEngine.Rendering.VolumeProfile volProf = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
			if (!volProf) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
			UnityEngine.Rendering.Universal.Vignette vignette;
			if (!volProf.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));

			var vigColor = biomeVariety.parts[0].mats[0].color;
			vignette.color.Override(vigColor);
		}

		private void SetParticle(BiomeVisualsScripOb.biomeVariety biomeVariety)
		{
			var renderMod = particle.GetComponent<ParticleSystemRenderer>();
			renderMod.material = biomeVariety.parts[0].mats[0];
		}
	}
}
