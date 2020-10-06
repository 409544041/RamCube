using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
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

		//Cache
		Rigidbody rb = null;
		PlayerCubeMover mover;

		public Dictionary<int, List<PointInTime>> listDictionary = new Dictionary<int, List<PointInTime>> ();

		private void Awake() 
		{
			rb = GetComponent<Rigidbody>();
			mover = FindObjectOfType<PlayerCubeMover>();
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
			if(rewindAmount == 0) return;
			if(isRewinding) Rewind();
			if(isRecording) Record();
		}

		public void ShiftLists()
		{
			for (int i = rewindAmount - 1; i > 0; i--)
			{
				if (listDictionary[i - 1].Count > 0)
					listDictionary[i] = listDictionary[i - 1];
			}

			if (listDictionary[0].Count > 0)
			{
				listDictionary[0] = new List<PointInTime>();
			}
		}

		public void InitialRecord(Vector3 pos, Quaternion rot, Vector3 scale)
		{
			listDictionary[0].Insert(0, new PointInTime(pos, rot, scale));
		}

		public void Record()
		{
			listDictionary[0].Insert(0,
				new PointInTime(transform.position, transform.rotation, transform.localScale));
		}

		public void StartRewinding()
		{
			isRewinding = true;
			rb.isKinematic = true;
		}

		private void Rewind()
		{
			
			if(listDictionary[timesRewinded].Count > 0)
			{
				if(gameObject.tag == "Player") mover.input = false;
				transform.position = listDictionary[timesRewinded][0].position;
				transform.rotation = listDictionary[timesRewinded][0].rotation;
				transform.localScale = listDictionary[timesRewinded][0].scale;
				listDictionary[timesRewinded].RemoveAt(0);
			}
			else
			{
				isRewinding = false;
				rewindAmount--;
				if (gameObject.tag == "Player")
				{
					mover.input = true;
					mover.UpdateCenterPosition();
					mover.RoundPosition();
				}
				if (gameObject.tag == "Environment") 
				{
					AddBackToDictionary();
				}
				
			} 
		}

		private void AddBackToDictionary()
		{	
			FloorCube cube = GetComponent<FloorCube>();
			Vector2Int cubePos = cube.FetchGridPos();

			if (cube.hasShrunk == true && cubePos == mover.FetchGridPos())
			{
				CubeHandler handler = FindObjectOfType<CubeHandler>();
				if(!handler.CheckIfContainsKey(cubePos))
					handler.floorCubeGrid.Add(cubePos, cube);
				cube.hasShrunk = false;
			}
		}
	}
}
