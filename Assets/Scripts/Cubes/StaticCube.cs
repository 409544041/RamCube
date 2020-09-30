using System.Collections;
using System.Collections.Generic;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.Cubes
{
	public class StaticCube : MonoBehaviour
	{
		[SerializeField] Material fallingCubeMat;

		public void BecomeFallingCube(GameObject cube)
		{
			GetComponent<MeshRenderer>().material = fallingCubeMat;
			GetComponent<FloorCube>().type = CubeTypes.Falling;
		}

	}
}

