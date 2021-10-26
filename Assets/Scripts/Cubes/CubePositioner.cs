using System.Collections;
using System.Collections.Generic;
using Qbism.MoveableCubes;
using UnityEngine;

namespace Qbism.Cubes
{
	public class CubePositioner : MonoBehaviour
	{
		//Config parameters
		[SerializeField] bool isPlayer = false;

		public void RoundPosition()
		{
			float yPos;

			if (isPlayer)
			{
				if (transform.position.y > .5f) yPos = .9f;
				else yPos = 0;
			}
			else yPos = Mathf.RoundToInt(transform.position.y);

			transform.position = new Vector3(Mathf.RoundToInt(transform.position.x),
				yPos, Mathf.RoundToInt(transform.position.z));

			Quaternion rotation = Quaternion.Euler(Mathf.RoundToInt(transform.rotation.x),
				Mathf.RoundToInt(transform.rotation.y), Mathf.RoundToInt(transform.rotation.z));
		}

		public Vector2Int FetchGridPos()
		{
			Vector2Int roundedPos = new Vector2Int
				(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));

			return roundedPos;
		}
	}
}
