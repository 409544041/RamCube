﻿using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.Rewind
{
	public class RewindHandler : MonoBehaviour
	{
		//Config parameters
		public int rewindsAmount;

		//Cache
		TimeBody[] timeBodies = null;
		PlayerCubeMover mover;
		CubeHandler handler;
		List<FloorCube> floorCubes = new List<FloorCube>();

		//States
		int timesRewindUsed = 0;

		private void Awake() 
		{
			timeBodies = FindObjectsOfType<TimeBody>();
			foreach (TimeBody timebody in timeBodies)
			{
				timebody.rewindAmount = rewindsAmount;
			}

			mover = FindObjectOfType<PlayerCubeMover>();
			handler = GetComponent<CubeHandler>();

			FloorCube[] floorCubesAtStart = FindObjectsOfType<FloorCube>();
			foreach (FloorCube cube in floorCubesAtStart)
			{
				floorCubes.Add(cube);
			}
		}

		private void OnEnable() 
		{
			if(mover != null)
			{
				mover.onInitialRecord += AddInitialPlayerRecording;
				mover.onRecordStart += StartRecordingPlayer;
				mover.onRecordStart += ResetTimesRewinded;
				mover.onCheckForNewFloor += CheckForNewFloor;
			} 

			if(handler != null)
			{
				handler.onRecordStop += StopRecordingPlayer;
				handler.onFloorRecord += ShiftLists;
				handler.onFloorRecord += StartRecordingCubes;
			} 

			if(floorCubes != null)
			{
				foreach (FloorCube floorCube in floorCubes)
				{
					floorCube.onListShift += ShiftLists;
					floorCube.onInitialRecord += AddInitialCubeRecording;
					floorCube.onRecordStart += StartRecordingCubes;
					floorCube.onRecordStop += StopRecordingCubes;
				}
			}
		}

		public void StartRewinding()
		{
			if(mover.input == false) return; 
			
			foreach (TimeBody timeBody in timeBodies)
			{
				timeBody.timesRewinded = timesRewindUsed;
				timeBody.StartRewinding();
			}

			timesRewindUsed++;
			if(rewindsAmount > 0) rewindsAmount--;
		}

		private void ShiftLists()
		{
			foreach (TimeBody timeBody in timeBodies)
			{
				if (timeBody.tag == "Player") continue;

				timeBody.ShiftLists();
			}
		}

		private void AddInitialPlayerRecording(Vector3 pos, Quaternion rot, Vector3 scale)
		{
			mover.GetComponent<TimeBody>().ShiftLists();
			mover.GetComponent<TimeBody>().InitialRecord(pos, rot, scale);
		}

		private void StartRecordingPlayer()
		{
			mover.GetComponent<TimeBody>().isRecording = true;
		}

		private void StopRecordingPlayer()
		{
			mover.GetComponent<TimeBody>().isRecording = false;
		}

		private void AddInitialCubeRecording(FloorCube cube, Vector3 pos, Quaternion rot, Vector3 scale)
		{
			cube.GetComponent<TimeBody>().InitialRecord(pos, rot, scale);
		}

		private void StartRecordingCubes()
		{
			foreach (TimeBody timeBody in timeBodies)
			{
				if (timeBody.tag == "Player") continue;

				timeBody.isRecording = true;
			}
		}

		private void StopRecordingCubes()
		{
			foreach (TimeBody timeBody in timeBodies)
			{
				if (timeBody.tag == "Player") continue;

				timeBody.isRecording = false;
			}
		}

		private void CheckForNewFloor()
		{
			FloorCube[] floorCubesAtCheck = FindObjectsOfType<FloorCube>();
			foreach (FloorCube cube in floorCubesAtCheck)
			{
				if(!floorCubes.Contains(cube))
				{
					floorCubes.Add(cube);
					cube.onListShift += ShiftLists;
					cube.onInitialRecord += AddInitialCubeRecording;
					cube.onRecordStart += StartRecordingCubes;
					cube.onRecordStop += StopRecordingCubes;
				}
			}
		}

		private void ResetTimesRewinded()
		{
			timesRewindUsed = 0;
		}

		private void OnDisable()
		{
			if (mover != null)
			{
				mover.onInitialRecord -= AddInitialPlayerRecording;
				mover.onRecordStart -= StartRecordingPlayer;
				mover.onRecordStart -= ResetTimesRewinded;
				mover.onCheckForNewFloor += CheckForNewFloor;
			} 

			if (handler != null)
			{
				handler.onRecordStop -= StopRecordingPlayer;
				handler.onFloorRecord -= ShiftLists;
				handler.onFloorRecord -= StartRecordingCubes;
			} 

			if (floorCubes != null)
			{
				foreach (FloorCube floorCube in floorCubes)
				{
					floorCube.onListShift -= ShiftLists;
					floorCube.onInitialRecord -= AddInitialCubeRecording;
					floorCube.onRecordStart -= StartRecordingCubes;
					floorCube.onRecordStop -= StopRecordingCubes;
				}
			}
		}
	}
}
