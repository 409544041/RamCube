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
		[SerializeField] SplineComputer[] viableSplines;
		[SerializeField] SplineFollower follower;

		//Cache
		Renderer[] meshes;

		//States
		float respawnTimer;
		float timeToRespawn;
		bool timerActivated = false;
		SplineComputer currentSpline;

		private void Awake()
		{
			meshes = GetComponentsInChildren<Renderer>();
		}

		private void Start()
		{
			currentSpline = follower.spline;
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
			print("Calling");
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
