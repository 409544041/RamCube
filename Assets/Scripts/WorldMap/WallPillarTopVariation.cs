using System.Collections;
using System.Collections.Generic;
using Qbism.Environment;
using UnityEngine;

namespace Qbism.WorldMap
{
	[ExecuteInEditMode]
	public class WallPillarTopVariation : MonoBehaviour
	{
		//Config parameters
		public GameObject[] tops;
		[SerializeField] float spawnChance = .25f, secondSpawnChance = .5f;

		//States
		GameObject activeTop;

		private void DisableAllTopMeshes()
		{
			for (int i = 0; i < tops.Length; i++)
			{
				var meshes = tops[i].GetComponentsInChildren<MeshRenderer>();

				for (int j = 0; j < meshes.Length; j++)
				{
					meshes[j].enabled = false;
				}
			}
		}

		public void VaryTop(GameObject pillar)
		{
			DisableAllTopMeshes();

			float roll = Random.Range(0, 1);
			if (roll > spawnChance) return;

			int i = Random.Range(0, tops.Length);

			for (int j = 0; j < tops.Length; j++)
			{
				if (j == i)
				{
					var top = tops[j];
					activeTop = top;

					var activeMesh = EnableMesh(top);

					SetHeight(pillar, activeMesh.gameObject);

					if (tops[j].GetComponent<WallPillarSpawner>().pillarSize == WallPillarID.medium)
						VaryNextTop(activeMesh.gameObject);
				}
			}
		}

		private void VaryNextTop(GameObject prevTop)
		{
			float roll = Random.Range(0, 1);
			if (roll > secondSpawnChance) return;

			for (int i = 0; i < tops.Length; i++)
			{
				var top = tops[i];
				if (top != activeTop)
				{
					var activeMesh = EnableMesh(top);

					SetHeight(prevTop, activeMesh.gameObject);
				} 
			}
		}

		private MeshRenderer EnableMesh(GameObject top)
		{
			var meshes = top.GetComponentsInChildren<MeshRenderer>();
			int k = Random.Range(0, meshes.Length);
			MeshRenderer meshToReturn = null;

			for (int l = 0; l < meshes.Length; l++)
			{
				var mesh = meshes[k];
				if (l == k)
				{
					mesh.enabled = true;
					meshToReturn = mesh;
				}
			}

			return meshToReturn;
		}

		private void SetHeight(GameObject bottom, GameObject top)
		{
			top.transform.localPosition = new Vector3(top.transform.localPosition.x,
				0, top.transform.localPosition.z);

			var topBottomCon = top.GetComponent<WallPillarTopVarietyConnector>().bottomConnector;
			var bottomTopCon = bottom.GetComponent<WallPillarTopVarietyConnector>().topConnector;

			var topDelta = top.transform.position.y - topBottomCon.position.y;
			var variation = Random.Range(topDelta / 1.5f, topDelta / 3f);

			var topY = bottomTopCon.position.y + topDelta - variation;

			top.transform.position = new Vector3(top.transform.position.x, 
				topY, top.transform.position.z);
		}
	}
}
