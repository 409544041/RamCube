using System.Collections;
using System.Collections.Generic;
using Qbism.Environment;
using UnityEngine;

namespace Qbism.General
{
	[ExecuteInEditMode]
	public class WallPillarSpawner : MonoBehaviour
	{
		//Config parameters
		[SerializeField] GameObject[] pillars;
		[SerializeField] float[] spawnRotations;
		
		//Cache
		BiomeOverwriter bOverwriter;

		//States
		List<MeshRenderer> meshes = new List<MeshRenderer>();

		private void Awake()
		{
			bOverwriter = FindObjectOfType<BiomeOverwriter>();
		}

		private void Start()
		{
			if (bOverwriter && bOverwriter.respawnWallPillarVariety)
			{
				int i = Random.Range(0, pillars.Length);
				EnableCorrectPillar(i);
			}
		}

		private void EnableCorrectPillar(int i)
		{
			for (int j = 0; j < pillars.Length; j++)
			{
				var value = false;
				if (j == i) value = true;

				var meshes = pillars[j].GetComponentsInChildren<MeshRenderer>();

				for (int k = 0; k < meshes.Length; k++)
				{
					meshes[k].enabled = value;
					if (j == i)
					{
						RotateMeshes(meshes, k);
					}
				}
			}
		}

		private void RotateMeshes(MeshRenderer[] meshes, int k)
		{
			int l = Random.Range(0, spawnRotations.Length);

			meshes[k].transform.rotation = Quaternion.Euler
				(0, spawnRotations[l], 0);
		}
	}
}
