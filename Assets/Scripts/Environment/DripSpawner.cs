using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Environment
{
	[ExecuteAlways]
	public class DripSpawner : MonoBehaviour
	{
		//Config parameters
		public PillarSegment[] pillarSegments; //Make sure they're in order from left ot right
		[SerializeField] bool includeLowDrips = true, connectAllAround = false;

		//Cache
		public BiomeOverwriter bOverwriter { get; set; }

		//States
		DripHeightID prevHeight;
		DripHeightID firstStartHeight;

		private void Start()
		{
			if (bOverwriter != null && bOverwriter.respawnFloraVariety) GenerateDrips();
		}

		private void GenerateDrips()
		{
			for (int i = 0; i < pillarSegments.Length; i++)
			{
				DripIdentifier[] drips = pillarSegments[i].dripsToSpawn;

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
			DripIdentifier dripToShow = null;
			if (includeLowDrips) dripToShow = drips[Random.Range(0, drips.Length)];
			else
			{
				List<DripIdentifier> possibleDrips = new List<DripIdentifier>();
				foreach (var drip in drips)
				{
					if (drip.startHeight == DripHeightID.high && drip.endHeight == DripHeightID.high)
						possibleDrips.Add(drip);
				}

				dripToShow = possibleDrips[Random.Range(0, possibleDrips.Count)];
			}

			for (int j = 0; j < drips.Length; j++)
			{
				if (drips[j] == dripToShow)
				{
					drips[j].dripMesh.enabled = true;
					if (drips[j].florSpawner != null) drips[j].florSpawner.SpawnFlora();
					prevHeight = drips[j].endHeight;
					firstStartHeight = drips[j].startHeight;
				}
				else
				{
					drips[j].dripMesh.enabled = false;
					if (drips[j].florSpawner != null) drips[j].florSpawner.DespawnFlora();
				} 
			}
		}

		private List<DripIdentifier> FetchFittingDrips(DripIdentifier[] drips)
		{
			List<DripIdentifier> fittingDrips = new List<DripIdentifier>();

			if (includeLowDrips)
			{
				for (int i = 0; i < drips.Length; i++)
				{
					if (connectAllAround && i == drips.Length - 1)
					{
						if (drips[i].startHeight == prevHeight && drips[i].endHeight == firstStartHeight)
							fittingDrips.Add(drips[i]);
					}
					else
					{
						if (drips[i].startHeight == prevHeight)
							fittingDrips.Add(drips[i]);
					}
				}

				return fittingDrips;
			}
			else
			{
				foreach (var drip in drips)
				{
					if (drip.startHeight == DripHeightID.high && drip.endHeight == DripHeightID.high)
						fittingDrips.Add(drip);
				}

				return fittingDrips;
			}
		}

		private void SpawnDrips(DripIdentifier[] drips, List<DripIdentifier> fittingDrips)
		{
			var dripToShow = fittingDrips[Random.Range(0, fittingDrips.Count)];

			for (int k = 0; k < drips.Length; k++)
			{
				var florSpawn = drips[k].florSpawner;
				
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
