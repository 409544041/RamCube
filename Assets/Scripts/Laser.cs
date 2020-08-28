using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
	//Config parameters
	[SerializeField] float distance;
	[SerializeField] GameObject laserBeam;

	//Cache
	CubeMovement cubeMover;

	private void Awake() 
	{
		cubeMover = GameObject.FindGameObjectWithTag("Player").GetComponent<CubeMovement>();	
	}

	private void Start() 
	{
		laserBeam.transform.localScale = new Vector3(1, distance, 1);
	}

	void Update()
	{
		FireLaserCast();
	}

	private void FireLaserCast()
	{
		if(!cubeMover.input) return;

		RaycastHit[] hits = SortedRaycasts();

		if(hits.Length == 0) return;
		if(hits[0].transform.forward == -transform.forward) Debug.Log("Shielded");
		else if(hits[0].transform.gameObject.tag == "Player")  Debug.Log("Destroyed");
	}

	private RaycastHit[] SortedRaycasts()
	{
		RaycastHit[] hits = Physics.RaycastAll(transform.position, 
			transform.TransformDirection(Vector3.forward), distance);

		Debug.DrawRay(transform.position, 
			transform.TransformDirection(Vector3.right), Color.red, distance);

		float[] hitDistances = new float[hits.Length];

		for (int hit = 0; hit < hitDistances.Length; hit++)
		{
			hitDistances[hit] = hits[hit].distance;
		}

		Array.Sort(hitDistances, hits);

		return hits;
	}
}
