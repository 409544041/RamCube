using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostCube : MonoBehaviour
{
	//Config parameters
	[SerializeField] float boostSpeed = 30f;
	[SerializeField] GameObject boostCollider;
	[SerializeField] Transform colliderSpawnPos;

	public void PrepareBoost(GameObject cube)
	{
		GameObject spawnedCollider = Instantiate(boostCollider, 
			colliderSpawnPos.position, transform.localRotation);

		spawnedCollider.transform.parent = cube.transform;
		spawnedCollider.GetComponent<Collider>().enabled = true;
		
		if(cube.GetComponent<PlayerCubeMover>()) StartCoroutine(Boost(cube));
		else if (cube.GetComponent<FeedForwardCube>()) StartCoroutine(BoostFF(cube));
	}

	private IEnumerator Boost(GameObject cube)
	{
		var mover = cube.GetComponent<PlayerCubeMover>();

		mover.input = false;
		cube.GetComponent<Rigidbody>().isKinematic = true;
		mover.isBoosting = true;

		var tileToDrop = mover.FetchCubeGridPos();

		while(mover.isBoosting)
		{
			cube.transform.position += 
				transform.TransformDirection(Vector3.forward) * boostSpeed * Time.deltaTime;
			yield return null;
		}

		mover.RoundPosition();
		mover.UpdatePositions();

		FindObjectOfType<CubeHandler>().DropTile(tileToDrop);

		mover.isBoosting = false;
		cube.GetComponent<Rigidbody>().isKinematic = false;
		
		mover.CheckFloorInNewPos();
	}

	private IEnumerator BoostFF(GameObject ffCube)
	{
		var ff = ffCube.GetComponent<FeedForwardCube>();

		ff.isBoosting = true;

		while(ff.isBoosting)
		{
			ffCube.transform.position +=
				transform.TransformDirection(Vector3.forward) * boostSpeed * Time.deltaTime;
			yield return null;
		}

		ff.RoundPosition();
		ff.isBoosting = false;
	}

}
