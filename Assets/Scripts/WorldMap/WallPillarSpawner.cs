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
		[SerializeField] bool varyRotation = true, varyHeight = false;
		[SerializeField] float[] spawnRotations;
		[SerializeField] Vector2 minMaxHeight;
		
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

					if (varyHeight) VaryHeight(pillars, j);

					if (j == i && varyRotation)
					{
						RotateMeshes(meshes, k);
					}
				}
			}
		}

		private void VaryHeight(GameObject[] pillars, int j)
		{
			float yPos = Random.Range(minMaxHeight.x, minMaxHeight.y);
			
			pillars[j].transform.position = new Vector3(pillars[j].transform.position.x,
				yPos, pillars[j].transform.position.z);
		}

		private void RotateMeshes(MeshRenderer[] meshes, int k)
		{
			int l = Random.Range(0, spawnRotations.Length);

			meshes[k].transform.rotation = Quaternion.Euler
				(0, spawnRotations[l], 0);
		}
	}
}
