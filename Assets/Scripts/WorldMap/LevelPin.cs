using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.SceneTransition;
using UnityEngine;
using BansheeGz.BGDatabase;

namespace Qbism.WorldMap
{
	public class LevelPin : MonoBehaviour
	{
		//Config parameters
		public LevelPinUI pinUI;
		[Header ("Unlocking")]
		[SerializeField] float lockedYPos;
		public float unlockedYPos;
		[SerializeField] float raiseStep;
		[SerializeField] float raiseSpeed;
		[Header ("Paths")]
		public Transform pathPoint;
		public LineRenderer[] fullLineRenderers;
		public LineRenderer dottedLineRenderer;
		[Header ("References")]
		public M_LevelData m_levelData;
		public M_Pin m_Pin;

		//Cache
		MeshRenderer mRender;

		//Actions, events, delegates etc
		public event Action<Transform, LineTypes, List<LevelPin>> onPathDrawing;
		public event Action<Transform, List<LineDrawData>, LineRenderer[], LineRenderer> onPathCreation;
		public Func<E_Pin, LevelPin> onGetPin;

		//States
		public bool justCompleted { get; set; } = false;
		bool raising = false;
		int amountInDrawList = 0;
		List<LineDrawData> lineDestList = new List<LineDrawData>();

		private void Awake() 
		{
			mRender = GetComponentInChildren<MeshRenderer>();
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

		public void CheckPathStatus(E_Pin uPin, bool uUnlocked, bool uUnlockAnimPlayed, int uLocksLeft, bool completed,
			int listCount)
		{
			AddToList(uPin, completed, uUnlocked, uUnlockAnimPlayed, uLocksLeft, lineDestList);
			amountInDrawList++;

			if (amountInDrawList == listCount)
				onPathCreation(pathPoint, lineDestList, fullLineRenderers, dottedLineRenderer);
		}

		private void AddToList(E_Pin uPin, bool completed, bool unlockStatus, bool unlockAnim, 
			int locks, List<LineDrawData> lineDestList)
		{
			LevelPin destPin = onGetPin(uPin);

			int locksAmount = destPin.m_levelData.f_LocksAmount;
			bool lessLocks = locksAmount > locks && locks > 0;
			bool noLocks = locksAmount > locks && locks == 0;

			if((completed && lessLocks) || (!justCompleted && completed && noLocks && !unlockAnim))
			{
				LineDrawData drawData = new LineDrawData();
				drawData.destination = destPin.pathPoint;
				drawData.lineType = LineTypes.dotted;
				lineDestList.Add(drawData);
			}

			if (completed && unlockStatus && unlockAnim)
			{
				LineDrawData drawData = new LineDrawData();
				drawData.destination = destPin.pathPoint;
				drawData.lineType = LineTypes.full;
				lineDestList.Add(drawData);
			}
		}

		public void InitiateRaising(bool unlocked, bool unlockAnimPlayed, List<LevelPin> originPins)
		{
			mRender.transform.position = new Vector3
				(transform.position.x, lockedYPos, transform.position.z);

			raising = true;
			StartCoroutine(RaiseCliff(mRender, originPins));
		}

		private IEnumerator RaiseCliff(MeshRenderer mRender, List<LevelPin> originPins)
		{
			mRender.enabled = true;
			var raiser = GetComponent<LevelPinRaiseJuicer>();
			raiser.PlayRaiseJuice();

			while(raising)
			{
				mRender.transform.position += new Vector3 (0, raiseStep, 0);

				yield return new WaitForSeconds(raiseSpeed);

				if (mRender.transform.position.y >= unlockedYPos) raising = false;

				mRender.transform.position = new Vector3
					(transform.position.x, unlockedYPos, transform.position.z);

				raiser.StopRaiseJuice();
				pinUI.ShowOrHideUI(true);
				onPathDrawing(pathPoint, LineTypes.full, originPins);
			}
		}

		public void DrawDottedPath(List<LevelPin> originPins)
		{
			onPathDrawing(pathPoint, LineTypes.dotted, originPins);
		}
	}
}
