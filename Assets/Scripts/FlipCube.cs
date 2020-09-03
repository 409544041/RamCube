using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipCube : MonoBehaviour
{
	//Config parameters
	[SerializeField] int turnStep = 9;

	public void StartFlip(GameObject cube)
	{
		StartCoroutine(Flip(cube));
	}

	private IEnumerator Flip(GameObject cube)
	{
		CubeMovement mover = cube.GetComponent<CubeMovement>();

		mover.input = false;
		cube.GetComponent<Rigidbody>().isKinematic = true;

		var tileToDrop = mover.FetchCubeGridPos();

		var axis = transform.TransformDirection(Vector3.forward);

		for (int i = 0; i < (90 / turnStep); i++)
		{
			cube.transform.Rotate(axis, turnStep, Space.World);
			yield return null;
		}

		mover.RoundPosition();
		mover.UpdatePositions();

		cube.GetComponent<Rigidbody>().isKinematic = false;

		mover.CheckFloorInNewPos();
	}
}
