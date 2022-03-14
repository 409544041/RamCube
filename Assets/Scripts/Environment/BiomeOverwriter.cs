using System.Collections;
using System.Collections.Generic;
using Qbism.WorldMap;
using UnityEngine;

namespace Qbism.Environment
{
	public class BiomeOverwriter : MonoBehaviour
	{
		//Config parameters
		public Biomes biomeOverwrite;
		public bool respawnFloraVariety = false;
		public bool respawnWallPillarVariety = false;

		private void Awake()
		{
			AssignThisToSwappers();
			if (respawnWallPillarVariety) AssignThisToPillars();
			if (respawnFloraVariety) AssignThisToFloraAndDripSpawners();
		}

		private void AssignThisToSwappers()
		{
			var visualSwappers = FindObjectsOfType<BiomeVisualSwapper>();
			foreach (var swapper in visualSwappers)
			{
				swapper.bOverWriter = this;
			}
		}

		private void AssignThisToFloraAndDripSpawners()
		{
			var floraSpawners = FindObjectsOfType<FloraSpawner>();
			foreach (var spawner in floraSpawners)
			{
				spawner.bOverwriter = this;
			}

			var dripSpawners = FindObjectsOfType<DripSpawner>();
			foreach (var spawner in dripSpawners)
			{
				spawner.bOverwriter = this;
			}
		}

		private void AssignThisToPillars()
		{
			var spawners = FindObjectsOfType<WallPillarSpawner>();

			for (int i = 0; i < spawners.Length; i++)
			{
				spawners[i].bOverwriter = this;
			}
		}
	}
}
