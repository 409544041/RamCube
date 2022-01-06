using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Dreamteck.Splines;
using Qbism.General;
using Qbism.MoveableCubes;
using Qbism.PlayerCube;
using Qbism.Saving;
using Qbism.SceneTransition;
using UnityEngine;

namespace Qbism.Cubes
{
	public class FinishEndSeqHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float shrinkInterval = .25f;
		[SerializeField] CinemachineVirtualCamera closeUpCam, endCam;
		[SerializeField] Transform fartTowardsTarget;
		[SerializeField] float fartDelay;
		[SerializeField] AnimationCurve camResizeCurve;
		[SerializeField] float closeUpResize = .9f, launchResize = 8, finishImpactResize = 1,
			finishImpactResizeDur = .2f, endCamDollyTarget = .3f, endCamDollyDur = 5;

		//Cache
		CubeHandler handler;
		MoveableCubeHandler movHandler;
		PlayerAnimator playerAnim;
		FinishCubeJuicer juicer;
		SceneHandler loader;
		ProgressHandler progHandler;
		PlayerFartLauncher farter;
		FeatureSwitchBoard switchBoard;
		CamResizer camResizer;

		//Actions, events, delegates etc
		public event Action<bool> onSetSerpentMove;
		public event Action onShowSegments;
		public event Action onSpawnSegment;
		public event Action onSpawnShapie;
		public event Action<float> onUIFade;

		private void Awake()
		{
			handler = FindObjectOfType<CubeHandler>();
			movHandler = handler.GetComponent<MoveableCubeHandler>();
			playerAnim = FindObjectOfType<PlayerAnimator>();
			juicer = GetComponent<FinishCubeJuicer>();
			loader = FindObjectOfType<SceneHandler>();
			progHandler = FindObjectOfType<ProgressHandler>();
			farter = FindObjectOfType<PlayerFartLauncher>();
			switchBoard = progHandler.GetComponent<FeatureSwitchBoard>();
			camResizer = handler.GetComponent<CamResizer>();
		}

		private void OnEnable() 
		{
			if (playerAnim != null)
			{
				playerAnim.onTriggerSerpent += InitiateSerpentSequence;
				playerAnim.onGetFinishPos += GetPos;
			}

			if (juicer != null) juicer.onSpawnFriends += SpawnFriends;
			
			if (farter != null) farter.onSwitchToEndCam += SwitchToEndCam;
		}

		public void InitiateEndSeq()
		{
			StartCoroutine(EndSequence());
		}

		private IEnumerator EndSequence()
		{
			yield return DestroyAllFloorCubes();

			if (switchBoard.showEndLevelSeq)
			{
				closeUpCam.Priority = 11;
				onUIFade(0);
				yield return new WaitForSeconds(fartDelay);
				farter.InitiateFartSequence(fartTowardsTarget);
			}
			else StartCoroutine(SerpentSequence());
		}

		private IEnumerator DestroyAllFloorCubes()
		{
			List<CubeShrinker> cubesToShrinkList = new List<CubeShrinker>();

			foreach (KeyValuePair<Vector2Int, MoveableCube> pair in movHandler.moveableCubeDic)
			{
				var cube = pair.Value;
				var pos = pair.Key;
				if (!handler.movFloorCubeDic.ContainsKey(pos) &&
					!handler.shrunkMovFloorCubeDic.ContainsKey(pos))
					cubesToShrinkList.Add(cube.GetComponent<CubeShrinker>());
			}

			foreach (KeyValuePair<Vector2Int, FloorCube> pair in handler.floorCubeDic)
			{
				var cube = pair.Value;
				if (cube.type == CubeTypes.Finish) continue;
				cubesToShrinkList.Add(cube.GetComponent<CubeShrinker>());
			}

			foreach (KeyValuePair<Vector2Int, FloorCube> pair in handler.movFloorCubeDic)
			{
				var cube = pair.Value;
				if (cube.type == CubeTypes.Finish) continue;
				cubesToShrinkList.Add(cube.GetComponent<CubeShrinker>());
			}

			for (int i = 0; i < cubesToShrinkList.Count; i++)
			{
				cubesToShrinkList[i].StartShrinking();
				yield return new WaitForSeconds(shrinkInterval);
			}
		}

		private void SwitchToEndCam()
		{
			endCam.Priority = 12;
			endCam.transform.parent = null;
			DisableForegroundObjects();		
		}

		private void DisableForegroundObjects()
		{
			DisableAtEndCam[] objectsToDisable = FindObjectsOfType<DisableAtEndCam>();
			if (objectsToDisable.Length > 0)
			{
				for (int i = 0; i < objectsToDisable.Length; i++)
				{
					objectsToDisable[i].DisableMeshes();
				}
			}
		}

		private void InitiateSerpentSequence()
		{
			StartCoroutine(SerpentSequence());
		}

		private IEnumerator SerpentSequence()
		{
			if (switchBoard.serpentConnected)
			{
				ActivateSerpent(); //TO DO: eventually these checks should be obsolete bc every level will have serpent
				yield return new WaitForSeconds(2); //TO DO: this should be the length of serpent anim
			} 

			if (switchBoard.worldMapConnected) StartCoroutine(LevelTransition(true, false));
			else StartCoroutine(LevelTransition(false, false));
		}

		private void ActivateSerpent()
		{
			var serpent = GameObject.FindGameObjectWithTag("LevelCompFollower");
			serpent.GetComponent<SplineFollower>().followSpeed = 15;
			onSetSerpentMove(true);
			onShowSegments();
		}

		private void SpawnFriends()
		{
			if (FetchHasSegment() && switchBoard.serpentConnected)
			{
				onSpawnSegment();
			}
			else onSpawnShapie();
		}

		private IEnumerator LevelTransition(bool mapConnected, bool restart)
		{
			yield return new WaitWhile(() => juicer.source.isPlaying);
			//TO DO: Make timing wait for animations that are to come

			if (restart)
			{
				juicer.PlayFailSound();
				loader.RestartLevel();
			}
			else if (mapConnected) FindObjectOfType<WorldMapLoading>().
				StartLoadingWorldMap(true);
			else loader.NextLevel();
		}

		public bool FetchHasSegment()
		{
			return progHandler.currentHasSegment;
		}

		private Vector3 GetPos()
		{
			return transform.position;
		}

		public void FartChargeCamResize(float dur)
		{
			camResizer.InitiateCamResize(closeUpCam, closeUpResize, dur, camResizeCurve);
		}

		public void FartLaunchCamResize(float dur)
		{
			camResizer.InitiateCamResize(closeUpCam, launchResize, dur, camResizeCurve);
		}

		private void FinishImpactCamResize() // Called from animation
		{
			camResizer.InitiateCamResize(endCam, finishImpactResize, finishImpactResizeDur, 
				camResizeCurve);

			StartCoroutine(PanEndCam(finishImpactResizeDur, endCam, endCamDollyTarget, endCamDollyDur,
				camResizeCurve));
		}

		private IEnumerator PanEndCam(float delay, CinemachineVirtualCamera cam, float target, 
			float travelDur, AnimationCurve curve)
		{
			yield return new WaitForSeconds(delay);

			camResizer.InitiateCamDollyMove(cam, target, travelDur, curve);
		}

		private void OnDisable()
		{
			if (playerAnim != null)
			{
				playerAnim.onTriggerSerpent -= InitiateSerpentSequence;
				playerAnim.onGetFinishPos -= GetPos;
			}

			if (juicer != null) juicer.onSpawnFriends -= SpawnFriends;

			if (farter != null) farter.onSwitchToEndCam -= SwitchToEndCam;
		}
	}
}
