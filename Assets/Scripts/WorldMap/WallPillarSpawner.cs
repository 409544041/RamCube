using System.Collections;
using System.Collections.Generic;
using Qbism.Environment;
using UnityEngine;

namespace Qbism.WorldMap
{
	[ExecuteInEditMode]
	public class WallPillarSpawner : MonoBehaviour
	{
		//Config parameters
		public WallPillarID pillarSize;
		public GameObject[] pillars;
		[SerializeField] bool varyRotation = true, varyHeight = false;
		[SerializeField] float[] spawnRotations;
		[SerializeField] Vector2 minMaxHeight;
		[SerializeField] WallPillarTopVariation topVariation;
		
		//Cache
		public BiomeOverwriter bOverwriter { get; set; }

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
				var pillar = pillars[j];
				var meshes = pillar.GetComponentsInChildren<MeshRenderer>();

				if (j == i)
				{
					for (int k = 0; k < meshes.Length; k++)
					{
						var mesh = meshes[k];
						mesh.enabled = true;

						if (varyHeight) VaryHeight(pillar);

						if (j == i && varyRotation)
						{
							RotateMeshes(mesh);
						}

						if (pillarSize == WallPillarID.large && topVariation != null)
							topVariation.VaryTop(pillar);
					}
				}

				else
				{
					for (int k = 0; k < meshes.Length; k++)
					{
						meshes[k].enabled = false;
					}
				}
			}
		}

		private void VaryHeight(GameObject pillar)
		{
			float yPos = Random.Range(minMaxHeight.x, minMaxHeight.y);
			
			pillar.transform.position = new Vector3(pillar.transform.position.x,
				yPos, pillar.transform.position.z);
		}

		private void RotateMeshes(MeshRenderer mesh)
		{
			int l = Random.Range(0, spawnRotations.Length);

			mesh.transform.rotation = Quaternion.Euler(0, spawnRotations[l], 0);
		}
	}
}
