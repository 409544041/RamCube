using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
	//Config parameters
	public NavMeshSurface surface;

	private void Start()
	{
		surface.BuildNavMesh();
	}

}