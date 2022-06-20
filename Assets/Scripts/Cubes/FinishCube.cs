using System.Collections;
using System.Collections.Generic;
using Qbism.PlayerCube;
using UnityEngine;
using UnityEngine.Analytics;
using Qbism.Saving;
using System;
using Qbism.General;


namespace Qbism.Cubes
{
	public class FinishCube : MonoBehaviour
	{
		//Config parameters
		[SerializeField] FinishRefHolder refs;

		//Cache
		PlayerCubeMover mover;
		CubeHandler handler;
		ProgressHandler progHandler;
		SerpentProgress serpProg;
		ObjectsProgress objProg;
		FloorCubeChecker floorChecker;
		FeatureSwitchBoard switchBoard;
		PlayerCubeFeedForward playerFF;
		PlayerRefHolder player;

		//States
		Vector2Int myPosition;
		public bool wrongOnFinish { get; set; } = false;
		public bool hasFinished = false;

		private void Awake()
		{
			player = refs.gcRef.pRef;
			mover = player.playerMover;
			playerFF = player.playerFF;
			handler = refs.gcRef.glRef.cubeHandler;
			progHandler = refs.persRef.progHandler;
			serpProg = refs.persRef.serpProg;
			objProg = refs.persRef.objProg;
			floorChecker = refs.gcRef.glRef.floorChecker;
			switchBoard = refs.persRef.switchBoard;
		}

		private void OnEnable()
		{
			if (floorChecker != null) floorChecker.onCheckForFinish += CheckForFinish;
			if (playerFF != null) playerFF.onFinishCheck += FetchFinishStatus;
		}

		private void Start()
		{
			myPosition = new Vector2Int
				(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
		}

		private void CheckForFinish()
		{
			if (handler.FetchCube(myPosition, true) == handler.FetchCube(player.cubePos.FetchGridPos(), true))
			{
				if (Mathf.Approximately(Vector3.Dot(mover.transform.forward, transform.up), -1))
					Finish(false);	

				else
				{
					wrongOnFinish = true;
					refs.gcRef.rewindPulser.InitiatePulse();
				} 
			}
			else if (wrongOnFinish)
			{
				refs.gcRef.rewindPulser.StopPulse();
				wrongOnFinish = false;
			} 
		}

		public void Finish(bool isDebug)
		{
			var screenStateMngr = refs.gcRef.glRef.screenStateMngr;
			screenStateMngr.SwitchState(screenStateMngr.levelEndSeqState, ScreenStates.levelEndSeqState);
			
			SendAnalyticsEvent();

			if (isDebug) PositionPlayerForFinish();

			if (switchBoard.worldMapConnected)
				progHandler.SetLevelToComplete(progHandler.currentPin);

			if (progHandler.currentHasSegment && switchBoard.serpentConnected)
			{
				refs.segSpawner.SetSegmentToSpawn();
				serpProg.AddSegmentToDatabase();
			}
			else if (progHandler.currentHasObject && switchBoard.objectsConnected)
			{
				refs.objSpawner.SetObjectToSpawn();
				objProg.AddObjectToDatabase();
			}

			progHandler.SaveProgData();

			hasFinished = true;
			player.exprHandler.hasFinished = true;
			mover.allowRewind = false;
			mover.allowMoveInput = false;

			refs.finishJuicer.DeactivateGlow();
			refs.finishJuicer.PlaySuccesSound();

			DisableOutOfBounds();

			refs.endSeq.InitiateEndSeq();
		}

		private void SendAnalyticsEvent()
		{
			var levelEntity = E_LevelGameplayData.FindEntity(entity =>
								entity.f_Pin == refs.persRef.progHandler.currentPin);

			if (!levelEntity.f_Completed)
			{
				Analytics.CustomEvent(AnalyticsEvents.LevelsComp, new Dictionary<string, object>
				{
					{AnalyticsEvents.Level, refs.persRef.progHandler.debugCurrentPin},
				});
				Analytics.CustomEvent(AnalyticsEvents.LevelComp + " " + 
					refs.persRef.progHandler.debugCurrentPin, new Dictionary<string, object>
				{
					{AnalyticsEvents.TimeWindow, refs.gcRef.glRef.levelTimer.timeWindow}
				});
			}
			else
			{
				Analytics.CustomEvent(AnalyticsEvents.LevelReplayComp, new Dictionary<string, object>
				{
					{AnalyticsEvents.Level, refs.persRef.progHandler.debugCurrentPin},
				});
			}
		}

		private void PositionPlayerForFinish()
		{
			mover.transform.position = new Vector3(transform.position.x, 
				transform.position.y + 1, transform.position.z);
			mover.transform.forward = Vector3.down;
		}

		public bool FetchFinishStatus()
		{
			return hasFinished;
		}

		private void DisableOutOfBounds()
		{
			var oob = refs.gcRef.outOfBounds;

			for (int i = 0; i < oob.Length; i++)
			{
				oob[i].gameObject.SetActive(false);
			}
		}

		private void OnDisable()
		{
			if (floorChecker != null) floorChecker.onCheckForFinish -= CheckForFinish;
			if (playerFF != null) playerFF.onFinishCheck -= FetchFinishStatus;
		}
	}
}