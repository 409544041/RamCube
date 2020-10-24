using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using Qbism.MoveableCubes;
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
		PlayerCubeMover mover = null;
		CubeHandler handler = null;
		List<FloorCube> floorCubes = new List<FloorCube>();
		MoveableCubeHandler moveHandler = null;
		MoveableCube[] moveableCubes;

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
			moveHandler = handler.GetComponent<MoveableCubeHandler>();
			moveableCubes = FindObjectsOfType<MoveableCube>();

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
			} 

			if(handler != null)
			{
				handler.onRecordStop += StopRecordingPlayer;
				handler.onInitialCubeRecording += AddInitialCubeRecording;
			} 

			if(floorCubes != null)
			{
				foreach (FloorCube floorCube in floorCubes)
				{
					floorCube.onRecordStart += StartRecordingCubes;
					floorCube.onRecordStop += StopRecordingCubes;
				}
			}

			if(moveHandler != null)
			{
				moveHandler.onRecordStart += StartRecordingMoveables;
				moveHandler.onRecordStop += StopRecordingMoveables;
				moveHandler.onInitialCubeRecording += AddInitialMoveableRecording;
			}

			if(moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onCheckForNewFloorCubes += CheckForNewFloorCubes;
					cube.onRecordStop += StopRecordingSpecificMoveable;
				}
			}

			if(timeBodies != null)
			{
				foreach(TimeBody body in timeBodies)
				{
					body.onRewindCheck += CheckForRewinds;
				}
			}
		}

		public void StartRewinding()
		{
			if(mover.input == false) return; 
			
			foreach (TimeBody timeBody in timeBodies)
			{
				moveHandler.moveableCubeDic.Clear();
				timeBody.timesRewinded = timesRewindUsed;
				timeBody.StartRewinding();
			}

			timesRewindUsed++;
			if(rewindsAmount > 0) rewindsAmount--;
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
			var body = cube.GetComponent<TimeBody>();

			if(body != null)
			{
				cube.GetComponent<TimeBody>().ShiftLists();
				cube.GetComponent<TimeBody>().InitialRecord(pos, rot, scale);
			}
		}

		private void StartRecordingCubes(FloorCube cube)
		{
			cube.GetComponent<TimeBody>().isRecording = true;
		}

		private void StopRecordingCubes(FloorCube cube)
		{
			cube.GetComponent<TimeBody>().isRecording = false;
		}

		private void AddInitialMoveableRecording(MoveableCube cube, Vector3 pos, Quaternion rot, Vector3 scale)
		{
			var body = cube.GetComponent<TimeBody>();

			if (body != null)
			{
				cube.GetComponent<TimeBody>().ShiftLists();
				cube.GetComponent<TimeBody>().InitialRecord(pos, rot, scale);
			}
		}

		private void StartRecordingMoveables(MoveableCube cube)
		{
			var body = cube.GetComponent<TimeBody>();

			if (body != null) body.isRecording = true;
		}

		private void StopRecordingMoveables()
		{
			foreach (TimeBody timeBody in timeBodies)
			{
				if (timeBody.tag == "Moveable")
					timeBody.isRecording = false;
			}
		}

		private void StopRecordingSpecificMoveable(MoveableCube cube)
		{
			cube.GetComponent<TimeBody>().isRecording = false;
		}

		private bool CheckForRewinds()
		{	
			bool bodyRewinding = false;
			foreach (TimeBody timeBody in timeBodies)
			{
				if(timeBody.isRewinding)
				{
					bodyRewinding = true;
					return true;
				}
			}
			return bodyRewinding;
		}

		private void CheckForNewFloorCubes()
		{
			FloorCube[] floorCubesAtCheck = FindObjectsOfType<FloorCube>();
			foreach (FloorCube cube in floorCubesAtCheck)
			{
				if(!floorCubes.Contains(cube))
				{
					floorCubes.Add(cube);
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
			} 

			if (handler != null)
			{
				handler.onRecordStop -= StopRecordingPlayer;
				handler.onInitialCubeRecording -= AddInitialCubeRecording;
			} 

			if (floorCubes != null)
			{
				foreach (FloorCube floorCube in floorCubes)
				{
					floorCube.onRecordStart -= StartRecordingCubes;
					floorCube.onRecordStop -= StopRecordingCubes;
				}
			}

			if (moveHandler != null)
			{
				moveHandler.onRecordStart -= StartRecordingMoveables;
				moveHandler.onRecordStop -= StopRecordingMoveables;
				moveHandler.onInitialCubeRecording -= AddInitialMoveableRecording;
			}

			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onCheckForNewFloorCubes -= CheckForNewFloorCubes;
					cube.onRecordStop -= StopRecordingSpecificMoveable;
				}
			}

			if (timeBodies != null)
			{
				foreach (TimeBody body in timeBodies)
				{
					body.onRewindCheck -= CheckForRewinds;
				}
			}
		}
	}
}
