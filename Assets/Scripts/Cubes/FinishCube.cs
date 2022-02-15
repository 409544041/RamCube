using System.Collections;
using System.Collections.Generic;
using Qbism.PlayerCube;
using UnityEngine;
using Qbism.Saving;
using System;
using Qbism.SpriteAnimations;
using Qbism.General;
using Qbism.Serpent;
using Qbism.Objects;

namespace Qbism.Cubes
{
	public class FinishCube : MonoBehaviour
	{
		//Config parameters
		[SerializeField] FinishEndSeqHandler finishEndSeq;
		[SerializeField] FinishCubeJuicer juicer;
		[SerializeField] SegmentSpawner segSpawner;
		[SerializeField] ObjectSpawner objSpawner;

		//Cache
		PlayerCubeMover mover;
		CubeHandler handler;
		ProgressHandler progHandler;
		SerpentProgress serpProg;
		ObjectsProgress objProg;
		FloorCubeChecker floorChecker;
		FeatureSwitchBoard switchBoard;
		PlayerCubeFeedForward playerFF;

		//States
		Vector2Int myPosition;
		public bool wrongOnFinish { get; set; } = false;
		public bool hasFinished = false;
		
		//Actions, events, delegates etc
		public event Action<InterfaceIDs> onRewindPulse;
		public event Action<InterfaceIDs> onStopRewindPulse;

		private void Awake()
		{
			mover = FindObjectOfType<PlayerCubeMover>();
			playerFF = mover.GetComponent<PlayerCubeFeedForward>();
			handler = FindObjectOfType<CubeHandler>();
			progHandler = FindObjectOfType<ProgressHandler>();
			if (progHandler)
			{
				serpProg = progHandler.GetComponent<SerpentProgress>();
				objProg = progHandler.GetComponent<ObjectsProgress>();
			}
			floorChecker = handler.GetComponent<FloorCubeChecker>();
			switchBoard = progHandler.GetComponent<FeatureSwitchBoard>();
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
			if (switchBoard.allowDebugFinish) PositionPlayerForFinish();

			if (switchBoard.worldMapConnected)
				progHandler.SetLevelToComplete(progHandler.currentPin);

			if (progHandler.currentHasSegment && switchBoard.serpentConnected)
			{
				segSpawner.SetSegmentToSpawn(); //Needs to be done before AddSegment
				serpProg.AddSegmentToDatabase();
			}
			else if (progHandler.currentHasObject && switchBoard.objectsConnected)
			{
				objSpawner.SetObjectToSpawn();
				objProg.AddObjectToDatabase();
			}

			progHandler.SaveProgData();

			hasFinished = true;
			mover.GetComponentInChildren<ExpressionHandler>().hasFinished = true;
			mover.input = false;

			juicer.DeactivateGlow();
			juicer.PlaySuccesSound();

			DisableOutOfBounds();

			finishEndSeq.InitiateEndSeq();
		}

		private void PositionPlayerForFinish()
		{
			mover.transform.position = new Vector3(transform.position.x, 
				transform.position.y + 1, transform.position.z);
			mover.transform.forward = Vector3.down;
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

		private void DisableOutOfBounds()
		{
			var oob = FindObjectsOfType<OutOfBounds>();

			for (int i = 0; i < oob.Length; i++)
			{
				oob[i].gameObject.SetActive(false);
			}
		}

		private void OnDisable()
		{
			if (floorChecker != null) floorChecker.onCheckForFinish -= CheckForFinish;
			if (juicer != null) juicer.onFinishCheck -= FetchFinishStatus;
			if (playerFF != null) playerFF.onFinishCheck -= FetchFinishStatus;
		}
	}
}