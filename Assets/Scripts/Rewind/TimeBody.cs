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
		[SerializeField] CubeRefHolder refs;

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
			if (this.tag == "Player") //TO DO: player refs
			{
				mover = FindObjectOfType<PlayerCubeMover>();
				expresHandler = GetComponentInChildren<ExpressionHandler>();
				//TO DO: Remove once we have player refs
				handler = FindObjectOfType<CubeHandler>();
				moveHandler = handler.GetComponent<MoveableCubeHandler>();
				floorChecker = handler.GetComponent<FloorCubeChecker>();
			}
			else
			{
				handler = refs.gcRef.glRef.cubeHandler;
				moveHandler = refs.gcRef.glRef.movCubeHandler;
				floorChecker = refs.gcRef.glRef.floorChecker;
			}
		}

		private void OnEnable() 
		{
			if (refs != null && refs.movCube != null) 
				refs.movCube.onUpdateOrderInTimebody += AddToMoveOrderList;
		}

		public void InitialRecord(Vector3 pos, Quaternion rot, Vector3 scale)
		{
			rewindList.Insert(0, new PointInTime(pos, rot, scale));

			if (refs != null && refs.floorCube != null)
			{
				var cubePos = refs.cubePos.FetchGridPos();
				AddToStaticList(refs.floorCube);
				AddToShrunkList(refs.floorCube, refs.movCube, cubePos);
			}

			if (refs != null && refs.movCube != null)
			{
				AddToDockedList(refs.movCube);
				AddToOutOfBoundsList(refs.movCube);
				AddToMoveOrderList(-1, refs.movCube);
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
			var cubePos = refs.cubePos.FetchGridPos();
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
			
			if (refs != null && refs.movCube != null)
			{
				ResetDocked(refs.movCube, rewindList[0].position);
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
					// refs.gcRef.rewindPulser.StopPulse(InterfaceIDs.Rewind); //TO DO: Link to player ref
				}
				
				if (mover.isStunned)
				{
					//yield here bc otherwise laser detects cube collider again and player stays stunned
					yield return null;
					mover.isStunned = false;
					// refs.gcRef.rewindPulser.StopPulse(InterfaceIDs.Rewind); //TO DO: Link to player ref
					mover.GetComponent<PlayerStunJuicer>().StopStunVFX();
				}

				mover.gameObject.SendMessage("StartPostRewindJuice");
				mover.GetComponent<PlayerFartLauncher>().ResetFartCollided();
				expresHandler.SetFace(Expressions.smiling, expresHandler.GetRandomTime());
			}

			if (this.tag == "Environment" || this.tag == "Moveable")
			{
				if (refs.floorCube != null)
				{
					ResetStatic(refs.floorCube);
					ResetShrunkStatus(refs.floorCube);
				}

				if (refs.movCube != null)
				{
					refs.cubePos.RoundPosition();
					refs.movCube.UpdateCenterPosition();
					ResetOutOfBounds(refs.movCube);
					movementOrderList.RemoveAt(0);
					
					if (refs.floorCube == null)
					{
						moveHandler.RemoveFromMoveableDic(preRewPos);
						moveHandler.AddToMoveableDic(refs.cubePos.FetchGridPos(), refs.movCube);
					}
				}
			}
		}

		private void ResetShrunkStatus(FloorCube cube)
		{
			if (hasShrunkList.Count <= 0) return;
			var cubePos = refs.cubePos.FetchGridPos();

			if (refs.movCube != null)
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
			refs.cubeShrink.EnableMesh();
			refs.nmCutter.enabled = true;
			handler.FromShrunkToFloorDic(cubePos, cube);
		}

		private void ResetStatic(FloorCube cube)
		{
			if(isStaticList.Count <= 0) return;

			if(isStaticList[0] == CubeTypes.Static &&
				cube.type == CubeTypes.Shrinking)
			{
				cube.type = CubeTypes.Static;
				refs.staticFace.SetActive(true);
			}

			isStaticList.RemoveAt(0);
		}	

		private void ResetDocked(MoveableCube moveable, Vector3 rewindPos) 
		{
			var cubePos = refs.cubePos.FetchGridPos();
			var rewPos = new Vector2Int(Mathf.RoundToInt(rewindPos.x), Mathf.RoundToInt(rewindPos.z));

			if(isDockedList.Count > 0 && isDockedList[0] == false 
				&& handler.movFloorCubeDic.ContainsKey(cubePos))
			{
				this.tag = "Moveable";
				Destroy(refs.floorCube);
				refs.lineRender.enabled = false;
				handler.movFloorCubeDic.Remove(cubePos);
				moveHandler.moveableCubeDic.Add(rewPos, moveable);
				moveable.gameObject.SendMessage("StartPostRewindJuice");

				if (refs.movEffector != null)
				{
					if (refs.movEffector.effectorType == CubeTypes.Boosting)
						Destroy(refs.boostCube);

					else if (refs.movEffector.effectorType == CubeTypes.Turning)
						Destroy(refs.turnCube);

					else if (refs.movEffector.effectorType == CubeTypes.Static)
						Destroy(refs.staticCube);
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
				refs.mesh.enabled = true;
			}

			isOutOfBoundsList.RemoveAt(0);
		}

		private void OnDisable()
		{
			if (refs != null && refs.movCube != null) 
				refs.movCube.onUpdateOrderInTimebody -= AddToMoveOrderList;
		}
	}
}
