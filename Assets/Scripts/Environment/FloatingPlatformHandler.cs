using Dreamteck.Splines;
using Qbism.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Environment
{
	public class FloatingPlatformHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] bool respawn, varyHeight;
		[SerializeField] Vector2 minMaxSpawnDelay;
		[SerializeField] float maxAddHeight = 2;
		[SerializeField] SplineComputer[] viableSplines;
		[SerializeField] SplineFollower[] followers;

		//States
		float respawnTimer;
		float timeToRespawn;
		bool timerActivated = false;
		SplineComputer currentSpline;
		SplineFollower currentFollower;
		int currentFollowerIndex;
		float addHeight;

		private void Start()
		{
			currentFollowerIndex = Random.Range(0, followers.Length);
			currentFollower = followers[currentFollowerIndex];
			
			if (viableSplines.Length > 0)
			{
				int j = Random.Range(0, viableSplines.Length);
				currentSpline = viableSplines[j];
				currentFollower.spline = currentSpline;
			}

			VaryHeight();
			TogglePlatform();
		}

		private void Update()
		{
			if (timerActivated && respawn)
			{
				respawnTimer += Time.deltaTime;
				if (respawnTimer > timeToRespawn)
				{
					RespawnPlatform();
					timerActivated = false;
					respawnTimer = 0;
				}
			}
		}

		private void RespawnPlatform()
		{
			if (followers.Length > 1)
			{
				SplineFollower[] otherFollowers = new SplineFollower[followers.Length - 1];
				int j = 0;

				for (int i = 0; i < followers.Length; i++)
				{
					if (i == currentFollowerIndex) continue;
					otherFollowers[j] = followers[i];
					j++;
				}

				currentFollower = otherFollowers[Random.Range(0, otherFollowers.Length)];

				for (int i = 0; i < followers.Length; i++)
				{
					if (followers[i] = currentFollower) currentFollowerIndex = i;
				}
			}
			
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

			currentFollower.spline = currentSpline;
			VaryHeight();
			currentFollower.Restart(1);
			TogglePlatform();
		}

		public void HandlePlatformRespawn() //Called from spline event
		{
			currentFollower.gameObject.SetActive(false);
			currentFollower.spline = null;

			respawnTimer = 0;
			timeToRespawn = Random.Range(minMaxSpawnDelay.x, minMaxSpawnDelay.y);
			timerActivated = true;
		}

		private void TogglePlatform()
		{
			for (int i = 0; i < followers.Length; i++)
			{
				if (i == currentFollowerIndex)
				{
					followers[i].gameObject.SetActive(true);

					if (currentSpline != null) followers[i].enabled = true;
				}
				else
				{
					followers[i].gameObject.SetActive(false);
					followers[i].enabled = false;
				}
			}
		}

		private void VaryHeight()
		{
			if (varyHeight)
			{
				addHeight = 0 + Random.Range(0, maxAddHeight);
				if (currentFollower.spline != null)
					currentFollower.spline.transform.localPosition = new Vector3(0, addHeight, 0);
			}
		}
	}
}
