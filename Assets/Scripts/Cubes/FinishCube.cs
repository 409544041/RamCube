using System.Collections;
using System.Collections.Generic;
using Qbism.PlayerCube;
using Qbism.SceneTransition;
using UnityEngine;
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

		//Cache
		PlayerCubeMover mover;
		CubeHandler handler;
		SceneHandler loader;
		FinishCubeJuicer juicer;
		PlayerFartLauncher farter;
		ProgressHandler progHandler;
		SerpentProgress serpProg;

		//States
		Vector2Int myPosition;
		public bool wrongOnFinish { get; set; } = false;
		
		//Actions, events, delegates etc
		public delegate bool GetConnectionDel();
		public GetConnectionDel onSerpentCheck;
		public GetConnectionDel onMapCheck;
		public event Action onSetSegment;
		public event Action onSpawnSegment;
		public event Action<bool> onSetSerpentMove; 
		public event Action onSpawnShapie;
		public event Action<InterfaceIDs> onRewindPulse;
		public event Action<InterfaceIDs> onStopRewindPulse;
		public event Action onShowSegments;

		private void Awake()
		{
			mover = FindObjectOfType<PlayerCubeMover>();
			handler = FindObjectOfType<CubeHandler>();
			loader = FindObjectOfType<SceneHandler>();
			juicer = GetComponent<FinishCubeJuicer>();
			farter = FindObjectOfType<PlayerFartLauncher>();
			progHandler = FindObjectOfType<ProgressHandler>();
			serpProg = FindObjectOfType<SerpentProgress>();
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

				else
				{
					wrongOnFinish = true;
					PulseRewindUI();
				} 
			}
			else if (wrongOnFinish)
			{
				onStopRewindPulse(InterfaceIDs.Rewind);
				wrongOnFinish = false;
			} 
		}

		public void StartFinish()
		{
			StartCoroutine(Finish());
		}

		private IEnumerator Finish()
		{
			if (onMapCheck()) //TO DO: eventually these checks should be obsolete bc map should always be available and a level is always started via map
			{
				progHandler.SetLevelToComplete(progHandler.currentLevelID, true);
			}

			if (progHandler.currentHasSegment)
			{
				print("Serpent segment found!");

				if (onSerpentCheck())
				{
					onSetSegment(); //Needs to be done before AddSegment
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
			yield return new WaitForSeconds(4); //TO DO: this should be the length of serpent anim

			if (onMapCheck()) StartCoroutine(LevelTransition(true, false));
			else StartCoroutine(LevelTransition(false, false));
		}

		private void ActivateSerpent()
		{
			var serpent = GameObject.FindGameObjectWithTag("LevelCompFollower");
			serpent.GetComponent<SplineFollower>().followSpeed = 15;
			onSetSerpentMove(true);
			onShowSegments();
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
			if (progHandler.currentHasSegment)
			{
				onSpawnSegment();
			}
			else onSpawnShapie();
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

		private void PulseRewindUI()
		{
			onRewindPulse(InterfaceIDs.Rewind);
		}

		public void StopPulseRewindUI()
		{
			onStopRewindPulse(InterfaceIDs.Rewind);
		}

		private void OnDisable()
		{
			if (handler != null) handler.onLand -= CheckForFinish;
			if (juicer != null) juicer.onSpawnFriends -= SpawnFriends;
		}
	}

}