using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Dreamteck.Splines;
using Qbism.Environment;
using Qbism.General;
using Qbism.MoveableCubes;
using Qbism.Objects;
using Qbism.PlayerCube;
using Qbism.Saving;
using Qbism.SceneTransition;
using Qbism.Serpent;
using UnityEngine;

namespace Qbism.Cubes
{
	public class FinishEndSeqHandler : MonoBehaviour
	{
		//Config parameters
		[Header("Floor Cube Shrinking")]
		[SerializeField] float shrinkInterval = .25f;
		[Header("Cameras")]
		[SerializeField] CinemachineVirtualCamera closeUpCam;
		[SerializeField] CinemachineVirtualCamera endCam;
		[Header("Camera Values")]
		[SerializeField] AnimationCurve camResizeCurve;
		[Space(10)]
		[SerializeField] float closeUpResize = .9f; 
		[SerializeField] float launchResize = 8, finishImpactResize = 1,
			afterDialogueResize = 1.5f, endCamMotherResize = 1.5f, shapieEndCamSize = 5,
			shapieEndCamResize = 5.5f;
		[Space(10)]
		[SerializeField] float finishImpactResizeDur = .2f;
		[SerializeField] float endCamDollyDur = 5, endCamMotherDollyDur = 10;
		[Space(10)]
		[SerializeField] float endCamDollyTarget = .3f;
		[SerializeField] float endCamMotherDollyTarget = 1;
		[Header("Fart Values")]
		[SerializeField] Transform fartTowardsTarget;
		[SerializeField] float fartLaunchDelay, fartExplosionDelay;
		[Header("References")]
		public FinishCubeJuicer finishJuicer;
		[SerializeField] SegmentSpawner segSpawner;
		[SerializeField] ObjectSpawner objSpawner;
		

		//Cache
		CubeHandler handler;
		MoveableCubeHandler movHandler;
		PlayerAnimator playerAnim;
		SceneHandler loader;
		ProgressHandler progHandler;
		PlayerFartLauncher farter;
		FeatureSwitchBoard switchBoard;
		CamResizer camResizer;

		//Actions, events, delegates etc
		public event Action<bool> onSetSerpentMove;
		public event Action onShowSegments;
		public event Action onSpawnShapie;
		public event Action<float> onUIFade;

		private void Awake()
		{
			handler = FindObjectOfType<CubeHandler>();
			movHandler = handler.GetComponent<MoveableCubeHandler>();
			playerAnim = FindObjectOfType<PlayerAnimator>();
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

			if (finishJuicer != null) finishJuicer.onSpawn += Spawn;
			
			if (farter != null) farter.onSwitchToEndCam += SwitchToEndCam;
		}

		public void InitiateEndSeq()
		{
			StartCoroutine(EndSequence());
		}

		private IEnumerator EndSequence()
		{
			yield return ShrinkAllFloorCubes();
			yield return ShrinkAllWalls();

			if (switchBoard.showEndLevelSeq)
			{
				closeUpCam.Priority = 11;
				onUIFade(0);

				if (FetchHasSegment())
				{
					yield return new WaitForSeconds(fartLaunchDelay);
					farter.InitiateLaunchSequence(fartTowardsTarget, true);
				}
				
				else
				{
					yield return new WaitForSeconds(fartExplosionDelay);
					if (progHandler.currentHasObject) farter.hasSegObj = true;
					farter.InitiateLaunchSequence(null, false); 
				}
			}
			else StartCoroutine(SerpentSequence());
		}

		private IEnumerator ShrinkAllFloorCubes()
		{
			List<CubeShrinker> cubesToShrinkList = new List<CubeShrinker>();

			foreach (KeyValuePair<Vector2Int, MoveableCube> pair in movHandler.moveableCubeDic)
			{
				var cube = pair.Value;
				var pos = pair.Key;

				var effector = cube.GetComponent<MoveableEffector>();
				if (effector != null) effector.ToggleEffectFace(false);

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

				var effector = cube.GetComponent<MoveableEffector>();
				if (effector != null) effector.ToggleEffectFace(false);

				cubesToShrinkList.Add(cube.GetComponent<CubeShrinker>());
			}

			for (int i = 0; i < cubesToShrinkList.Count; i++)
			{
				cubesToShrinkList[i].StartShrinking();
				yield return new WaitForSeconds(shrinkInterval);
			}
		}

		//Includes temporary wall shrink script for old walls
		private IEnumerator ShrinkAllWalls()
		{
			var wallsToShrink = GameObject.FindGameObjectsWithTag("Wall");

			for (int i = 0; i < wallsToShrink.Length; i++)
			{
				var shrinker = wallsToShrink[i].GetComponent<CubeShrinker>();
				if (shrinker != null) shrinker.StartShrinking();
				else
				{
					var wallRef = wallsToShrink[i].GetComponentInParent<WallRefHolder>();
					if (wallRef == null) continue;

					wallRef.wallJuicer.Burrow();
					wallRef.col.enabled = false;

					yield return new WaitForSeconds(wallRef.wallJuicer.burrowTime);
				}

				yield return new WaitForSeconds(shrinkInterval);
			}			
		}

		private void SwitchToEndCam()
		{
			if (!FetchHasSegment())
			{
				endCam.m_Lens.OrthographicSize = shapieEndCamSize;
				camResizer.InitiateCamDollyMove(endCam, endCamDollyTarget, 
					endCamMotherDollyDur, camResizeCurve);
				camResizer.InitiateCamResize(endCam, shapieEndCamResize, endCamMotherDollyDur,
					camResizeCurve);
			}

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

		private void Spawn()
		{
			if (FetchHasSegment() && switchBoard.serpentConnected)
				segSpawner.SpawnSegment();
			else if (progHandler.currentHasObject && switchBoard.objectsConnected)
				objSpawner.SpawnObject();
			else onSpawnShapie();
		}

		private IEnumerator LevelTransition(bool mapConnected, bool restart)
		{
			yield return new WaitWhile(() => finishJuicer.source.isPlaying);
			//TO DO: Make timing wait for animations that are to come

			if (restart)
			{
				finishJuicer.PlayFailSound();
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

			var serpProg = progHandler.GetComponent<SerpentProgress>();

			//if it's a segment rescue and 1 is still false, it has to be the head
			if (E_SegmentsGameplayData.GetEntity(1).f_Rescued == false) 
			{
				StartCoroutine(PanCamWithDelay(finishImpactResizeDur, endCam, endCamMotherDollyTarget, 
					endCamMotherDollyDur, camResizeCurve));
				StartCoroutine(ResizeCamWithDelay(finishImpactResizeDur, endCam, endCamMotherResize,
					endCamMotherDollyDur, camResizeCurve));
			}	

			else
			StartCoroutine(PanCamWithDelay(finishImpactResizeDur, endCam, endCamDollyTarget, endCamDollyDur,
				camResizeCurve));
		}

		public void PanAndZoomCamAfterDialogue()
		{
			camResizer.InitiateCamDollyMove(endCam, 0, endCamDollyDur, camResizeCurve);
			camResizer.InitiateCamResize(endCam, afterDialogueResize, endCamDollyDur, camResizeCurve);
		}

		private IEnumerator ResizeCamWithDelay(float delay, CinemachineVirtualCamera cam, float sizeTarget,
			float resizeDur, AnimationCurve curve)
		{
			yield return new WaitForSeconds(delay);

			camResizer.InitiateCamResize(cam, sizeTarget, resizeDur, curve);
		}

		private IEnumerator PanCamWithDelay(float delay, CinemachineVirtualCamera cam, float target, 
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

			if (finishJuicer != null) finishJuicer.onSpawn -= Spawn;

			if (farter != null) farter.onSwitchToEndCam -= SwitchToEndCam;
		}
	}
}
