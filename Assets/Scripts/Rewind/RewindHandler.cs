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
		LaserCube[] lasers;
		FinishCube finish;

		//Actions, events, delegates etc
		public event Action<InterfaceIDs> onStopRewindPulse;

		private void Awake() 
		{
			timeBodies = FindObjectsOfType<TimeBody>();
			mover = FindObjectOfType<PlayerCubeMover>();
			handler = GetComponent<CubeHandler>();
			moveHandler = handler.GetComponent<MoveableCubeHandler>();
			moveableCubes = FindObjectsOfType<MoveableCube>();
			lasers = FindObjectsOfType<LaserCube>();
			finish = FindObjectOfType<FinishCube>();

			FloorCube[] floorCubesAtStart = FindObjectsOfType<FloorCube>(); //TO DO: What is up with this list? Doesn't seem to be used anywhere?
			foreach (FloorCube cube in floorCubesAtStart)
			{
				floorCubes.Add(cube);
			}
		}

		private void OnEnable() 
		{
			if(mover != null)
				mover.onInitialRecord += AddInitialPlayerRecording;

			if(handler != null)
				handler.onInitialCubeRecording += AddInitialCubeRecording;

			if(moveHandler != null)
				moveHandler.onInitialCubeRecording += AddInitialMoveableRecording;

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
			if(mover.isBoosting || mover.isMoving || mover.isTurning || 
				mover.isInIntroSeq) return;

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
			if (!mover.input) return;

			LaserRewindStuff();
			
			RewindTimeBodies();

			//To stop rewind UI element from pulsing if rewinding off finish
			if (finish.wrongOnFinish)
				onStopRewindPulse(InterfaceIDs.Rewind);

			StartCoroutine(DelayedLaserRewindStuff());

			StartCoroutine(ReloadDics());
		}

		private void RewindTimeBodies()
		{
			foreach (TimeBody timeBody in timeBodies)
			{
				timeBody.StartRewind();
			}
		}

		private void LaserRewindStuff()
		{
			if (lasers.Length > 0)
			{
				foreach (var laser in lasers)
				{
					//To make sure there's no delay on turning on laser again upon rewind
					laser.laserPause = false;
				}
			}
		}

		private IEnumerator DelayedLaserRewindStuff()
		{
			yield return new WaitForSeconds(.05f); 
			//to avoid laser reading player eventhough player already rewinded

			if (lasers.Length > 0)
			{
				foreach (var laser in lasers)
				{
					laser.shouldTrigger = true;
				}
			}
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
				mover.onInitialRecord -= AddInitialPlayerRecording; 

			if (handler != null)
				handler.onInitialCubeRecording -= AddInitialCubeRecording;

			if (moveHandler != null)
				moveHandler.onInitialCubeRecording -= AddInitialMoveableRecording;

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
