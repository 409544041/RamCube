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
		public CubeRefHolder refs;

		public void CastDottedLines(Vector3 laserPos, bool enableValue, TotemTypes type)
		{
			//Adjust to get a clean normalized direction
			var adjustedPos = new Vector3 (laserPos.x, transform.position.y, laserPos.z);
			var dir = (transform.position - adjustedPos).normalized;

			//z = y because the linerenderer is rotated 90
			float startX = .4f * -dir.x;
			float startY = .4f * -dir.z;
			float endX = .4f * dir.x;
			float endY = .4f * dir.z;

			LineRenderer dottedLine;
			if (type == TotemTypes.laser) dottedLine = refs.laserDottedLine;
			else dottedLine = refs.magnetDottedLine;

			dottedLine.SetPosition(0, new Vector3(startX, startY, transform.position.y));
			dottedLine.SetPosition(1, new Vector3(endX, endY, transform.position.y));
			dottedLine.enabled = enableValue;
		}

		public CubeTypes FetchType()
		{
			return type;
		}
	}
}
