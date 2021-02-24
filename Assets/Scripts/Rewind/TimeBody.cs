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

		private List<PointInTime> rewindList = new List<PointInTime>(); //TO DO: Make private afterwards
		private List<bool> isFindableList = new List<bool>();
		private List<bool> hasShrunkList = new List<bool>();
		private List<CubeTypes> isStaticList = new List<CubeTypes>();
		private List<bool> isDockedList = new List<bool>();

		private void Awake() 
		{
			mover = FindObjectOfType<PlayerCubeMover>();
			moveHandler = FindObjectOfType<MoveableCubeHandler>();
			handler = FindObjectOfType<CubeHandler>();
		}

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

				if (cube.hasShrunk == true) hasShrunkList.Insert(0, true);
				else hasShrunkList.Insert(0, false);
			}
							
			if(moveable)
			{
				if (moveable.isDocked == true) isDockedList.Insert(0, true);
				else isDockedList.Insert(0, false); 
			}
		}

		public void Rewind()
		{	
			if(rewindList.Count <= 0) return;

			if (this.tag == "Player")
			{
				mover.RoundPosition();
				mover.UpdateCenterPosition();
				mover.GetComponent<PlayerCubeFeedForward>().ShowFeedForward();
				handler.currentCube = handler.FetchCube(mover.FetchGridPos());
			}

			if (this.tag == "Environment" || this.tag == "Moveable")
			{
				var cube = GetComponent<FloorCube>();
				if (cube)
				{
					//if(cube.isShrinking) cube.StopShrinking(); //----- TO DO: wait on MM response. Fix this.
					ResetStatic(cube);
					ResetShrunkStatus(cube);
					SetIsFindable(cube);
				}

				var moveable = GetComponent<MoveableCube>();
				if (moveable)
				{
					ResetDocked(moveable);
				}
			}

			transform.position = rewindList[0].position;
			transform.rotation = rewindList[0].rotation;
			transform.localScale = rewindList[0].scale;
			rewindList.RemoveAt(0);
		}

		//----- TO DO: Check if record moment for each of these is good (it's wrong for docked)
		private void SetIsFindable(FloorCube cube) //----- TO DO: What is this even? Do we need it still?
		{
			if (isFindableList.Count > 0 && isFindableList[0] == true && cube.isFindable == false)
				cube.isFindable = true;
			
			isFindableList.RemoveAt(0);
		}

		private void ResetShrunkStatus(FloorCube cube)
		{			
			if(hasShrunkList.Count > 0 && hasShrunkList[0] == false && cube.hasShrunk == true)
				cube.hasShrunk = false;
			
			hasShrunkList.RemoveAt(0);
		}

		private void ResetStatic(FloorCube cube)
		{
			if(isStaticList.Count > 0 && isStaticList[0] == CubeTypes.Static &&
				cube.type == CubeTypes.Shrinking)
			{
				cube.type = CubeTypes.Static;

				Material[] mats = GetComponent<Renderer>().materials;
				mats[2].SetTexture("_BaseMap", GetComponent<StaticCube>().staticFaceTex);
			}

			isStaticList.RemoveAt(0);
		}	

		private void ResetDocked(MoveableCube moveable)
		{
			moveable.RoundPosition();
			moveable.UpdateCenterPosition();

			if(isDockedList.Count > 0 && isDockedList[0] == false && moveable.isDocked == true)
			{
				this.tag = "Moveable";
				moveable.isDocked = false;
				Destroy(GetComponent<FloorCube>());
			}

			isDockedList.RemoveAt(0);
		}	
	}
}
