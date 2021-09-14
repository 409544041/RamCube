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
		}

		private void OnEnable() 
		{
			if(mover != null)
				mover.onInitialRecord += AddInitialPlayerRecording;

			if(handler != null)
				handler.onInitialCubeRecording += AddInitialCubeRecording;

			if(moveHandler != null)
				moveHandler.onInitialCubeRecording += AddInitialMoveableRecording;
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

			handler.shrunkToFloorThisRewind = new Vector2Int(99, 99);
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

			if (body != null)
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

		private void OnDisable()
		{
			if (mover != null)
				mover.onInitialRecord -= AddInitialPlayerRecording; 

			if (handler != null)
				handler.onInitialCubeRecording -= AddInitialCubeRecording;

			if (moveHandler != null)
				moveHandler.onInitialCubeRecording -= AddInitialMoveableRecording;
		}
	}
}
