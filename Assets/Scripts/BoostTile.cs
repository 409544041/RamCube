using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostTile : MonoBehaviour
{
	//Config parameters
	[SerializeField] Collider boostCollider;
	[SerializeField] float boostSpeed = 30f;

	public void PrepareBoost(GameObject cube)
	{
		boostCollider.enabled = true;
		boostCollider.transform.parent = cube.transform;

		StartCoroutine(Boost(cube));
	}

	public IEnumerator Boost(GameObject cube)
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
		mover.input = true;
	}

}
