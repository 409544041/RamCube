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
		//Cache
		PlayerCubeMover mover;
		MoveableCubeHandler moveHandler;
		CubeHandler handler;
		FloorCubeChecker floorChecker;
		MoveableCube myMoveable;
		ExpressionHandler expresHandler;

		//States
		public bool priorityRewind { get; set; } = false;

		private List<PointInTime> rewindList = new List<PointInTime>(); 
		private List<bool> hasShrunkList = new List<bool>();
		private List<CubeTypes> isStaticList = new List<CubeTypes>();
		private List<bool> isDockedList = new List<bool>();
		private List<bool> isOutOfBoundsList = new List<bool>();
		public List<int> movementOrderList { get; set; } = new List<int>();

		//Actions, events, delegates etc
		public event Action<InterfaceIDs> onStopRewindPulse;

		private void Awake() 
		{
			if (this.tag == "Player")
			{
				mover = FindObjectOfType<PlayerCubeMover>();
				expresHandler = GetComponentInChildren<ExpressionHandler>();
			}

			handler = FindObjectOfType<CubeHandler>();
			moveHandler = handler.GetComponent<MoveableCubeHandler>();
			floorChecker = handler.GetComponent<FloorCubeChecker>();
			if (this.tag == "Moveable") myMoveable = GetComponent<MoveableCube>();
		}

		private void OnEnable() 
		{
			if (myMoveable != null) myMoveable.onUpdateOrderInTimebody += AddToMoveOrderList;
		}

		public void InitialRecord(Vector3 pos, Quaternion rot, Vector3 scale)
		{
			var cube = GetComponent<FloorCube>();
			var moveable = GetComponent<MoveableCube>();

			rewindList.Insert(0, new PointInTime(pos, rot, scale));

			if (cube)
			{
				var cubePos = cube.cubePoser.FetchGridPos();
				AddToStaticList(cube);
				AddToShrunkList(cube, moveable, cubePos);
			}

			if (moveable)
			{
				AddToDockedList(moveable);
				AddToOutOfBoundsList(moveable);
				AddToMoveOrderList(-1, moveable);
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
			var cubePos = moveable.cubePoser.FetchGridPos();
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

			var moveable = GetComponent<MoveableCube>();
			
			if (moveable)
			{
				ResetDocked(moveable, rewindList[0].position);
			} 
			
			transform.position = rewindList[0].position;
			transform.rotation = rewindList[0].rotation;
			transform.localScale = rewindList[0].scale;
			rewindList.RemoveAt(0);

			if (this.tag == "Player")
			{
				mover.cubePoser.RoundPosition();
				mover.UpdateCenterPosition();
				mover.GetComponent<PlayerCubeFeedForward>().ShowFeedForward();
				floorChecker.currentCube = handler.FetchCube(mover.cubePoser.FetchGridPos(), false);

				if (mover.isOutOfBounds || mover.isLowered)
				{
					mover.isOutOfBounds = false;
					mover.isLowered = false;
					onStopRewindPulse(InterfaceIDs.Rewind);
				}
				
				if (mover.isStunned)
				{
					//yield here bc otherwise laser detects cube collider again and player stays stunned
					yield return null;
					mover.isStunned = false;
					onStopRewindPulse(InterfaceIDs.Rewind);
					mover.GetComponent<PlayerStunJuicer>().StopStunVFX();
				}

				mover.gameObject.SendMessage("StartPostRewindJuice");
				mover.GetComponent<PlayerFartLauncher>().ResetFartCollided();
				expresHandler.SetFace(Expressions.smiling, expresHandler.GetRandomTime());
			}

			if (this.tag == "Environment" || this.tag == "Moveable")
			{
				var cube = GetComponent<FloorCube>();
				if (cube)
				{
					ResetStatic(cube);
					ResetShrunkStatus(cube);
				}

				if (moveable)
				{
					moveable.cubePoser.RoundPosition();
					moveable.UpdateCenterPosition();
					ResetOutOfBounds(moveable);
					movementOrderList.RemoveAt(0);
					
					if (!cube)
					{
						moveHandler.RemoveFromMoveableDic(preRewPos);
						moveHandler.AddToMoveableDic(moveable.cubePoser.FetchGridPos(), moveable);
					}
				}
			}
		}

		private void ResetShrunkStatus(FloorCube cube)
		{
			if (hasShrunkList.Count <= 0) return;
			var cubePos = cube.cubePoser.FetchGridPos();

			if (cube.GetComponent<MoveableCube>())
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
			CubeShrinker shrinker = cube.GetComponent<CubeShrinker>();
			shrinker.EnableMesh();
			shrinker.nmCutter.enabled = true;
			handler.FromShrunkToFloorDic(cubePos, cube);
		}

		private void ResetStatic(FloorCube cube)
		{
			if(isStaticList.Count <= 0) return;

			if(isStaticList[0] == CubeTypes.Static &&
				cube.type == CubeTypes.Shrinking)
			{
				cube.type = CubeTypes.Static;
				cube.GetComponent<StaticCube>().face.SetActive(true);
			}

			isStaticList.RemoveAt(0);
		}	

		private void ResetDocked(MoveableCube moveable, Vector3 rewindPos) 
		{
			var cubePos = moveable.cubePoser.FetchGridPos();
			var rewPos = new Vector2Int(Mathf.RoundToInt(rewindPos.x), Mathf.RoundToInt(rewindPos.z));

			if(isDockedList.Count > 0 && isDockedList[0] == false 
				&& handler.movFloorCubeDic.ContainsKey(cubePos))
			{
				this.tag = "Moveable";
				Destroy(GetComponent<FloorCube>());
				moveable.laserLine.enabled = false;
				handler.movFloorCubeDic.Remove(cubePos);
				moveHandler.moveableCubeDic.Add(rewPos, moveable);
				moveable.gameObject.SendMessage("StartPostRewindJuice");

				if (moveable.moveEffector != null)
				{
					if (moveable.moveEffector.effectorType == CubeTypes.Boosting)
						Destroy(GetComponent<BoostCube>());

					else if (moveable.moveEffector.effectorType == CubeTypes.Turning)
						Destroy(GetComponent<TurningCube>());

					else if (moveable.moveEffector.effectorType == CubeTypes.Static)
						Destroy(GetComponent<StaticCube>());
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
				moveable.mesh.enabled = true;
			}

			isOutOfBoundsList.RemoveAt(0);
		}

		private void OnDisable()
		{
			if (myMoveable != null) myMoveable.onUpdateOrderInTimebody -= AddToMoveOrderList;
		}
	}
}
