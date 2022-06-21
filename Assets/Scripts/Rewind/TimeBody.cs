using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using Qbism.MoveableCubes;
using Qbism.PlayerCube;
using Qbism.SpriteAnimations;
using UnityEngine;

namespace Qbism.Rewind
{
	public class TimeBody : MonoBehaviour
	{
		//Config parameters
		public CubeRefHolder cubeRef;
		[SerializeField] PlayerRefHolder pRef;

		//Cache
		PlayerCubeMover mover;
		MoveableCubeHandler moveHandler;
		CubeHandler handler;
		FloorCubeChecker floorChecker;
		ExpressionHandler expresHandler;
		RewindHandler rewHandler;

		//States
		public List<PointInTime> rewindList { get; private set; } = new List<PointInTime>(); 
		private List<bool> hasShrunkList = new List<bool>();
		private List<CubeTypes> isStaticList = new List<CubeTypes>();
		private List<bool> isDockedList = new List<bool>();
		private List<bool> isOutOfBoundsList = new List<bool>();

		private void Awake() 
		{
			if (pRef != null)
			{
				mover = pRef.playerMover;
				expresHandler = pRef.exprHandler;
				handler = pRef.gcRef.glRef.cubeHandler;
				moveHandler = pRef.gcRef.glRef.movCubeHandler;
				floorChecker = pRef.gcRef.glRef.floorChecker;
				rewHandler = pRef.gcRef.glRef.rewindHandler;
			}
			else
			{
				handler = cubeRef.gcRef.glRef.cubeHandler;
				moveHandler = cubeRef.gcRef.glRef.movCubeHandler;
				floorChecker = cubeRef.gcRef.glRef.floorChecker;
				rewHandler = cubeRef.gcRef.glRef.rewindHandler;
			}
		}

		public void InitialRecord(Vector3 pos, Quaternion rot, Vector3 scale, Quaternion faceRot, Vector3 faceScale)
		{
			rewindList.Insert(0, new PointInTime(pos, rot, scale, faceRot, faceScale));
			if (cubeRef != null && cubeRef.floorCube != null)
			{
				var cubePos = cubeRef.cubePos.FetchGridPos();
				AddToStaticList(cubeRef.floorCube);
				AddToShrunkList(cubeRef.floorCube, cubeRef.movCube, cubePos);
			}

			if (cubeRef != null && cubeRef.movCube != null)
			{
				AddToDockedList(cubeRef.movCube);
				AddToOutOfBoundsList(cubeRef.movCube);
			}
		}

		private void AddToStaticList(FloorCube cube)
		{
			if (cube.type == CubeTypes.Static) isStaticList.Insert(0, CubeTypes.Static);
			else if (cube.type == CubeTypes.Shrinking) isStaticList.Insert(0, CubeTypes.Shrinking);
		}

		private void AddToShrunkList(FloorCube cube, MoveableCube moveable, Vector2Int cubePos)
		{
			if (moveable)
			{
				var shrunkMovFound = false;

				if (handler.shrunkMovFloorCubeDic.ContainsKey(cubePos))
				{
					foreach (var floorCube in handler.shrunkMovFloorCubeDic[cubePos])
					{
						if (floorCube == cube)
						{
							hasShrunkList.Insert(0, true);
							shrunkMovFound = true;
							break;
						}
					}

					if (!shrunkMovFound) hasShrunkList.Insert(0, false);
				}
				else hasShrunkList.Insert(0, false);
			}
			else
			{
				if (handler.shrunkFloorCubeDic.ContainsKey(cubePos))
					hasShrunkList.Insert(0, true);
				else hasShrunkList.Insert(0, false);
			}
		}

		private void AddToDockedList(MoveableCube moveable)
		{
			var cubePos = cubeRef.cubePos.FetchGridPos();
			if (moveHandler.moveableCubeDic.ContainsKey(cubePos))
				isDockedList.Insert(0, false);
			else isDockedList.Insert(0, true);
		}

		private void AddToOutOfBoundsList(MoveableCube moveable)
		{
			if (moveable.isOutOfBounds == true) isOutOfBoundsList.Insert(0, true);
			else isOutOfBoundsList.Insert(0, false);
		}

		public void StartRewind()
		{
			if (rewindList.Count <= 0) return;
			StartCoroutine(Rewind());
		}

		public IEnumerator Rewind()
		{
			var preRewPos = new Vector2Int(Mathf.RoundToInt(transform.position.x),
				Mathf.RoundToInt(transform.position.z));

			if (pRef != null) mover.isRewinding = true;
			
			if (cubeRef != null && cubeRef.movCube != null)
			{
				ResetDocked(cubeRef.movCube, rewindList[0].position);
			}

			transform.position = rewindList[0].position;
			transform.rotation = rewindList[0].rotation;
			transform.localScale = rewindList[0].scale;
			
			if (pRef != null)
			{
				pRef.cubePos.RoundPosition();
				mover.UpdateCenterPosition();
				pRef.playerFF.ShowFeedForward();
				floorChecker.currentCube = handler.FetchCube(pRef.cubePos.FetchGridPos(), false);

				if (mover.isOutOfBounds || mover.isLowered)
				{
					mover.isOutOfBounds = false;
					mover.isLowered = false;
					pRef.gcRef.rewindPulser.StopPulse();
					mover.allowMoveInput = true;
				}
				
				if (mover.isStunned)
				{
					mover.isStunned = false;
					pRef.gcRef.rewindPulser.StopPulse();
					pRef.stunJuicer.StopStunVFX();
				}

				if (!mover.isResetting) pRef.rewindJuicer.StartPostRewindJuice();
				else if (rewindList.Count == 1) pRef.rewindJuicer.StartPostRewindJuice();

				pRef.fartLauncher.ResetFartCollided();
				expresHandler.SetFace(Expressions.smiling, expresHandler.GetRandomTime());
			}

			if (this.tag == "Environment" || this.tag == "Moveable")
			{
				if (cubeRef.floorCube != null)
				{
					ResetStatic(cubeRef.floorCube);
					ResetShrunkStatus(cubeRef.floorCube);
				}

				if (cubeRef.movCube != null)
				{
					cubeRef.cubePos.RoundPosition();
					cubeRef.movCube.UpdateCenterPosition();
					ResetOutOfBounds(cubeRef.movCube);
					var rewPos = new Vector2Int(Mathf.RoundToInt(rewindList[0].position.x),
						Mathf.RoundToInt(rewindList[0].position.z));
					if (preRewPos != rewPos) cubeRef.rewindJuicer.StartPostRewindJuice();

					if (cubeRef.movFaceMesh != null)
					{
						cubeRef.movFaceMesh.transform.rotation = rewindList[0].faceRot;
						cubeRef.movFaceMesh.transform.localScale = rewindList[0].faceScale;
					}

					if (cubeRef.floorCube == null)
						moveHandler.AddToMoveableDic(cubeRef.cubePos.FetchGridPos(), cubeRef.movCube);
					else handler.AddToMovFloorCubeDic(cubeRef.cubePos.FetchGridPos(), cubeRef.floorCube);
				}
			}

			rewindList.RemoveAt(0);
			yield return null;

			if (mover != null)
			{
				mover.isRewinding = false;
				mover.isResetting = false;
			}
		}

		private void ResetShrunkStatus(FloorCube cube)
		{
			if (hasShrunkList.Count <= 0) return;
			var cubePos = cubeRef.cubePos.FetchGridPos();

			if (cubeRef.movCube != null)
			{
				if (rewHandler.shrunkMovRewindDic.ContainsKey(cubePos))
				{
					if (hasShrunkList[0] == false)
						ResetShrinking(cube, cubePos);
					else handler.HandleFromFloorToShrunk(cubePos, cube, null, 
						handler.shrunkMovFloorCubeDic);
				}
			}
			else
			{
				if (hasShrunkList[0] == false
					&& handler.shrunkFloorCubeDic.ContainsKey(cubePos))
				{
					ResetShrinking(cube, cubePos);
				}
			}
			cubeRef.shrinkMesh.enabled = false;
			hasShrunkList.RemoveAt(0);
		}

		private void ResetShrinking(FloorCube cube, Vector2Int cubePos)
		{
			cubeRef.cubeShrink.EnableMesh();
			if (cube.refs.movCube == null) handler.FromShrunkToFloor(cubePos, cube);

			if (cube.refs.movEffector != null) cube.refs.effectorShrinkingFace.transform.parent =
				cube.refs.effectorFace.transform;
		}

		private void ResetStatic(FloorCube cube)
		{
			if(isStaticList.Count <= 0) return;

			if(isStaticList[0] == CubeTypes.Static &&
				cube.type == CubeTypes.Shrinking)
			{
				cube.type = CubeTypes.Static;
				cubeRef.effectorFace.gameObject.SetActive(true);
				cubeRef.effectorFace.enabled = true;
			}

			isStaticList.RemoveAt(0);
		}	

		private void ResetDocked(MoveableCube moveable, Vector3 rewindPos) 
		{
			var cubePos = cubeRef.cubePos.FetchGridPos();
			var rewPos = new Vector2Int(Mathf.RoundToInt(rewindPos.x), Mathf.RoundToInt(rewindPos.z));

			if(isDockedList.Count > 0 && isDockedList[0] == false 
				&& rewHandler.dockedMovRewindDic.ContainsKey(cubePos))
			{
				this.tag = "Moveable";
				Destroy(cubeRef.floorCube);
				cubeRef.floorCube = null;
				cubeRef.lineRender.enabled = false;
				if (moveable.refs.cubeUI != null) moveable.refs.cubeUI.showCubeUI = false;
				moveable.refs.floorCompAdder.markOnGround.enabled = false;
				moveable.refs.cubeShrink.ResetTransform();

				if (cubeRef.movEffector != null)
				{
					if (cubeRef.movEffector.effectorType == CubeTypes.Boosting)
						Destroy(cubeRef.boostCube);

					else if (cubeRef.movEffector.effectorType == CubeTypes.Turning)
						Destroy(cubeRef.turnCube);

					else if (cubeRef.movEffector.effectorType == CubeTypes.Static)
						Destroy(cubeRef.staticCube);
				}
			}

			isDockedList.RemoveAt(0);
		}	

		private void ResetOutOfBounds(MoveableCube moveable)
		{
			if (isOutOfBoundsList.Count > 0 && isOutOfBoundsList[0] == false &&
				moveable.isOutOfBounds == true)
			{
				moveable.isOutOfBounds = false;
				cubeRef.mesh.enabled = true;
			}

			isOutOfBoundsList.RemoveAt(0);
		}
	}
}
