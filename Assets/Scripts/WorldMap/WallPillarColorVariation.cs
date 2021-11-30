using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class WallPillarColorVariation : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Material[] mats;

		//States
		List<MeshRenderer> meshes = new List<MeshRenderer>();

		private void Start()
		{
			VaryColor();
		}

		private void VaryColor()
		{
			int i = Random.Range(0, mats.Length);
			var pillarSpawner = GetComponentInParent<WallPillarSpawner>();

			for (int j = 0; j < pillarSpawner.pillars.Length; j++)
			{
				var meshes = pillarSpawner.pillars[j].GetComponentsInChildren<MeshRenderer>();

				for (int k = 0; k < meshes.Length; k++)
				{
					meshes[k].material = mats[i];
				}
			}
		}
	}
}
