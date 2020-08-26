using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCeilingCheck : MonoBehaviour
{	
	//Config parameters
	[SerializeField] GameObject thruster;

	//Cache
	GameObject cube;

	private void Awake() 
	{
		cube = GameObject.FindGameObjectWithTag("Player");
	}

	private void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject == thruster) cube.GetComponent<CubeMovement>().canBoost = false;
	}

	private void OnTriggerExit(Collider other) 
	{
		if (other.gameObject == thruster) cube.GetComponent<CubeMovement>().canBoost = true;
	}
}
