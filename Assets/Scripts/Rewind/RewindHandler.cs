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
		//Config parameters
		[SerializeField] GameLogicRefHolder glRef;

		//Cache
		TimeBody[] timeBodies;
		PlayerCubeMover mover;
		CubeHandler handler;
		MoveableCubeHandler moveHandler;
		LaserRefHolder[] laserRefs;
		FinishCube finish;

		private void Awake() 
		{
			timeBodies = glRef.gcRef.timeBodies;
			mover = glRef.gcRef.pRef.playerMover;
			handler = glRef.cubeHandler;
			moveHandler = glRef.movCubeHandler;
			laserRefs = glRef.gcRef.laserRefs;
			finish = glRef.gcRef.finishRef.finishCube;
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
			if (finish.wrongOnFinish) glRef.gcRef.rewindPulser.StopPulse();

			StartCoroutine(DelayedLaserRewindStuff());

			handler.shrunkToFloorThisRewind = new Vector2Int(99, 99);
		}

		private void RewindTimeBodies()
		{
			SortedDictionary<int, TimeBody> rewindFirstDic = CreateRewindFirstDic();
			PriorityRewind(rewindFirstDic);
			NormalRewind();
			ResetPriorityRewindValue(rewindFirstDic);
		}

		private SortedDictionary<int, TimeBody> CreateRewindFirstDic()
		{
			SortedDictionary<int, TimeBody> rewindFirstDic =
							new SortedDictionary<int, TimeBody>();

			//Order in which moveables get rewinded is important to avoid dic errors
			foreach (TimeBody body in timeBodies)
			{
				var refs = body.cubeRef;

				MoveableCube moveable = null;
				if (refs != null) moveable = body.cubeRef.movCube;

				if (refs != null && moveable != null)
				{
					if (moveable.orderOfMovement == -1) break;

					rewindFirstDic.Add(moveable.orderOfMovement, body);
					body.priorityRewind = true;
				}
			}
			return rewindFirstDic;
		}

		private static void PriorityRewind(SortedDictionary<int, TimeBody> rewindFirstDic)
		{
			for (int j = 0; j < rewindFirstDic.Count; j++)
			{
				rewindFirstDic[j].StartRewind();
			}
		}

		private void NormalRewind()
		{
			foreach (var body in timeBodies)
			{
				if (!body.priorityRewind) body.StartRewind();
			}
		}

		private static void ResetPriorityRewindValue(SortedDictionary<int, TimeBody> rewindFirstDic)
		{
			foreach (var pair in rewindFirstDic)
			{
				pair.Value.priorityRewind = false;
			}
		}

		private void LaserRewindStuff()
		{
			if (laserRefs.Length > 0)
			{
				foreach (var laserRef in laserRefs)
				{
					//To make sure there's no delay on turning on laser again upon rewind
					laserRef.laser.laserPause = false;
				}
			}
		}

		private IEnumerator DelayedLaserRewindStuff()
		{
			yield return new WaitForSeconds(.05f); 
			//to avoid laser reading player eventhough player already rewinded

			if (laserRefs.Length > 0)
			{
				foreach (var laserRef in laserRefs)
				{
					laserRef.laser.shouldTrigger = true;
				}
			}
		}

		private void AddInitialPlayerRecording(Vector3 pos, Quaternion rot, Vector3 scale)
		{
			glRef.gcRef.pRef.timeBody.InitialRecord(pos, rot, scale);
		}

		private void AddInitialCubeRecording(CubeRefHolder cubeRef, Vector3 pos, Quaternion rot, Vector3 scale)
		{
			var body = cubeRef.timeBody;

			if (body != null)
			{
				body.InitialRecord(pos, rot, scale);
			}
		}

		private void AddInitialMoveableRecording(CubeRefHolder cubeRef, Vector3 pos, Quaternion rot, Vector3 scale)
		{
			var body = cubeRef.timeBody;

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
