using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Environment
{
	public class WindSpawner : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Transform[] zMarkers, xMarkers;
		[SerializeField] ParticleSystem[] windVFX;
		[SerializeField] Vector2 minMaxSpawnInterval;

		//States
		float spawnTimer;
		float timeToSpawn;

		private void Start() 
		{
			timeToSpawn = Random.Range(minMaxSpawnInterval.x, minMaxSpawnInterval.y);
		}

		private void Update()
		{
			spawnTimer += Time.deltaTime;
			SpawnWind();
		}

		private void SpawnWind()
		{
			if (spawnTimer >= timeToSpawn)
			{
				int index = Random.Range(0, windVFX.Length);
				PositionWind(windVFX[index]);
				windVFX[index].Play();
				spawnTimer = 0;
				timeToSpawn = Random.Range(minMaxSpawnInterval.x, minMaxSpawnInterval.y);
			}
		}

		private void PositionWind(ParticleSystem windVFX)
		{
			var spawnPosZ = Random.Range(zMarkers[0].position.z, zMarkers[1].position.z);
			var spawnPosX = Random.Range(xMarkers[0].position.x, xMarkers[1].position.x);
			Vector3 spawnPos = new Vector3(spawnPosX, transform.position.y, spawnPosZ);
			windVFX.transform.position = spawnPos;
		}
	}
}
