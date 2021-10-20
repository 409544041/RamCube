using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using UnityEngine;

namespace Qbism.General
{
	[ExecuteInEditMode]
	public class EditorFlipArrows : MonoBehaviour
	{ 
		//Cache
		TurningCube cube;

		private void Awake() 
		{
			cube = GetComponent<TurningCube>();
		}

		private void Start() 
		{
			MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();

			if (cube.isLeftTurning)
			{
				renderer.transform.localScale = new Vector3 (Mathf.Abs(renderer.transform.localScale.x) * -1,
					renderer.transform.localScale.y, renderer.transform.localScale.z);
				cube.turnAxis = Vector3.down;
			}
			else
			{
				renderer.transform.localScale = new Vector3(Mathf.Abs(renderer.transform.localScale.x),
					renderer.transform.localScale.y, renderer.transform.localScale.z);
				cube.turnAxis = Vector3.up;
			} 
		}
	}
}

