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
		[SerializeField] MeshVarietyScripOb meshVarietySO;
		[SerializeField] MatBiomeColorsScripOb vignetteSO;
		[SerializeField] bool recalculate = false, checkBiomeLocally = false;
		[SerializeField] bool isSkyBox = false, isVolume = false;

		//Cache
		public VarietyMaterialHandler matHandler { get; set; }

		//States
		E_Biome currentBiome;

		private void Start()
		{
			FetchBiome();

			if (isSkyBox) SetSkyBox();
			else if (isVolume) SetVolume();
			else if (particle != null) SetParticle();
			else if (lineRender != null) SetLineRendererVisuals();
			else SwapVisuals();
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

		private void SwapVisuals()
		{
			for (int i = 0; i < meshParts.Length; i++)
			{
				var mFilter = meshParts[i].GetComponent<MeshFilter>();
				var oldMats = meshParts[i].GetComponent<Renderer>().sharedMaterials;
				var matsLength = oldMats.Length;
				var newMats = CreateNewMatsArray(matsLength, i, oldMats);

				if (newMats != null) meshParts[i].GetComponent<Renderer>().materials = newMats;

				if (meshVarietySO != null) SwapMesh(i, mFilter);
			}
		}

		private void SwapMesh(int i, MeshFilter mFilter)
		{
			MeshVarietyScripOb.biomeVariety biomeVariety = null;

			foreach (var variety in meshVarietySO.biomeVarieties)
			{
				if (variety.biome != currentBiome.f_BiomeEnum) continue;
				biomeVariety = variety;
			}

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

		private Material[] CreateNewMatsArray(int matsLength, int i, Material[] oldMats)
		{
			//Create new mats array
			var newMats = new Material[matsLength];

			//Fill new mats array with correct mats from SO
			for (int j = 0; j < newMats.Length; j++)
			{
				if (matHandler.allMatsDic.ContainsKey(oldMats[j]))
				{
					if (matHandler.allMatsDic[oldMats[j]].ContainsKey(currentBiome.f_BiomeEnum))
						newMats[j] = matHandler.allMatsDic[oldMats[j]][currentBiome.f_BiomeEnum];

					else return null;
				}
				else return null;
			}
			return newMats;
		}

		private void SetLineRendererVisuals()
		{
			Material[] oldMats = lineRender.materials;
			Material[] newMats = CreateNewMatsArray(oldMats.Length, 0, oldMats);
			if (newMats != null) lineRender.materials = newMats;
		}

		private void SetVolume()
		{
			//Not sure how it works is but it works
			UnityEngine.Rendering.VolumeProfile volProf = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
			if (!volProf) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
			UnityEngine.Rendering.Universal.Vignette vignette;
			if (!volProf.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));

			foreach (var entry in vignetteSO.biomeColors)
			{
				if (entry.biome != currentBiome.f_BiomeEnum) continue;
				vignette.color.Override(entry.baseColor);
			}
		}

		private void SetSkyBox()
		{
			var oldMat = RenderSettings.skybox;

			if (matHandler.allMatsDic.ContainsKey(oldMat))
			{
				if (matHandler.allMatsDic[oldMat].ContainsKey(currentBiome.f_BiomeEnum))
					RenderSettings.skybox = matHandler.allMatsDic[oldMat][currentBiome.f_BiomeEnum];
			}			
		}

		private void SetParticle()
		{
			var renderMod = particle.GetComponent<ParticleSystemRenderer>();
			var oldMat = renderMod.sharedMaterial;

			if (matHandler.allMatsDic.ContainsKey(oldMat))
			{
				if (matHandler.allMatsDic[oldMat].ContainsKey(currentBiome.f_BiomeEnum))
					renderMod.material = matHandler.allMatsDic[oldMat][currentBiome.f_BiomeEnum];
			}
		}
	}
}
