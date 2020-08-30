using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Laser : MonoBehaviour
{
	//Config parameters
	[SerializeField] float distance = 1;
	[SerializeField] GameObject laserBeam = null;
	[SerializeField] Transform laserOrigin = null;

	//Cache
	CubeMovement cubeMover;

	private void Awake() 
	{
		cubeMover = GameObject.FindGameObjectWithTag("Player").GetComponent<CubeMovement>();	
	}

	private void Start() 
	{
		laserBeam.transform.localScale = new Vector3(1, distance, 1);
		laserBeam.transform.localPosition = new Vector3(0, 0, .5f * distance);
	}

	void FixedUpdate()
	{
		FireLaserCast();
	}

	private void FireLaserCast()
	{
		if(cubeMover.input || cubeMover.isBoosting) 
		{
			RaycastHit[] hits = SortedRaycasts();

			if (hits.Length == 0) return;

			if (Mathf.Approximately(Vector3.Dot(cubeMover.transform.forward,
				transform.forward), -1)) Debug.Log("Shielded");

			else SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	private RaycastHit[] SortedRaycasts()
	{
		RaycastHit[] hits = Physics.RaycastAll(laserOrigin.position,
			transform.TransformDirection(Vector3.forward), distance, 1 << 9, QueryTriggerInteraction.Ignore);

		Debug.DrawRay(laserOrigin.position, 
			transform.TransformDirection(Vector3.forward), Color.red, distance);

		float[] hitDistances = new float[hits.Length];

		for (int hit = 0; hit < hitDistances.Length; hit++)
		{
			hitDistances[hit] = hits[hit].distance;
		}

		Array.Sort(hitDistances, hits);

		return hits;
	}
}
