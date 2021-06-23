using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Cubes
{
	public class FloorCube : MonoBehaviour
	{
		//Config parameters
		public CubeTypes type = CubeTypes.Shrinking;
		public LineRenderer laserLine = null;

		//States
		public bool isFindable { get; set; } = true;

		public Vector2Int FetchGridPos()
		{
			Vector2Int roundedPos = new Vector2Int
				(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));

			return roundedPos;
		}

		public void CastDottedLines(Vector3 laserPos, bool enableValue)
		{
			//Adjust to get a clean normalized direction
			var adjustedPos = new Vector3 (laserPos.x, transform.position.y, laserPos.z);
			var dir = (transform.position - adjustedPos).normalized;

			//z = y because the linerenderer is rotated 90
			float startX = .4f * -dir.x;
			float startY = .4f * -dir.z;
			float endX = .4f * dir.x;
			float endY = .4f * dir.z;

			laserLine.SetPosition(0, new Vector3(startX, startY, transform.position.y));
			laserLine.SetPosition(1, new Vector3(endX, endY, transform.position.y));
			laserLine.enabled = enableValue;
		}

		public CubeTypes FetchType()
		{
			return type;
		}
	}
}
