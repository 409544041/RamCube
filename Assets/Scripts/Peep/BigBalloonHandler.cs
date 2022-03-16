using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Peep
{
	public class BigBalloonHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Vector2 minMaxSpawnDelay;
		[SerializeField] float maxAddHeight = 2;
		[SerializeField] SplineComputer[] viableSplines;
		[SerializeField] SplineFollower follower;

		//Cache
		Renderer[] meshes;

		//States
		float respawnTimer;
		float timeToRespawn;
		bool timerActivated = false;
		SplineComputer currentSpline;
		float addHeight;

		private void Awake()
		{
			meshes = GetComponentsInChildren<Renderer>();
		}

		private void Start()
		{
			currentSpline = follower.spline;
			addHeight = 0 + Random.Range(0, maxAddHeight);
			follower.spline.transform.localPosition = new Vector3(0, addHeight, 0);
		}

		private void Update()
		{
			if (timerActivated)
			{
				respawnTimer += Time.deltaTime;
				if (respawnTimer > timeToRespawn)
				{
					RespawnBalloon();
					timerActivated = false;
					respawnTimer = 0;
				}
			}
		}

		public void HandleBalloonRespawn() //Called from spline event
		{
			ToggleMeshes(false);

			respawnTimer = 0;
			timeToRespawn = Random.Range(minMaxSpawnDelay.x, minMaxSpawnDelay.y);
			timerActivated = true;
		}

		private void RespawnBalloon()
		{
			print("Respawning");
			if (viableSplines.Length > 1)
			{
				SplineComputer[] otherSplines = new SplineComputer[viableSplines.Length - 1];
				int j = 0;

				for (int i = 0; i < viableSplines.Length; i++)
				{
					if (viableSplines[i] == currentSpline) continue;
					otherSplines[j] = viableSplines[i];
					j++;
				}

				currentSpline = otherSplines[Random.Range(0, otherSplines.Length)];
			}

			follower.spline = currentSpline;
			addHeight = 0 + Random.Range(0, maxAddHeight);
			follower.spline.transform.localPosition = new Vector3(0, addHeight, 0);
			follower.Restart(1);
			ToggleMeshes(true);
		}

		private void ToggleMeshes(bool value)
		{
			foreach (var mesh in meshes)
			{
				mesh.enabled = value;
			}
		}
	}
}
