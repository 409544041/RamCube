using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollider : MonoBehaviour
{
	//Cache
	CubeMovement cubeMover;

	private void Awake()
	{
		cubeMover = GameObject.FindGameObjectWithTag("Player").GetComponent<CubeMovement>();
	}

	private void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.tag == "Environment") 
		{
			cubeMover.isBoosting = false;
			Destroy(gameObject);
		}
	}
}
