using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloraIdentifier : MonoBehaviour
{
	//Config parameters
	public FloraID floraType;
	public bool checkForCollision = false;

	//States
	public bool canSpawn { get; set; } = true;
}
