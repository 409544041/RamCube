using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterHandler : MonoBehaviour
{
	//Cache
	CubeMovement cubeMover;

	private void Awake() 
	{
		cubeMover = GameObject.FindGameObjectWithTag("Player").GetComponent<CubeMovement>();
	}

	private void OnEnable() 
	{
		if(cubeMover) cubeMover.onRaycast += FireRaycast;
	}

	public bool FireRaycast(Vector3 direction)
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, 
			transform.TransformDirection(direction), out hit, 1, 1 << 8))
			return false;
		else return true;
	}

	private void DisEnable()
	{
		if (cubeMover) cubeMover.onRaycast -= FireRaycast;
	}
}
