using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCeilingCheck : MonoBehaviour
{	
	//Config parameters
	[SerializeField] GameObject thruster;

	//Cache
	CubeMovement cubeMover;

	private void Awake()
	{
		cubeMover = GameObject.FindGameObjectWithTag("Player").GetComponent<CubeMovement>();
	}

	private void OnTriggerEnter(Collider other) 
	{
		if(thruster == null) Debug.LogError("No thruster assigned to FloorCelingCheck");
		if(other.gameObject == thruster) cubeMover.isInBoostPos = false;
	}

	private void OnTriggerExit(Collider other) 
	{
		if (thruster == null) Debug.LogError("No thruster assigned to FloorCelingCheck");
		if (other.gameObject == thruster) cubeMover.isInBoostPos = true;
	}
}
