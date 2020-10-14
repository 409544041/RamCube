using System.Collections;
using System.Collections.Generic;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.Cubes
{
	public class TurningCube : MonoBehaviour, ICubeInfluencer
	{
		//Config parameters
		[SerializeField] int turnStep = 9;
		[SerializeField] float timeStep = 0.01f;
		[SerializeField] bool isLeftTurning = false;
		[SerializeField] GameObject topPlane = null;

		//Cache
		PlayerCubeMover mover;

		//States
		Vector3 turnAxis = new Vector3(0, 0, 0);

		private void Awake()
		{
			mover = FindObjectOfType<PlayerCubeMover>();

			if(isLeftTurning)
			{
				topPlane.transform.localScale = new Vector3 (-1, 1, 1);
				turnAxis = Vector3.down;
			} 
			else turnAxis = Vector3.up;
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

			var axis = transform.TransformDirection(turnAxis);

			//onFlipEvent.Invoke();

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

			var axis = transform.TransformDirection(turnAxis);

			for (int i = 0; i < (90 / turnStep); i++)
			{
				ffCube.transform.Rotate(axis, turnStep, Space.World);
				yield return null;
			}

			ff.RoundPosition();
		}
	}
}
