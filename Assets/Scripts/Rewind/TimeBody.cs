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

		//States
		public bool priorityRewind { get; set; } = false;

		private List<PointInTime> rewindList = new List<PointInTime>(); 
		private List<bool> hasShrunkList = new List<bool>();
		private List<CubeTypes> isStaticList = new List<CubeTypes>();
		private List<bool> isDockedList = new List<bool>();
		private List<bool> isOutOfBoundsList = new List<bool>();
		public List<int> movementOrderList { get; set; } = new List<int>();

		private void Awake() 
		{
			if (pRef != null)
			{
				mover = pRef.playerMover;
				expresHandler = pRef.exprHandler;
				handler = pRef.gcRef.glRef.cubeHandler;
				moveHandler = pRef.gcRef.glRef.movCubeHandler;
				floorChecker = pRef.gcRef.glRef.floorChecker;
			}
			else
			{
				handler = cubeRef.gcRef.glRef.cubeHandler;
				moveHandler = cubeRef.gcRef.glRef.movCubeHandler;
				floorChecker = cubeRef.gcRef.glRef.floorChecker;
			}
		}

		private void OnEnable() 
		{
			if (cubeRef != null && cubeRef.movCube != null) 
				cubeRef.movCube.onUpdateOrderInTimebody += AddToMoveOrderList;
		}

		public void InitialRecord(Vector3 pos, Quaternion rot, Vector3 scale)
		{
			rewindList.Insert(0, new PointInTime(pos, rot, scale));

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
				AddToMoveOrderList(-1, cubeRef.movCube);
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

		private void AddToMoveOrderList(int order, MoveableCube moveable)
		{
			if (order == -1) movementOrderList.Insert(0, moveable.orderOfMovement);
			else movementOrderList[0] = order;
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
			
			if (cubeRef != null && cubeRef.movCube != null)
			{
				ResetDocked(cubeRef.movCube, rewindList[0].position);
			} 
			
			transform.position = rewindList[0].position;
			transform.rotation = rewindList[0].rotation;
			transform.localScale = rewindList[0].scale;
			rewindList.RemoveAt(0);

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
				}
				
				if (mover.isStunned)
				{
					//yield here bc otherwise laser detects cube collider again and player stays stunned
					yield return null;
					mover.isStunned = false;
					pRef.gcRef.rewindPulser.StopPulse();
					pRef.stunJuicer.StopStunVFX();
				}

				pRef.rewindJuicer.StartPostRewindJuice();
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
					movementOrderList.RemoveAt(0);
					
					if (cubeRef.floorCube == null)
					{
						moveHandler.RemoveFromMoveableDic(preRewPos);
						moveHandler.AddToMoveableDic(cubeRef.cubePos.FetchGridPos(), cubeRef.movCube);
					}
				}
			}
		}

		private void ResetShrunkStatus(FloorCube cube)
		{
			if (hasShrunkList.Count <= 0) return;
			var cubePos = cubeRef.cubePos.FetchGridPos();

			if (cubeRef.movCube != null)
			{
				if (hasShrunkList[0] == false
					&& handler.shrunkMovFloorCubeDic.ContainsKey(cubePos))
				{
					if (handler.shrunkMovFloorCubeDic[cubePos][0] == cube)
						ResetShrinking(cube, cubePos);
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

			hasShrunkList.RemoveAt(0);
		}

		private void ResetShrinking(FloorCube cube, Vector2Int cubePos)
		{
			cubeRef.cubeShrink.EnableMesh();
			handler.FromShrunkToFloorDic(cubePos, cube);
		}

		private void ResetStatic(FloorCube cube)
		{
			if(isStaticList.Count <= 0) return;

			if(isStaticList[0] == CubeTypes.Static &&
				cube.type == CubeTypes.Shrinking)
			{
				cube.type = CubeTypes.Static;
				cubeRef.staticFace.SetActive(true);
			}

			isStaticList.RemoveAt(0);
		}	

		private void ResetDocked(MoveableCube moveable, Vector3 rewindPos) 
		{
			var cubePos = cubeRef.cubePos.FetchGridPos();
			var rewPos = new Vector2Int(Mathf.RoundToInt(rewindPos.x), Mathf.RoundToInt(rewindPos.z));

			if(isDockedList.Count > 0 && isDockedList[0] == false 
				&& handler.movFloorCubeDic.ContainsKey(cubePos))
			{
				this.tag = "Moveable";
				Destroy(cubeRef.floorCube);
				cubeRef.lineRender.enabled = false;
				handler.movFloorCubeDic.Remove(cubePos);
				moveHandler.moveableCubeDic.Add(rewPos, moveable);
				cubeRef.rewindJuicer.StartPostRewindJuice();

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

		private void OnDisable()
		{
			if (cubeRef != null && cubeRef.movCube != null) 
				cubeRef.movCube.onUpdateOrderInTimebody -= AddToMoveOrderList;
		}
	}
}
