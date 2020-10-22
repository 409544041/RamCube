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
				mover.onMoveableListShift += ShiftListsForMoveables;
			} 

			if(handler != null)
			{
				handler.onRecordStop += StopRecordingPlayer;
				handler.onFloorRecord += ShiftListsForCubes;
				handler.onFloorRecord += StartRecordingCubes;
			} 

			if(floorCubes != null)
			{
				foreach (FloorCube floorCube in floorCubes)
				{
					floorCube.onListShift += ShiftListsForCubes;
					floorCube.onInitialRecord += AddInitialCubeRecording;
					floorCube.onRecordStart += StartRecordingCubes;
					floorCube.onRecordStop += StopRecordingCubes;
				}
			}

			if(moveHandler != null)
			{
				moveHandler.onRecordStart += StartRecordingMoveables;
				moveHandler.onRecordStop += StopRecordingMoveables;
			}

			if(moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onInitialRecord += AddInitialMoveableRecording;
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

		private void ShiftListsForCubes()
		{
			foreach (TimeBody timeBody in timeBodies)
			{
				if (timeBody.tag == "Environment")
					timeBody.ShiftLists();
			}
		}

		private void AddInitialCubeRecording(FloorCube cube, Vector3 pos, Quaternion rot, Vector3 scale)
		{
			cube.GetComponent<TimeBody>().InitialRecord(pos, rot, scale);
		}

		private void StartRecordingCubes()
		{
			foreach (TimeBody timeBody in timeBodies)
			{
				if (timeBody.tag == "Environment")
					timeBody.isRecording = true;
			}
		}

		private void StopRecordingCubes()
		{
			foreach (TimeBody timeBody in timeBodies)
			{
				if (timeBody.tag == "Environment")
					timeBody.isRecording = false;
			}
		}

		private void ShiftListsForMoveables()
		{
			foreach (TimeBody timeBody in timeBodies)
			{
				if (timeBody.tag == "Moveable")
					timeBody.ShiftLists();
			}
		}

		private void AddInitialMoveableRecording(MoveableCube cube, Vector3 pos, Quaternion rot, Vector3 scale)
		{
			cube.GetComponent<TimeBody>().InitialRecord(pos, rot, scale);
		}

		private void StartRecordingMoveables()
		{
			foreach (TimeBody timeBody in timeBodies)
			{
				if (timeBody.tag == "Moveable")
					timeBody.isRecording = true;
			}
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
					cube.onListShift += ShiftListsForCubes;
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
				mover.onMoveableListShift -= ShiftListsForMoveables;
			} 

			if (handler != null)
			{
				handler.onRecordStop -= StopRecordingPlayer;
				handler.onFloorRecord -= ShiftListsForCubes;
				handler.onFloorRecord -= StartRecordingCubes;
			} 

			if (floorCubes != null)
			{
				foreach (FloorCube floorCube in floorCubes)
				{
					floorCube.onListShift -= ShiftListsForCubes;
					floorCube.onInitialRecord -= AddInitialCubeRecording;
					floorCube.onRecordStart -= StartRecordingCubes;
					floorCube.onRecordStop -= StopRecordingCubes;
				}
			}

			if (moveHandler != null)
			{
				moveHandler.onRecordStart -= StartRecordingMoveables;
				moveHandler.onRecordStop -= StopRecordingMoveables;
			}

			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onInitialRecord -= AddInitialMoveableRecording;
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
