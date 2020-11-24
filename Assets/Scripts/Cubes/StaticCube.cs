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

		public void BecomeFallingCube(GameObject cube)
		{
			Material[] mats = GetComponent<Renderer>().materials;
			mats[2].SetTexture("_BaseMap", null);
			GetComponent<FloorCube>().type = CubeTypes.Shrinking;
		}

	}
}

