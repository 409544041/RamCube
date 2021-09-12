using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Dreamteck.Splines;
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
		[SerializeField] float fartDelay, flyByDelay;

		//Cache
		CubeHandler handler;
		PlayerAnimator playerAnim;
		FinishCubeJuicer juicer;
		SceneHandler loader;
		ProgressHandler progHandler;
		PlayerFartLauncher farter;
		Fader fader;

		//Actions, events, delegates etc
		public delegate bool GetConnectionDel();
		public GetConnectionDel onSerpentCheck;
		public GetConnectionDel onMapCheck;
		public event Action<bool> onSetSerpentMove;
		public event Action onShowSegments;
		public event Action onAlignCam;
		public event Action onSpawnSegment;
		public event Action onSpawnShapie;
		public event Action<float> onUIFade;

		private void Awake()
		{
			handler = FindObjectOfType<CubeHandler>();
			playerAnim = FindObjectOfType<PlayerAnimator>();
			juicer = GetComponent<FinishCubeJuicer>();
			loader = FindObjectOfType<SceneHandler>();
			progHandler = FindObjectOfType<ProgressHandler>();
			farter = FindObjectOfType<PlayerFartLauncher>();
		}

		private void OnEnable() 
		{
			if (playerAnim != null)
			{
				playerAnim.onTriggerSerpent += InitiateSerpentSequence;
				playerAnim.onGetFinishPos += GetPos;
			}

			if (juicer != null) juicer.onSpawnFriends += SpawnFriends;
			
			if (farter != null) farter.onSwitchToEndCam += InitiateEndCamSwitch;
		}

		public void InitiateEndSeq()
		{
			StartCoroutine(EndSequence());
		}

		private IEnumerator EndSequence()
		{
			yield return DestroyAllFloorCubes();
			StartCoroutine(SwitchToCloseupCam());
			onUIFade(0);
			yield return new WaitForSeconds(fartDelay);
			farter.InitiateFartSequence(fartTowardsTarget);
		}

		private IEnumerator DestroyAllFloorCubes()
		{
			List<FloorCube> floorCubeList = new List<FloorCube>();

			foreach (KeyValuePair<Vector2Int, FloorCube> pair in handler.floorCubeDic)
			{
				var cube = pair.Value;
				if (cube.type == CubeTypes.Finish) continue;

				floorCubeList.Add(cube);

			}

			for (int i = 0; i < floorCubeList.Count; i++)
			{
				floorCubeList[i].GetComponent<CubeShrinker>().StartShrinking();
				yield return new WaitForSeconds(shrinkInterval);
			}
		}

		private IEnumerator SwitchToCloseupCam()
		{
			fader = FindObjectOfType<Fader>();
			onAlignCam();
			yield return fader.FadeOut(fader.endSeqTransTime);
			closeUpCam.Priority = 11;
			Camera.main.orthographic = false;
			closeUpCam.transform.parent = null;
			fader.FadeIn(fader.endSeqTransTime);
		}

		private void InitiateEndCamSwitch()
		{
			StartCoroutine(SwitchToEndCam());
		}

		private IEnumerator SwitchToEndCam()
		{
			yield return fader.FadeOut(fader.endSeqTransTime);
			endCam.Priority = 12;
			Camera.main.orthographic = true;
			endCam.transform.parent = null;
			fader.FadeIn(fader.endSeqTransTime);
			yield return new WaitForSeconds(flyByDelay);

			Vector3 startPos, endPos;
			CalculateStartEnd(out startPos, out endPos);
			farter.InitiateFlyBy(startPos, endPos);
		}

		private void CalculateStartEnd(out Vector3 startPos, out Vector3 endPos)
		{
			float[] possibleX = new float[2];
			possibleX[0] = -3f;
			possibleX[1] = 4f;

			var index = UnityEngine.Random.Range(0, possibleX.Length);
			float startX = possibleX[index];

			float targetX;
			if (startX > 0) targetX = -4;
			else targetX = 5;

			float startY = UnityEngine.Random.Range(.15f, .85f);
			float targetY = UnityEngine.Random.Range(.15f, .85f);

			startPos = Camera.main.ViewportToWorldPoint(new Vector3(startX, startY, 5));
			endPos = Camera.main.ViewportToWorldPoint(new Vector3(targetX, targetY, -3));
		}

		private void InitiateSerpentSequence()
		{
			StartCoroutine(SerpentSequence());
		}

		private IEnumerator SerpentSequence()
		{
			if (onSerpentCheck()) ActivateSerpent(); //TO DO: eventually these checks should be obsolete bc every level will have serpent
			yield return new WaitForSeconds(2); //TO DO: this should be the length of serpent anim

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

		private void SpawnFriends()
		{
			if (FetchHasSegment())
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
			else if (mapConnected) loader.LoadWorldMap();
			else loader.NextLevel();
		}

		private bool FetchHasSegment()
		{
			return progHandler.currentHasSegment;
		}

		private Vector3 GetPos()
		{
			return transform.position;
		}

		private void OnDisable()
		{
			if (playerAnim != null)
			{
				playerAnim.onTriggerSerpent -= InitiateSerpentSequence;
				playerAnim.onGetFinishPos -= GetPos;
			}

			if (juicer != null) juicer.onSpawnFriends -= SpawnFriends;

			if (farter != null) farter.onSwitchToEndCam -= InitiateEndCamSwitch;
		}
	}
}
