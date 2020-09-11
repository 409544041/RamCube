using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipCube : MonoBehaviour
{
	//Config parameters
	[SerializeField] int turnStep = 9;
	[SerializeField] float timeStep = 0.01f;
	[SerializeField] GameObject seeThroughCube;

	//Cache
	PlayerCubeMover mover;
	CubeHandler handler;
	FeedForwardCube[] ffCubes;
	PlayerCubeFeedForward cubeFeedForward;

	//States
	Vector2Int myPosition;

	private void Awake() 
	{
		mover = FindObjectOfType<PlayerCubeMover>();
		handler = FindObjectOfType<CubeHandler>();
		cubeFeedForward = FindObjectOfType<PlayerCubeFeedForward>();
		ffCubes = cubeFeedForward.FetchFFCubes();
	}

	private void OnEnable() 
	{
		if (mover != null) mover.onLand += DisableSeeThrough;
	}

	private void Start() 
	{
		myPosition = new Vector2Int
			(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
	}

	private void Update() 
	{
		FlipSelf(Vector3.left, seeThroughCube);
	}

	public void StartFlip(GameObject cube)
	{
		if(cube.GetComponent<PlayerCubeMover>()) StartCoroutine(FlipPlayerCube(cube));
		else if(cube.GetComponent<FeedForwardCube>()) StartCoroutine(FlipFF(cube));
	}

	private IEnumerator FlipPlayerCube(GameObject cube)
	{
		mover.input = false;
		cube.GetComponent<Rigidbody>().isKinematic = true;

		var tileToDrop = mover.FetchGridPos();

		var axis = transform.TransformDirection(Vector3.left);

		for (int i = 0; i < (90 / turnStep); i++)
		{
			cube.transform.Rotate(axis, turnStep, Space.World);
			yield return new WaitForSeconds(timeStep);
		}

		mover.RoundPosition();
		mover.UpdatePositions();

		cube.GetComponent<Rigidbody>().isKinematic = false;

		mover.CheckFloorInNewPos();
	}

	private IEnumerator FlipFF(GameObject ffCube)
	{
		var ff = ffCube.GetComponent<FeedForwardCube>();
		var axis = transform.TransformDirection(Vector3.left);

		for (int i = 0; i < (90 / turnStep); i++)
		{
			ffCube.transform.Rotate(axis, turnStep, Space.World);
			yield return null;
		}

		ff.RoundPosition();
	}

	private void FlipSelf(Vector3 direction, GameObject objectToFlip)
	{
		var axis = transform.TransformDirection(direction);
		objectToFlip.transform.Rotate(axis, turnStep, Space.World);
	}

	private void DisableSeeThrough()
	{
		if(handler.FetchTile(myPosition) == handler.FetchTile(mover.FetchGridPos()))
		{
			seeThroughCube.SetActive(false);
			return;
		}
			
		seeThroughCube.SetActive(true);
	}

	private void OnDisable()
	{
		if (mover != null) mover.onLand -= DisableSeeThrough;
	}
}
