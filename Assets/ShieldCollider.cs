using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollider : MonoBehaviour
{
	//Cache
	GameObject cube;

	private void Awake()
	{
		cube = GameObject.FindGameObjectWithTag("Player");
	}

	private void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.tag == "Environment") 
			cube.GetComponent<CubeMovement>().isBoosting = false;
	}
}
