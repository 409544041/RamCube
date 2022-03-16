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
			if (refs.turnCube.isLeftTurning)
			{
				refs.mesh.transform.localScale = new Vector3 (Mathf.Abs(refs.mesh.transform.localScale.x) * -1,
					refs.mesh.transform.localScale.y, refs.mesh.transform.localScale.z);
				refs.turnCube.turnAxis = Vector3.down;
			}
			else
			{
				refs.mesh.transform.localScale = new Vector3(Mathf.Abs(refs.mesh.transform.localScale.x),
					refs.mesh.transform.localScale.y, refs.mesh.transform.localScale.z);
				refs.turnCube.turnAxis = Vector3.up;
			} 
		}
	}
}

