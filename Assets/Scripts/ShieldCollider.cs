using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollider : MonoBehaviour
{
	//Cache
	PlayerCubeMover cubeMover;

	private void Awake()
	{
		cubeMover = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCubeMover>();
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
