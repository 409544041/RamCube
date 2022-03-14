using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Environment
{
	[ExecuteInEditMode]
	public class DripSpawner : MonoBehaviour
	{
		//Config parameters
		[SerializeField] GameObject[] pillarSegments; //Make sure they're in order from left ot right

		//Cache
		public BiomeOverwriter bOverwriter { get; set; }

		//States
		DripHeightID prevHeight;

		private void Start()
		{
			if (bOverwriter && bOverwriter.respawnFloraVariety) GenerateDrips();
		}

		private void GenerateDrips()
		{
			for (int i = 0; i < pillarSegments.Length; i++)
			{
				DripIdentifier[] drips = pillarSegments[i].GetComponentsInChildren<DripIdentifier>();

				if (i == 0)
				{
					SpawnFirstDrip(drips);
				}
				else
				{
					List<DripIdentifier> fittingDrips = FetchFittingDrips(drips);
					SpawnDrips(drips, fittingDrips);
				}
			}
		}

		private void SpawnFirstDrip(DripIdentifier[] drips)
		{
			var dripToShow = drips[Random.Range(0, drips.Length)];

			for (int j = 0; j < drips.Length; j++)
			{
				var florSpawn = drips[j].GetComponent<FloraSpawner>();

				if (drips[j] == dripToShow)
				{
					drips[j].dripMesh.enabled = true;
					if (florSpawn) florSpawn.SpawnFlora();
					prevHeight = drips[j].endHeight;
				}
				else
				{
					drips[j].dripMesh.enabled = false;
					if (florSpawn) florSpawn.DespawnFlora();
				} 
			}
		}

		private List<DripIdentifier> FetchFittingDrips(DripIdentifier[] drips)
		{
			List<DripIdentifier> fittingDrips = new List<DripIdentifier>();

			for (int j = 0; j < drips.Length; j++)
			{
				if (drips[j].startHeight == prevHeight)
					fittingDrips.Add(drips[j]);
			}

			return fittingDrips;
		}

		private void SpawnDrips(DripIdentifier[] drips, List<DripIdentifier> fittingDrips)
		{
			var dripToShow = fittingDrips[Random.Range(0, fittingDrips.Count)];

			for (int k = 0; k < drips.Length; k++)
			{
				var florSpawn = drips[k].GetComponent<FloraSpawner>();
				
				if (drips[k] == dripToShow)
				{
					drips[k].dripMesh.enabled = true;
					if (florSpawn) florSpawn.SpawnFlora();
					prevHeight = drips[k].endHeight;
				}
				else
				{
					drips[k].dripMesh.enabled = false;
					if (florSpawn) florSpawn.DespawnFlora();
				} 
			}
		}
	}
}
