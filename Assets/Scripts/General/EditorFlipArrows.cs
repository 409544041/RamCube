using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using UnityEngine;

namespace Qbism.General
{
	[ExecuteInEditMode]
	public class EditorFlipArrows : MonoBehaviour
	{
		//Config parameters
		public CubeRefHolder refs;

		private void Start() 
		{
			var baseScale = Vector3.one;

			if (refs.turnCube.isLeftTurning)
			{
				refs.effectorFace.transform.localScale = SetLocalScale(-1, baseScale);
				refs.effectorShrinkingFace.transform.localScale = SetLocalScale(-1, baseScale);
				refs.turnCube.turnAxis = Vector3.down;
			}
			else
			{
				refs.effectorFace.transform.localScale = SetLocalScale(1, baseScale);
				refs.effectorShrinkingFace.transform.localScale = SetLocalScale(1, baseScale);
				refs.turnCube.turnAxis = Vector3.up;
			} 
		}

		private Vector3 SetLocalScale(int i, Vector3 baseScale)
		{
			Vector3 newScale = new Vector3(Mathf.Abs(baseScale.x) * i,
				baseScale.y, baseScale.z);
			return newScale;
		}
	}
}

