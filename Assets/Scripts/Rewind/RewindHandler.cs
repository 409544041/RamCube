using System;
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
		//Cache
		TimeBody[] timeBodies = null;
		PlayerCubeMover mover = null;
		CubeHandler handler = null;
		List<FloorCube> floorCubes = new List<FloorCube>();
		MoveableCubeHandler moveHandler = null;
		MoveableCube[] moveableCubes;

		//Actions, events, delegates etc
		public event Action<InterfaceIDs> onStopRewindPulse;

		private void Awake() 
		{
			timeBodies = FindObjectsOfType<TimeBody>();
			mover = FindObjectOfType<PlayerCubeMover>();
			handler = GetComponent<CubeHandler>();
			moveHandler = handler.GetComponent<MoveableCubeHandler>();
			moveableCubes = FindObjectsOfType<MoveableCube>();

			FloorCube[] floorCubesAtStart = FindObjectsOfType<FloorCube>(); //TO DO: What is up with this list? Doesn't seem to be used anywhere?
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
			} 

			if(handler != null)
			{
				handler.onInitialCubeRecording += AddInitialCubeRecording;
			} 

			if(moveHandler != null)
			{
				moveHandler.onInitialCubeRecording += AddInitialMoveableRecording;
			}

			if(moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onCheckForNewFloorCubes += CheckForNewFloorCubes;
				}
			}
		}

		private void Update()
		{
			CheckForMovement();
		}

		//Makes sure that after moveables stopped moving they are added to correct dic
		private void CheckForMovement() 
		{
			if(mover.isBoosting || mover.isMoving || mover.isTurning) return;

			if(!moveHandler.CheckForMovingMoveables())
			{
				moveHandler.moveableCubeDic.Clear();
				moveHandler.LoadMoveableCubeDictionary();
				mover.input = true;
			}
		}

		private IEnumerator ReloadDics() 
		{
			yield return new WaitForSeconds(.1f); 
			//Without this extra time floorcubedic would register just rewinded moveables as floorcubes
			
			handler.floorCubeDic.Clear();
			handler.LoadFloorCubeDictionary();
			moveHandler.moveableCubeDic.Clear();
			moveHandler.LoadMoveableCubeDictionary();
		}

		public void StartRewinding()
		{
			if(mover.input == false) return; 
			
			foreach (TimeBody timeBody in timeBodies)
			{
				timeBody.Rewind();
			}

			//To stop rewind UI element from pulsing if rewinding off finish. Here bc finish doesn't have timebody component
			var finish = FindObjectOfType<FinishCube>();
			if (finish.wrongOnFinish)
				onStopRewindPulse(InterfaceIDs.Rewind);

			StartCoroutine(ReloadDics());
		}

		private void AddInitialPlayerRecording(Vector3 pos, Quaternion rot, Vector3 scale)
		{
			mover.GetComponent<TimeBody>().InitialRecord(pos, rot, scale);
		}

		private void AddInitialCubeRecording(FloorCube cube, Vector3 pos, Quaternion rot, Vector3 scale)
		{
			var body = cube.GetComponent<TimeBody>();

			if(body != null)
			{
				body.InitialRecord(pos, rot, scale);
			}
		}

		private void AddInitialMoveableRecording(MoveableCube cube, Vector3 pos, Quaternion rot, Vector3 scale)
		{
			var body = cube.GetComponent<TimeBody>();

			if (body != null)
			{
				body.InitialRecord(pos, rot, scale);
			}
		}

		private void CheckForNewFloorCubes()
		{
			FloorCube[] floorCubesAtCheck = FindObjectsOfType<FloorCube>();
			foreach (FloorCube cube in floorCubesAtCheck)
			{
				if(!floorCubes.Contains(cube) && cube.isFindable)
				{
					floorCubes.Add(cube);
				}
			}
		}

		private void OnDisable()
		{
			if (mover != null)
			{
				mover.onInitialRecord -= AddInitialPlayerRecording;
			} 

			if (handler != null)
			{
				handler.onInitialCubeRecording -= AddInitialCubeRecording;
			} 

			if (moveHandler != null)
			{
				moveHandler.onInitialCubeRecording -= AddInitialMoveableRecording;
			}

			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onCheckForNewFloorCubes -= CheckForNewFloorCubes;
				}
			}
		}
	}
}
