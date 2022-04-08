using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FloraIdentifier : MonoBehaviour
{
	//Config parameters
	public FloraID floraType;
	public MeshRenderer[] floraMeshes;
	public Collider[] colliders;
	public NavMeshObstacle navMeshOb;

	//States
	public bool canSpawn { get; set; } = true;
}
