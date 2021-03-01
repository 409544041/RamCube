using System.Collections;
using System.Collections.Generic;
using Qbism.PlayerCube;
using Qbism.SceneTransition;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using Dreamteck.Splines;
using Qbism.Saving;

namespace Qbism.Cubes
{
	public class FinishCube : MonoBehaviour
	{
		//Config parameters
		[SerializeField] AudioClip succesClip = null, failClip = null;

		//Cache
		PlayerCubeMover mover;
		CubeHandler handler;
		SceneHandler loader;
		AudioSource source;

		//States
		Vector2Int myPosition;

		//Actions, events, delegates etc
		public delegate bool GetConnectionDel();
		public GetConnectionDel onSerpentCheck;
		public GetConnectionDel onMapCheck;

		public UnityEvent onFinishEvent = new UnityEvent();

		private void Awake()
		{
			mover = FindObjectOfType<PlayerCubeMover>();
			handler = FindObjectOfType<CubeHandler>();
			loader = FindObjectOfType<SceneHandler>();
			source = GetComponentInChildren<AudioSource>();
		}

		private void OnEnable()
		{
			if (handler != null) handler.onLand += CheckForFinish;
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
				if (Mathf.Approximately(Vector3.Dot(mover.transform.forward,
					transform.up), -1)) 
				{
					if(onMapCheck()) //TO DO: eventually these checks should be obsolete bc map should always be available and a level is always started via map
					{
						ProgressHandler progHandler = FindObjectOfType<ProgressHandler>();
						progHandler.SetLevelToComplete(progHandler.currentLevelID, true);
					}
					
					mover.transform.parent = transform;
					DestroyAllFloorCubes();
					ActivateLevelCompleteCam();
					if(onSerpentCheck()) ActivateSerpent(); //TO DO: eventually these checks should be obsolete bc every level will have serpent
					if(onMapCheck()) StartCoroutine(LevelTransition(succesClip, true, false));
					else StartCoroutine(LevelTransition(succesClip, false, false));
				}		

				else StartCoroutine(LevelTransition(failClip, false, true));
			}
		}

		private void DestroyAllFloorCubes()
		{
			foreach (KeyValuePair<Vector2Int, FloorCube> pair in handler.floorCubeDic)
			{
				var cube = pair.Value;
				if(cube.GetComponent<FinishCube>() || 
					cube.GetComponent<CubeShrinker>().hasShrunk == true) continue;

				cube.GetComponent<CubeShrinker>().StartShrinking();
			}
		}

		private void ActivateLevelCompleteCam()
		{
			var lvlCompCam = GetComponentInChildren<CinemachineVirtualCamera>();
			lvlCompCam.Priority = 11;
			lvlCompCam.transform.parent = null;
		}

		private void ActivateSerpent()
		{
			var serpent = GameObject.FindGameObjectWithTag("LevelCompFollower").GetComponent<SplineFollower>();
			serpent.followSpeed = 15;
		}

		private IEnumerator LevelTransition(AudioClip clip, bool mapConnected, bool restart)
		{
			source.clip = clip;
			onFinishEvent.Invoke();
			yield return new WaitWhile(() => source.isPlaying);
			// yield return new WaitForSeconds(5); //TO DO: Make timing wait for animations that are to come

			if(restart) loader.RestartLevel();
			else if(mapConnected) loader.LoadWorldMap();
			else loader.NextLevel();
		}

		private void OnDisable()
		{
			if (handler != null) handler.onLand -= CheckForFinish;
		}
	}

}