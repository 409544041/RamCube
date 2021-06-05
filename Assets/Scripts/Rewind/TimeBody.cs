using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using Qbism.MoveableCubes;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.Rewind
{
	public class TimeBody : MonoBehaviour
	{
		//Cache
		PlayerCubeMover mover;
		MoveableCubeHandler moveHandler;
		CubeHandler handler;

		private List<PointInTime> rewindList = new List<PointInTime>(); 
		private List<bool> isFindableList = new List<bool>();
		private List<bool> hasShrunkList = new List<bool>();
		private List<CubeTypes> isStaticList = new List<CubeTypes>();
		private List<bool> isDockedList = new List<bool>();
		private List<bool> isOutOfBoundsList = new List<bool>();

		//Actions, events, delegates etc
		public event Action<InterfaceIDs> onStopRewindPulse;

		private void Awake() 
		{
			mover = FindObjectOfType<PlayerCubeMover>();
			moveHandler = FindObjectOfType<MoveableCubeHandler>();
			handler = FindObjectOfType<CubeHandler>();
		}

		//Is initiated by player move
		public void InitialRecord(Vector3 pos, Quaternion rot, Vector3 scale)
		{
			var cube = GetComponent<FloorCube>();
			var moveable = GetComponent<MoveableCube>();

			rewindList.Insert(0, new PointInTime(pos, rot, scale));

			if (cube)
			{
				if (cube.isFindable) isFindableList.Insert(0, true);
				else isFindableList.Insert(0, false);

				if (cube.type == CubeTypes.Static) isStaticList.Insert(0, CubeTypes.Static);
				else if (cube.type == CubeTypes.Shrinking) isStaticList.Insert(0, CubeTypes.Shrinking);

				CubeShrinker shrinker = cube.GetComponent<CubeShrinker>();
				if (shrinker && shrinker.hasShrunk == true) 
					hasShrunkList.Insert(0, true);
				else hasShrunkList.Insert(0, false);
			}
							
			if(moveable)
			{
				if (moveable.isDocked == true) isDockedList.Insert(0, true);
				else isDockedList.Insert(0, false); 

				if (moveable.isOutOfBounds == true) isOutOfBoundsList.Insert(0, true);
				else isOutOfBoundsList.Insert(0, false);
			}
		}

		public void Rewind()
		{	
			if(rewindList.Count <= 0) return;

			transform.position = rewindList[0].position;
			transform.rotation = rewindList[0].rotation;
			transform.localScale = rewindList[0].scale;
			rewindList.RemoveAt(0);

			if (this.tag == "Player")
			{
				mover.RoundPosition();
				mover.UpdateCenterPosition();
				mover.GetComponent<PlayerCubeFeedForward>().ShowFeedForward();
				handler.currentCube = handler.FetchCube(mover.FetchGridPos());

				if (mover.isOutOfBounds)
				{
					mover.isOutOfBounds = false;
					onStopRewindPulse(InterfaceIDs.Rewind);
				}
				
				if (mover.isStunned)
				{
					mover.isStunned = false;
					onStopRewindPulse(InterfaceIDs.Rewind);
					mover.GetComponent<PlayerStunJuicer>().StopStunVFX();
				}

				mover.gameObject.SendMessage("StartPostRewindJuice");
			}

			if (this.tag == "Environment" || this.tag == "Moveable")
			{
				var cube = GetComponent<FloorCube>();
				if (cube)
				{
					ResetStatic(cube);
					ResetShrunkStatus(cube);
					SetIsFindable(cube);
				}

				var moveable = GetComponent<MoveableCube>();
				if (moveable)
				{
					ResetDocked(moveable);
					ResetOutOfBounds(moveable);
				}
			}
		}

		//isFindable is removed from a floorcube if a moveable becomes a floorcube on a location of the old (shrunk) floorcube
		//floor cubes with isFindable are added to floorcubedic after moveables get docked. So the old shrunk floorcube isn't added to avoid dic overlap
		private void SetIsFindable(FloorCube cube)
		{
			if(isFindableList.Count <= 0) return;

			if (isFindableList[0] == true && cube.isFindable == false)
				cube.isFindable = true;
			
			isFindableList.RemoveAt(0);
		}

		private void ResetShrunkStatus(FloorCube cube)
		{
			if (hasShrunkList.Count <= 0) return;
			CubeShrinker shrinker = cube.GetComponent<CubeShrinker>();

			if(hasShrunkList[0] == false 
				&& shrinker.hasShrunk == true)
				{
					shrinker.hasShrunk = false;
					shrinker.EnableMesh();
				}
		
			hasShrunkList.RemoveAt(0);
		}

		private void ResetStatic(FloorCube cube)
		{
			if(isStaticList.Count <= 0) return;

			if(isStaticList[0] == CubeTypes.Static &&
				cube.type == CubeTypes.Shrinking)
			{
				cube.type = CubeTypes.Static;

				Material[] mats = GetComponentInChildren<Renderer>().materials;
				mats[2].SetTexture("_BaseMap", GetComponent<StaticCube>().staticFaceTex);
			}

			isStaticList.RemoveAt(0);
		}	

		private void ResetDocked(MoveableCube moveable) 
		{
			moveable.RoundPosition();
			moveable.UpdateCenterPosition();

			if(isDockedList.Count > 0 && isDockedList[0] == false 
				&& moveable.isDocked == true)
			{
				this.tag = "Moveable";
				moveable.isDocked = false;
				Destroy(GetComponent<FloorCube>());
				Destroy(GetComponent<CubeShrinker>());
				moveable.gameObject.SendMessage("StartPostRewindJuice");
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
	}
}
