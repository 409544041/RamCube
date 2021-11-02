using System.Collections;
using System.Collections.Generic;
using Qbism.PlayerCube;
using UnityEngine;
using Qbism.Saving;
using System;
using Qbism.SpriteAnimations;
using Qbism.General;

namespace Qbism.Cubes
{
	public class FinishCube : MonoBehaviour
	{
		//Cache
		PlayerCubeMover mover;
		CubeHandler handler;
		FinishCubeJuicer juicer;
		ProgressHandler progHandler;
		SerpentProgress serpProg;
		FinishEndSeqHandler finishEndSeq;
		FloorCubeChecker floorChecker;
		FeatureSwitchBoard switchBoard;
		PlayerCubeFeedForward playerFF;

		//States
		Vector2Int myPosition;
		public bool wrongOnFinish { get; set; } = false;
		bool hasFinished = false;
		
		//Actions, events, delegates etc
		public event Action onSetSegment;
		public event Action<InterfaceIDs> onRewindPulse;
		public event Action<InterfaceIDs> onStopRewindPulse;

		private void Awake()
		{
			mover = FindObjectOfType<PlayerCubeMover>();
			playerFF = mover.GetComponent<PlayerCubeFeedForward>();
			handler = FindObjectOfType<CubeHandler>();
			juicer = GetComponent<FinishCubeJuicer>();
			progHandler = FindObjectOfType<ProgressHandler>();
			if (progHandler) serpProg = progHandler.GetComponent<SerpentProgress>();
			finishEndSeq = GetComponent<FinishEndSeqHandler>();
			floorChecker = handler.GetComponent<FloorCubeChecker>();
			if (progHandler) switchBoard = progHandler.
				GetComponent<FeatureSwitchBoard>();
			else switchBoard = handler.
				GetComponent<FeatureSwitchBoard>();
		}

		private void OnEnable()
		{
			if (floorChecker != null) floorChecker.onCheckForFinish += CheckForFinish;
			if (juicer != null) juicer.onFinishCheck += FetchFinishStatus;
			if (playerFF != null) playerFF.onFinishCheck += FetchFinishStatus;
		}

		private void Start()
		{
			myPosition = new Vector2Int
				(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
		}

		private void CheckForFinish()
		{
			if (handler.FetchCube(myPosition, true) == handler.FetchCube(mover.cubePoser.FetchGridPos(), true))
			{
				if (Mathf.Approximately(Vector3.Dot(mover.transform.forward, transform.up), -1))
					Finish();	

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

		public void Finish()
		{
			if (switchBoard.worldMapConnected) //TO DO: eventually these checks should be obsolete bc map should always be available and a level is always started via map
			{
				progHandler.SetLevelToComplete(progHandler.currentPin, true);
			}

			if (progHandler.currentHasSegment)
			{
				if (switchBoard.serpentConnected)
				{
					onSetSegment(); //Needs to be done before AddSegment
					serpProg.AddSegment();
				}
			}

			progHandler.SaveProgData();

			hasFinished = true;
			mover.GetComponentInChildren<ExpressionHandler>().hasFinished = true;

			juicer.DeactivateGlow();
			juicer.PlaySuccesSound();

			finishEndSeq.InitiateEndSeq();
		}

		private void PulseRewindUI()
		{
			onRewindPulse(InterfaceIDs.Rewind);
		}

		public void StopPulseRewindUI()
		{
			onStopRewindPulse(InterfaceIDs.Rewind);
		}

		public bool FetchFinishStatus()
		{
			return hasFinished;
		}

		private void OnDisable()
		{
			if (floorChecker != null) floorChecker.onCheckForFinish -= CheckForFinish;
			if (juicer != null) juicer.onFinishCheck -= FetchFinishStatus;
			if (playerFF != null) playerFF.onFinishCheck -= FetchFinishStatus;
		}
	}
}