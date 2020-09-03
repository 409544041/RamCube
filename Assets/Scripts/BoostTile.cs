using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostTile : MonoBehaviour
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
		
		StartCoroutine(Boost(cube));
	}

	private IEnumerator Boost(GameObject cube)
	{
		var mover = cube.GetComponent<CubeMovement>();

		mover.input = false;
		cube.GetComponent<Rigidbody>().isKinematic = true;
		mover.isBoosting = true;

		var tileToDrop = mover.FetchCubeGridPos();

		while(mover.isBoosting)
		{
			cube.transform.position += transform.TransformDirection(Vector3.forward) * boostSpeed * Time.deltaTime;
			yield return null;
		}

		mover.RoundPosition();
		mover.UpdatePositions();

		FindObjectOfType<TileHandler>().DropTile(tileToDrop);

		mover.isBoosting = false;
		cube.GetComponent<Rigidbody>().isKinematic = false;
		
		mover.CheckFloorInNewPos();
	}

}
