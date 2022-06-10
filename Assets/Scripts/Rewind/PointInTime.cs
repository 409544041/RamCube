using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Rewind
{
	public class PointInTime
	{
		public Vector3 position { get; set; }
		public Quaternion rotation { get; set; }
		public Vector3 scale { get; set; }
		public Quaternion faceRot { get; set; }
		public Vector3 faceScale { get; set; }

		public PointInTime(Vector3 objPosition, Quaternion objRotation, Vector3 objScale, 
			Quaternion faceRotation, Vector3 fScale)
		{
			position = objPosition;
			rotation = objRotation;
			scale = objScale;
			faceRot = faceRotation;
			faceScale = fScale;
		}

	}
}
