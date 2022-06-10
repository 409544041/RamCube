using System.Collections;
using System.Collections.Generic;
using Qbism.MoveableCubes;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.Cubes
{
	public class CubePositioner : MonoBehaviour
	{
		//Config parameters
		[SerializeField] PlayerRefHolder pRef;
		[SerializeField] CubeRefHolder cRef;

		public void RoundPosition()
		{
			float yPos;

			if (pRef != null || (cRef != null && cRef.movCube != null))
			{
				if (transform.position.y > .5f) yPos = .905f;
				else yPos = 0;
			}
			else yPos = Mathf.RoundToInt(transform.position.y);

			transform.position = new Vector3(Mathf.RoundToInt(transform.position.x),
				yPos, Mathf.RoundToInt(transform.position.z));

			if (cRef != null && cRef.movFaceMesh != null) cRef.movFaceMesh.transform.position =
				new Vector3(Mathf.RoundToInt(cRef.movFaceMesh.transform.position.x),
				yPos, Mathf.RoundToInt(cRef.movFaceMesh.transform.position.z));

			var eulers = transform.eulerAngles;
			eulers.x = Mathf.Round(eulers.x / 90) * 90;
			eulers.y = Mathf.Round(eulers.y / 90) * 90;
			eulers.z = Mathf.Round(eulers.z / 90) * 90;
			transform.eulerAngles = eulers;
		}

		public Vector2Int FetchGridPos()
		{
			Vector2Int roundedPos = new Vector2Int
				(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));

			return roundedPos;
		}
	}
}
