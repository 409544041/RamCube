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
		//States
		public bool isRewinding { get; set; } = false;
		public bool isRecording { get; set; } = false;
		public int rewindAmount { get; set; } = 0;
		public int timesRewinded { get; set; } = 0;
		bool originPosSaved = false;
		Vector2Int rewindOriginPos = new Vector2Int(0,0);
		Vector3 rewindOriginTransform;

		//Cache
		PlayerCubeMover mover;
		MoveableCubeHandler moveHandler;
		CubeHandler handler;

		public delegate bool RewindCheckDelegate();
		public RewindCheckDelegate onRewindCheck;

		public Dictionary<int, List<PointInTime>> listDictionary = new Dictionary<int, List<PointInTime>>();
		public List<Vector2Int> firstPosList { get; set; } = new List<Vector2Int>();
		private List<bool> isFindableList = new List<bool>();

		private void Awake() 
		{
			mover = FindObjectOfType<PlayerCubeMover>();
			moveHandler = FindObjectOfType<MoveableCubeHandler>();
			handler = FindObjectOfType<CubeHandler>();
		}

		private void Start() 
		{	
			for (int i = 0; i < rewindAmount; i++)
			{
				listDictionary.Add(i, new List<PointInTime>());
			}
		}

		private void FixedUpdate() 
		{
			if(isRewinding) Rewind();
			if(isRecording) Record();
		}

		public void ShiftLists()
		{
			for (int i = rewindAmount - 1; i > 0; i--)
			{
				if (listDictionary[i - 1].Count > 0)
					listDictionary[i] = listDictionary[i - 1];

				if(this.tag == "Player" && firstPosList.Count > i)
					firstPosList[i] = firstPosList[i - 1];
				
				if(this.tag == "Environment" && isFindableList.Count > i)
					isFindableList[i] = isFindableList[i -1];
			}

			if (listDictionary[0].Count > 0)
			{
				listDictionary[0] = new List<PointInTime>();
			}
		}

		public void InitialRecord(Vector3 pos, Quaternion rot, Vector3 scale)
		{
			listDictionary[0].Insert(0, new PointInTime(pos, rot, scale));

			if(this.tag == "Player")
			{
				Vector2Int firstPos = new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z));
				firstPosList.Insert(0, firstPos);
			}

			if(this.tag == "Environment")
			{
				if(GetComponent<FloorCube>().isFindable) isFindableList.Insert(0, true);
				else isFindableList.Insert(0, false);
			}
		}

		public void Record()
		{
			listDictionary[0].Insert(0,
				new PointInTime(transform.position, transform.rotation, transform.localScale));
		}

		public void StartRewinding()
		{
			isRewinding = true;
			if (this.tag == "Player" || this.tag == "Moveable") mover.input = false;
			if (this.tag == "Moveable") originPosSaved = false;
		}

		private void Rewind()
		{	
			if(listDictionary[timesRewinded].Count > 0)
			{
				if(!originPosSaved) 
				{
					rewindOriginPos = new Vector2Int(Mathf.RoundToInt(listDictionary[timesRewinded][0].position.x), 
						Mathf.RoundToInt(listDictionary[timesRewinded][0].position.z));

					rewindOriginTransform = listDictionary[timesRewinded][0].position;

					originPosSaved = true;
				}

				transform.position = listDictionary[timesRewinded][0].position;
				transform.rotation = listDictionary[timesRewinded][0].rotation;
				transform.localScale = listDictionary[timesRewinded][0].scale;
				listDictionary[timesRewinded].RemoveAt(0);				
			}

			else //at end of rewind
			{
				isRewinding = false;
				rewindAmount--;
				if(!onRewindCheck()) mover.input = true;

				if (this.tag == "Player")
				{
					mover.RoundPosition();
					mover.UpdateCenterPosition();
					mover.GetComponent<PlayerCubeFeedForward>().ShowFeedForward();
					handler.currentCube = handler.FetchCube(mover.FetchGridPos());
				}

				if (this.tag == "Environment")
				{
					ResetStatic();
					ResetShrunkStatus();
					SetIsFindable();
				}

				if (GetComponent<MoveableCube>())
				{
					var moveable = GetComponent<MoveableCube>();

					if (Mathf.Approximately(Mathf.Abs(rewindOriginTransform.y - transform.position.y), 0)
						&& !GetComponent<FloorCube>())
					{
						moveable.RoundPosition();
						moveable.UpdateCenterPosition();
					}

					else if(Mathf.Approximately(Mathf.Abs(rewindOriginTransform.y - transform.position.y), 1)
						&& GetComponent<FloorCube>())
					{
						moveable.RoundPosition();
						moveable.UpdateCenterPosition();
						Destroy(GetComponent<FloorCube>());
						this.tag = "Moveable";
						moveable.isDocked = false;
					}
				}
			} 
		}

		private void SetIsFindable()
		{
			var cube = GetComponent<FloorCube>();

			if (isFindableList.Count > 0 && isFindableList[timesRewinded] == true && 
				cube.isFindable == false)
			{
				cube.isFindable = true;
			}
		}

		private void ResetShrunkStatus()
		{
			FloorCube cube;
			bool samePos;
			CheckIfSamePos(out cube, out samePos);

			if (cube.hasShrunk == true && samePos )
				cube.hasShrunk = false;
		}

		private void ResetStatic()
		{
			FloorCube cube;
			bool samePos;
			CheckIfSamePos(out cube, out samePos);

			if(cube.hasShrunk == false && samePos && 
				GetComponent<StaticCube>() && cube.type == CubeTypes.Shrinking)
			{
				cube.type = CubeTypes.Static;
				GetComponent<MeshRenderer>().material = 
					GetComponent<StaticCube>().staticCubeMat;
			}	
		}

		private void CheckIfSamePos(out FloorCube cube, out bool samePos)
		{
			cube = GetComponent<FloorCube>();
			Vector2Int cubePos = cube.FetchGridPos();

			samePos = cubePos == mover.GetComponent<TimeBody>().firstPosList[timesRewinded];
		}

		
	}
}
