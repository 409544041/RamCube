using System.Collections;
using System.Collections.Generic;
using Qbism.PlayerCube;
using UnityEngine;
using UnityEngine.Events;

namespace Qbism.Cubes
{
	public class FlipCube : MonoBehaviour, ICubeInfluencer
	{
		//Config parameters
		[SerializeField] int turnStep = 9;
		[SerializeField] float timeStep = 0.01f;
		[SerializeField] GameObject seeThroughCube = null;

		//Cache
		PlayerCubeMover mover;
		CubeHandler handler;
		FeedForwardCube[] ffCubes;
		PlayerCubeFeedForward cubeFeedForward;

		//States
		Vector2Int myPosition;

		public UnityEvent onFlipEvent = new UnityEvent();

		private void Awake()
		{
			mover = FindObjectOfType<PlayerCubeMover>();
			handler = FindObjectOfType<CubeHandler>();
			cubeFeedForward = FindObjectOfType<PlayerCubeFeedForward>();
			ffCubes = cubeFeedForward.FetchFFCubes();
		}

		private void OnEnable()
		{
			if (handler != null) handler.onLand += DisableSeeThrough;
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

		public void PrepareAction(GameObject cube)
		{
			if (cube.GetComponent<PlayerCubeMover>()) StartCoroutine(ExecuteActionOnPlayer(cube));
			else if (cube.GetComponent<FeedForwardCube>()) StartCoroutine(ExecuteActionOnFF(cube));
		}

		public IEnumerator ExecuteActionOnPlayer(GameObject cube)
		{
			mover.input = false;
			cube.GetComponent<Rigidbody>().isKinematic = true;

			var tileToDrop = mover.FetchGridPos();

			var axis = transform.TransformDirection(Vector3.left);

			onFlipEvent.Invoke();

			for (int i = 0; i < (90 / turnStep); i++)
			{
				cube.transform.Rotate(axis, turnStep, Space.World);
				yield return new WaitForSeconds(timeStep);
			}

			mover.RoundPosition();
			mover.UpdateCenterPosition();

			cube.GetComponent<Rigidbody>().isKinematic = false;

			mover.CheckFloorInNewPos();
		}

		public IEnumerator ExecuteActionOnFF(GameObject ffCube)
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
			if (handler.FetchCube(myPosition) == handler.FetchCube(mover.FetchGridPos()))
			{
				seeThroughCube.SetActive(false);
				return;
			}

			seeThroughCube.SetActive(true);
		}

		private void OnDisable()
		{
			if (handler != null) handler.onLand -= DisableSeeThrough;
		}
	}

}