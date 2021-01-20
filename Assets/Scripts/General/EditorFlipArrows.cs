﻿using System.Collections;
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
				renderer.transform.localScale = new Vector3 (-1, 1 ,1);
				cube.turnAxis = Vector3.down;
			}
			else
			{
				renderer.transform.localScale = new Vector3(1, 1, 1);
				cube.turnAxis = Vector3.up;
			} 
		}
	}
}
