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
			if (respawnWallPillarVariety)
			{
				var spawners = FindObjectsOfType<WallPillarSpawner>();

				for (int i = 0; i < spawners.Length; i++)
				{
					spawners[i].bOverwriter = this;
				}
			}	
		}
	}
}
