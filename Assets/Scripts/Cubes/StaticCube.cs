using System.Collections;
using System.Collections.Generic;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.Cubes
{
	public class StaticCube : MonoBehaviour
	{
		//Config parameters
		public Texture staticFaceTex = null;

		public void BecomeShrinkingCube(GameObject cube)
		{
			Material[] mats = GetComponentInChildren<MeshRenderer>().materials;
			mats[1].SetTexture("_BaseMap", null);
			GetComponent<FloorCube>().type = CubeTypes.Shrinking;
		}
	}
}

