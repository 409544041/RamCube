using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class WallPillarColorVariation : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Material[] mats;
		[SerializeField] WallPillarTopVariation topVariation;

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

			VaryTopVarietyColor(i);
		}

		private void VaryTopVarietyColor(int i)
		{
			if (topVariation != null)
			{
				for (int j = 0; j < topVariation.tops.Length; j++)
				{
					var meshes = topVariation.tops[j].GetComponentsInChildren<MeshRenderer>();
					
					for (int k = 0; k < meshes.Length; k++)
					{
						meshes[k].material = mats[i];
					}
				}
			}
		}
	}
}
