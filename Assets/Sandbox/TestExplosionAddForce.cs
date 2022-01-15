using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestExplosionAddForce : MonoBehaviour
{
	//Config parameters
	[SerializeField] float forceToAdd = 10, explRadius = 1;

	//Cache
	Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "LevelCompFollower")
		{
			//var force = transform.position - collision.transform.position * forceToAdd;
			//rb.AddExplosionForce(forceToAdd, collision.transform.position, explRadius);
			Vector3 force = (transform.position - collision.transform.position * forceToAdd).normalized;
			rb.AddForce(force);
			print("Triggering added force");
		}
	}
}
