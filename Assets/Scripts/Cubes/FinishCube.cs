using System.Collections;
using System.Collections.Generic;
using Qbism.PlayerCube;
using Qbism.SceneTransition;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using Dreamteck.Splines;
using Qbism.Saving;
using System;

namespace Qbism.Cubes
{
	public class FinishCube : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float shrinkInterval = .25f;
		[SerializeField] GameObject[] shapies = null;

		//Cache
		PlayerCubeMover mover;
		CubeHandler handler;
		SceneHandler loader;
		FinishCubeJuicer juicer;
		PlayerFartLauncher farter;

		//States
		Vector2Int myPosition;
		List<float> pushDegrees = new List<float> { 0, 45, 90, 135, 180, 225, 270, 315 };

		//Actions, events, delegates etc
		public delegate bool GetConnectionDel();
		public GetConnectionDel onSerpentCheck;
		public GetConnectionDel onMapCheck;

		public event Action<bool> onSetSerpentMove; 

		private void Awake()
		{
			mover = FindObjectOfType<PlayerCubeMover>();
			handler = FindObjectOfType<CubeHandler>();
			loader = FindObjectOfType<SceneHandler>();
			juicer = GetComponent<FinishCubeJuicer>();
			farter = FindObjectOfType<PlayerFartLauncher>();
		}

		private void OnEnable()
		{
			if (handler != null) handler.onLand += CheckForFinish;
			if (juicer != null) juicer.onSpawnFriends += SpawnFriends;
		}

		private void Start()
		{
			myPosition = new Vector2Int
				(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
		}

		private void CheckForFinish()
		{
			if (handler.FetchCube(myPosition) == handler.FetchCube(mover.FetchGridPos()))
			{
				if (Mathf.Approximately(Vector3.Dot(mover.transform.forward, transform.up), -1))
					StartCoroutine(Finish());	

				else StartCoroutine(LevelTransition(false, true));
			}
		}

		public void StartFinish()
		{
			StartCoroutine(Finish());
		}

		private IEnumerator Finish()
		{
			ProgressHandler progHandler = FindObjectOfType<ProgressHandler>();

			if (onMapCheck()) //TO DO: eventually these checks should be obsolete bc map should always be available and a level is always started via map
			{
				progHandler.SetLevelToComplete(progHandler.currentLevelID, true);
			}

			if (progHandler.currentHasSerpent)
			{
				print("Serpent segment found!");

				if (onSerpentCheck())
				{
					SerpentProgress serpProg = FindObjectOfType<SerpentProgress>();
					serpProg.AddSegment();
				}
			}
			else print("Shapekin liberated!");

			progHandler.SaveProgData();

			juicer.PlaySuccesSound();

			yield return DestroyAllFloorCubes();
			ActivateLevelCompleteCam();

			farter.InitiateFartSequence();
		}

		private IEnumerator SerpentSequence()
		{
			if (onSerpentCheck()) ActivateSerpent(); //TO DO: eventually these checks should be obsolete bc every level will have serpent
			yield return new WaitForSeconds(3); //TO DO: this should be the length of serpent anim

			if (onMapCheck()) StartCoroutine(LevelTransition(true, false));
			else StartCoroutine(LevelTransition(false, false));
		}

		private void ActivateSerpent()
		{
			var serpent = GameObject.FindGameObjectWithTag("LevelCompFollower");
			serpent.GetComponent<SplineFollower>().followSpeed = 15;
			onSetSerpentMove(true);
		}

		private IEnumerator DestroyAllFloorCubes()
		{
			List<FloorCube> floorCubeList = new List<FloorCube>();

			foreach (KeyValuePair<Vector2Int, FloorCube> pair in handler.floorCubeDic)
			{
				var cube = pair.Value;
				if(cube.type == CubeTypes.Finish || 
					cube.GetComponent<CubeShrinker>().hasShrunk == true) continue;

				floorCubeList.Add(cube);
				
			}

			for (int i = 0; i < floorCubeList.Count; i++)
			{
				floorCubeList[i].GetComponent<CubeShrinker>().StartShrinking();
				yield return new WaitForSeconds(shrinkInterval);
			}
		}

		private void ActivateLevelCompleteCam()
		{
			var lvlCompCam = GetComponentInChildren<CinemachineVirtualCamera>();
			lvlCompCam.Priority = 11;
			lvlCompCam.transform.parent = null;
		}

		private void SpawnFriends()
		{
			for (int i = 0; i < 3; i++)
			{
				int shapeIndex = UnityEngine.Random.Range(0, shapies.Length - 1);
				GameObject toSpawn = shapies[shapeIndex];
				int degreeIndex = UnityEngine.Random.Range(0, pushDegrees.Count - 1);
				Instantiate(toSpawn, transform.position, Quaternion.Euler(0f, pushDegrees[degreeIndex], 0f));
				pushDegrees.RemoveAt(degreeIndex);
			}
		}

		private IEnumerator LevelTransition(bool mapConnected, bool restart)
		{
			yield return new WaitWhile(() => juicer.source.isPlaying);
			//TO DO: Make timing wait for animations that are to come

			if(restart)
			{
				juicer.PlayFailSound();
				loader.RestartLevel();
			} 
			else if(mapConnected) loader.LoadWorldMap();
			else loader.NextLevel();
		}

		private void OnDisable()
		{
			if (handler != null) handler.onLand -= CheckForFinish;
			if (juicer != null) juicer.onSpawnFriends -= SpawnFriends;
		}
	}

}