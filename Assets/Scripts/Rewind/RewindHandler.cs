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

		//States
		public Dictionary<Vector2Int, MoveableCube> movRewindDic { get; private set; }
			= new Dictionary<Vector2Int, MoveableCube>();
		public Dictionary<Vector2Int, MoveableCube> dockedMovRewindDic { get; private set; }
			= new Dictionary<Vector2Int, MoveableCube>();
		public Dictionary<Vector2Int, MoveableCube> shrunkMovRewindDic { get; private set; }
			= new Dictionary<Vector2Int, MoveableCube>();

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
			if (!mover.allowRewind) return;
			
			RewindTimeBodies();

			if (finish.wrongOnFinish) glRef.gcRef.rewindPulser.StopPulse();
			LaserRewindStuff();

			handler.shrunkToFloorThisRewind = new Vector2Int(99, 99);
		}

		private void RewindTimeBodies()
		{
			FillMovRewindDic();
			NormalRewind();
		}

		private void FillMovRewindDic()
		{
			movRewindDic.Clear();
			dockedMovRewindDic.Clear();
			shrunkMovRewindDic.Clear();

			foreach (TimeBody body in timeBodies)
			{
				var refs = body.cubeRef;

				MoveableCube moveable = null;
				FloorCube floor = null;
				if (refs != null) floor = body.cubeRef.floorCube;
				if (refs != null) moveable = body.cubeRef.movCube;
				if (moveable == null) continue;
				
				var cubePos = refs.cubePos.FetchGridPos();

				if (moveHandler.moveableCubeDic.ContainsKey(cubePos))
				{
					moveHandler.moveableCubeDic.Remove(cubePos);
					movRewindDic.Add(cubePos, moveable);
				}
				else if (handler.movFloorCubeDic.ContainsKey(cubePos))
				{
					handler.movFloorCubeDic.Remove(cubePos);
					dockedMovRewindDic.Add(cubePos, moveable);
					
				}
				else if (handler.shrunkMovFloorCubeDic.ContainsKey(cubePos) &&
					handler.shrunkMovFloorCubeDic[cubePos][0] == floor)
				{
					handler.FromShrunkToFloor(cubePos, null);
					shrunkMovRewindDic.Add(cubePos, moveable);
				}
			}
		}

		private void NormalRewind()
		{
			foreach (var body in timeBodies)
			{
				body.StartRewind();
			}
		}

		private void LaserRewindStuff()
		{
			if (laserRefs.Length > 0)
			{
				foreach (var laserRef in laserRefs)
				{
					laserRef.laser.HandleLaser();
				}
			}
		}

		private void AddInitialPlayerRecording(Vector3 pos, Quaternion rot, Vector3 scale, Quaternion faceRot, Vector3 faceScale)
		{
			glRef.gcRef.pRef.timeBody.InitialRecord(pos, rot, scale, faceRot, faceScale);
		}

		private void AddInitialCubeRecording(CubeRefHolder cubeRef, Vector3 pos, Quaternion rot, Vector3 scale, Quaternion faceRot, Vector3 faceScale)
		{
			var body = cubeRef.timeBody;

			if (body != null)
			{
				body.InitialRecord(pos, rot, scale, faceRot, faceScale);
			}
		}

		private void AddInitialMoveableRecording(CubeRefHolder cubeRef, Vector3 pos, Quaternion rot, Vector3 scale, Quaternion faceRot, Vector3 faceScale)
		{
			var body = cubeRef.timeBody;

			if (body != null)
			{
				body.InitialRecord(pos, rot, scale, faceRot, faceScale);
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
