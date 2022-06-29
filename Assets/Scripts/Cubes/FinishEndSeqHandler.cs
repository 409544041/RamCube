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
		[Header("Camera Values")]
		[SerializeField] AnimationCurve camResizeCurve;
		[Space(10)]
		[SerializeField] float closeUpResize = .9f; 
		[SerializeField] float launchResize = 8, finishImpactResize = 1,
			afterDialogueResize = 1.5f, endCamMotherResize = 1.5f, shapieEndCamSize = 5,
			shapieEndCamResize = 5.5f;
		[Space(10)]
		[SerializeField] float finishImpactResizeDur = .2f;
		[SerializeField] float endCamDollyDur = 5, endCamMotherDollyDur = 10, launchCamResizeDur = .5f;
		[Space(10)]
		[SerializeField] float endCamDollyTarget = .3f;
		[SerializeField] float endCamMotherDollyTarget = 1;
		[Header("Fart Values")]
		[SerializeField] float fartLaunchDelay, fartExplosionDelay;
		[Header("References")]
		[SerializeField] FinishRefHolder refs;
		

		//Cache
		CubeHandler handler;
		MoveableCubeHandler movHandler;
		PlayerAnimator playerAnim;
		SceneHandler loader;
		ProgressHandler progHandler;
		PlayerFartLauncher farter;
		FeatureSwitchBoard switchBoard;
		CamResizer camResizer;
		public FinishCubeJuicer finishJuicer { get; private set; }

		private void Awake()
		{
			handler = refs.gcRef.glRef.cubeHandler;
			movHandler = refs.gcRef.glRef.movCubeHandler;
			playerAnim = refs.gcRef.pRef.playerAnim;
			loader = refs.gcRef.glRef.sceneHandler;
			progHandler = refs.persRef.progHandler;
			farter = refs.gcRef.pRef.fartLauncher;
			switchBoard = refs.persRef.switchBoard;
			camResizer = refs.gcRef.glRef.camResizer;
			finishJuicer = refs.finishJuicer;
		}

		private void OnEnable() 
		{
			if (playerAnim != null)
			{
				playerAnim.onTriggerSerpent += InitiateSerpentSequence;
				playerAnim.onGetFinishPos += GetPos;
			}
			
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
				refs.closeUpCam.Priority = 11;
				refs.gcRef.gameplayCanvasFader.StartFade(0);

				if (FetchHasSegment())
				{
					yield return new WaitForSeconds(fartLaunchDelay);
					farter.InitiateLaunchSequence(refs.fartTarget, true);
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
			List<CubeShrinker> marksToShrinkList = new List<CubeShrinker>();

			foreach (KeyValuePair<Vector2Int, MoveableCube> pair in movHandler.moveableCubeDic)
			{
				var cube = pair.Value;
				var pos = pair.Key;

				var effector = cube.refs.movEffector;
				if (effector != null) effector.ToggleEffectFace(false);

				if (!handler.movFloorCubeDic.ContainsKey(pos) &&
					!handler.shrunkMovFloorCubeDic.ContainsKey(pos))
					cubesToShrinkList.Add(cube.refs.cubeShrink);
			}

			foreach (KeyValuePair<Vector2Int, FloorCube> pair in handler.floorCubeDic)
			{
				var cube = pair.Value;
				if (cube.type == CubeTypes.Finish) continue;
				cubesToShrinkList.Add(cube.refs.cubeShrink);
				marksToShrinkList.Add(cube.refs.cubeShrink);
			}

			foreach (KeyValuePair<Vector2Int, FloorCube> pair in handler.movFloorCubeDic)
			{
				var cube = pair.Value;
				if (cube.type == CubeTypes.Finish) continue;

				var effector = cube.refs.movEffector;
				if (effector != null) effector.ToggleEffectFace(false);

				cubesToShrinkList.Add(cube.refs.cubeShrink);
				marksToShrinkList.Add(cube.refs.cubeShrink);
			}

			foreach (KeyValuePair<Vector2Int, List<FloorCube>> pair in handler.shrunkFloorCubeDic)
			{
				var cubes = pair.Value;
				foreach (var cube in cubes)
				{
					if (cube.type == CubeTypes.Finish) continue;
					marksToShrinkList.Add(cube.refs.cubeShrink);
				}
			}

			foreach (KeyValuePair<Vector2Int, List<FloorCube>> pair in handler.shrunkMovFloorCubeDic)
			{
				var cubes = pair.Value;
				foreach (var cube in cubes)
				{
					if (cube.type == CubeTypes.Finish) continue;
					marksToShrinkList.Add(cube.refs.cubeShrink);
				}
			}

			foreach (var mark in marksToShrinkList)
			{
				mark.ShrinkGroundMarks();
			}

			for (int i = 0; i < cubesToShrinkList.Count; i++)
			{
				cubesToShrinkList[i].StartShrinking();
				yield return new WaitForSeconds(shrinkInterval);
			}
		}

		private IEnumerator ShrinkAllWalls()
		{
			var wallsToShrink = refs.gcRef.walls;

			for (int i = 0; i < wallsToShrink.Length; i++)
			{
				//getting shrinker for temp walls. Remove once we don't have those anymore
				var shrinker = wallsToShrink[i].GetComponent<CubeShrinker>();
				if (shrinker != null) shrinker.StartShrinking();
				else
				{
					var wallRef = wallsToShrink[i].GetComponentInParent<WallRefHolder>();
					if (wallRef == null) continue;

					wallRef.wallJuicer.Burrow();
					wallRef.col.enabled = false;

					yield return new WaitForSeconds(wallRef.wallJuicer.burrowTime / 2);
				}

				yield return new WaitForSeconds(shrinkInterval);
			}			
		}

		private void SwitchToEndCam()
		{
			if (!FetchHasSegment())
			{
				refs.endCam.m_Lens.OrthographicSize = shapieEndCamSize;
				camResizer.InitiateCamDollyMove(refs.endCam, endCamDollyTarget, 
					endCamMotherDollyDur, camResizeCurve);
				camResizer.InitiateCamResize(refs.endCam, shapieEndCamResize, endCamMotherDollyDur,
					camResizeCurve);
			}

			refs.endCam.Priority = 12;
			refs.endCam.transform.parent = null;
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
				yield return new WaitForSeconds(.5f); //to show shapies dance a bit
				ActivateSerpent(); //TO DO: eventually these checks should be obsolete bc every level will have serpent
				yield return new WaitForSeconds(2); //TO DO: this should be the length of serpent anim
			} 

			StartCoroutine(LevelTransition());
		}

		private void ActivateSerpent()
		{
			refs.follower.followSpeed = 15;
			refs.serpMovement.SetMoving(true);
			refs.serpSegHandler.EnableSegmentsWithoutBilly();
		}

		public void Spawn()
		{
			if (FetchHasSegment() && switchBoard.serpentConnected)
				refs.segSpawner.SpawnSegment();
			else if (progHandler.currentHasObject && switchBoard.objectsConnected)
				refs.objSpawner.SpawnObject();
			else refs.shapieSpawner.SpawnShapie();
		}

		private IEnumerator LevelTransition()
		{
			yield return new WaitWhile(() => refs.source.isPlaying);
			//TO DO: Make timing wait for animations that are to come

			var entity = E_LevelData.FindEntity(entity =>
				entity.f_Pin == refs.gcRef.persRef.progHandler.currentPin);

			bool isLastLevel = entity.f_UnlocksPins[0].f_name == "_EMPTY";

			if (switchBoard.isPublicDemo && isLastLevel) loader.LoadEndOfDemo();
			else if (switchBoard.worldMapConnected) refs.gcRef.glRef.mapLoader.StartLoadingWorldMap(true);
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
			camResizer.InitiateCamResize(refs.closeUpCam, closeUpResize, dur, camResizeCurve);
		}

		public void FartLaunchCamResize()
		{
			camResizer.InitiateCamResize(refs.closeUpCam, launchResize, launchCamResizeDur, camResizeCurve);
		}

		private void FinishImpactCamResize() // Called from animation
		{
			camResizer.InitiateCamResize(refs.endCam, finishImpactResize, finishImpactResizeDur, 
				camResizeCurve);

			//if it's a segment rescue and 1 is still false, it has to be the head
			if (E_SegmentsGameplayData.GetEntity(1).f_Rescued == false) 
			{
				StartCoroutine(PanCamWithDelay(finishImpactResizeDur, refs.endCam, endCamMotherDollyTarget, 
					endCamMotherDollyDur, camResizeCurve));
				StartCoroutine(ResizeCamWithDelay(finishImpactResizeDur, refs.endCam, endCamMotherResize,
					endCamMotherDollyDur, camResizeCurve));
			}	

			else
			StartCoroutine(PanCamWithDelay(finishImpactResizeDur, refs.endCam, endCamDollyTarget, endCamDollyDur,
				camResizeCurve));
		}

		public void PanAndZoomCamAfterDialogue()
		{
			camResizer.InitiateCamDollyMove(refs.endCam, 0, endCamDollyDur, camResizeCurve);
			camResizer.InitiateCamResize(refs.endCam, afterDialogueResize, endCamDollyDur, camResizeCurve);
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

			if (farter != null) farter.onSwitchToEndCam -= SwitchToEndCam;
		}
	}
}
