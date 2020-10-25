using System.Collections;
using System.Collections.Generic;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.Cubes
{
	public class StaticCube : MonoBehaviour
	{
		//Config parameters
		public Material staticCubeMat = null;
		[SerializeField] Material shrinkingCubeMat = null;

		public void BecomeFallingCube(GameObject cube)
		{
			GetComponent<MeshRenderer>().material = shrinkingCubeMat;
			GetComponent<FloorCube>().type = CubeTypes.Shrinking;
		}

	}
}

