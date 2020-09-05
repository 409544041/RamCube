using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipCube : MonoBehaviour
{
	//Config parameters
	[SerializeField] int turnStep = 9;
	[SerializeField] GameObject seeThroughCube;

	PlayerCubeMover mover;

	private void Awake() 
	{
		mover = FindObjectOfType<PlayerCubeMover>();
	}

	private void OnEnable() 
	{
		if(mover != null) mover.onLand += StartSelfFlip;
	}

	public void StartFlip(GameObject cube)
	{
		StartCoroutine(FlipPlayerCube(cube));
	}

	private IEnumerator FlipPlayerCube(GameObject cube)
	{
		mover.input = false;
		cube.GetComponent<Rigidbody>().isKinematic = true;

		var tileToDrop = mover.FetchCubeGridPos();

		var axis = transform.TransformDirection(Vector3.left);

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

	private void StartSelfFlip()
	{
		StartCoroutine(FlipSelf(Vector3.right, this.gameObject));
		StartCoroutine(FlipSelf(Vector3.left, seeThroughCube));
	}

	private IEnumerator FlipSelf(Vector3 direction, GameObject objectToFlip)
	{
		var axis = transform.TransformDirection(direction);

		for (int i = 0; i < (90 / turnStep); i++)
		{
			objectToFlip.transform.Rotate(axis, turnStep, Space.World);
			yield return null;
		}
	}

	private void OnDisable()
	{
		if (mover != null) mover.onLand -= StartSelfFlip;
	}
}
