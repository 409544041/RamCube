using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.SceneTransition;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class LevelPin : MonoBehaviour
	{
		//Config parameters
		public LevelIDs levelID;
		public Biomes biome;
		[SerializeField] float lockedYPos;
		public float unlockedYPos;
		[SerializeField] float raiseStep;
		[SerializeField] float raiseSpeed;
		public Transform pathPoint;

		//Cache
		MeshRenderer mRender;
		LineRenderer[] lRenders;

		//Actions, events, delegates etc
		public event Action<Transform, LineRenderer[]> onPathDrawing;
		public event Action<Transform, List<Transform>, LineRenderer[]> onPathCreation;
		public event Action<LevelPin, bool> onShowOrHideUI;

		//States
		public bool justCompleted { get; set; } = false;
		bool raising = false;

		private void Awake() 
		{
			mRender = GetComponentInChildren<MeshRenderer>();
			lRenders = GetComponentsInChildren<LineRenderer>();
		}

		public void CheckRaiseStatus(bool unlocked, bool unlockAnimPlayed)
		{
			if (!unlocked)
			{
				mRender.enabled = false;
				mRender.transform.position = new Vector3 (transform.position.x, lockedYPos, transform.position.z);
			} 

			else if (unlocked && unlockAnimPlayed)
			{
				mRender.enabled = true;
				mRender.transform.position = new Vector3
				(transform.position.x, unlockedYPos, transform.position.z);
			}
		}

		public void CheckPathStatus(LevelStatusData unlock1Data, LevelStatusData unlock2Data, bool completed)
		{
			List<Transform> destinationList = new List<Transform>();

			AddToList(completed, unlock1Data.unlocked, unlock1Data.unlockAnimPlayed,
				unlock1Data.levelID, destinationList);
			AddToList(completed, unlock2Data.unlocked, unlock2Data.unlockAnimPlayed,
			unlock2Data.levelID, destinationList);

			onPathCreation(pathPoint, destinationList, lRenders);
		}

		private void AddToList(bool completed, bool unlockStatus, bool unlockAnim, 
			LevelIDs ID, List<Transform> destinationList)
		{		
			if(completed && unlockStatus && unlockAnim)
			{
				if(ID == LevelIDs.empty) return;

				LevelPin[] pins = FindObjectsOfType<LevelPin>();
				foreach(LevelPin pin in pins)
				{
					if(pin.levelID == ID) destinationList.Add(pin.pathPoint);
				}
			}
		}

		public void InitiateRaising(bool unlocked, bool unlockAnimPlayed)
		{
			mRender.transform.position = new Vector3
				(transform.position.x, lockedYPos, transform.position.z);

			raising = true;
			StartCoroutine(RaiseCliff(mRender));
		}

		private IEnumerator RaiseCliff(MeshRenderer mRender)
		{
			mRender.enabled = true;
			GetComponent<LevelPinRaiseJuicer>().PlayRaiseJuice();

			while(raising)
			{
				mRender.transform.position += new Vector3 (0, raiseStep, 0);

				yield return new WaitForSeconds(raiseSpeed);

				if (mRender.transform.position.y >= unlockedYPos)
				{
					raising = false;
					mRender.transform.position = new Vector3(
						transform.position.x, unlockedYPos, transform.position.z);

					GetComponent<LevelPinRaiseJuicer>().StopRaiseJuice();
					onShowOrHideUI(this, true);
					onPathDrawing(pathPoint, lRenders);
				}
			}
		}
	}
}
