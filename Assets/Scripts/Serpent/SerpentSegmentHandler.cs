using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;
using Qbism.Cubes;
using System;

namespace Qbism.Serpent
{
	public class SerpentSegmentHandler : MonoBehaviour
	{
		//Config parameters
		public Transform[] segments = null;

		//Cache
		FinishEndSeqHandler finishEndSeq = null;
		SerpentScreenSplineHandler splineHandler = null;

		//States
		List<bool> serpDataList = new List<bool>();

		//Actions, events, delegates etc
		public Func<List<bool>> onFetchSerpDataList;

		private void Awake() 
		{
			finishEndSeq = FindObjectOfType<FinishEndSeqHandler>();
			splineHandler = FindObjectOfType<SerpentScreenSplineHandler>();
		}

		private void OnEnable() 
		{
			if (finishEndSeq != null) finishEndSeq.onShowSegments += EnableSegments;
		}

		private void Start()
		{
			if (!splineHandler) //Checking if we're in serpent screen or not
			{
				for (int i = 0; i < segments.Length; i++)
				{
					var mRenders = segments[i].GetComponentsInChildren<Renderer>();
					var sRender = segments[i].GetComponentInChildren<SpriteRenderer>();

					for (int j = 0; j < mRenders.Length; j++)
					{
						mRenders[j].enabled = false;
					}

					if (sRender) sRender.enabled = false;
				}
			}
			else EnableSegments();
			
		}

		public void EnableSegments()
		{
			serpDataList = onFetchSerpDataList();

			for (int i = 0; i < segments.Length; i++)
			{
				var mRenders = segments[i].GetComponentsInChildren<Renderer>();
				SpriteRenderer[] sRenders = segments[i].GetComponentsInChildren<SpriteRenderer>();
				var follower = segments[i].GetComponent<SplineFollower>();

				if ((mRenders.Length == 0 || sRenders.Length == 0) && i != 0) Debug.Log
						(segments[i] + " is missing either a meshrenderer or spriterenderer!");

				if (serpDataList[i] == true) SwitchRenderers(mRenders, sRenders, follower, true);
				else SwitchRenderers(mRenders, sRenders, follower, false);
			}
		}

		private void SwitchRenderers(Renderer[] mRenders, SpriteRenderer[] sRenders, SplineFollower follower, bool value)
		{
			for (int i = 0; i < mRenders.Length; i++)
			{
				mRenders[i].enabled = value;
			}

			if (follower) follower.useTriggers = value;

			if (sRenders.Length == 0) return;

			for (int i = 0; i < sRenders.Length; i++)
			{
				sRenders[i].enabled = value;
			}
		}

		private void OnDisable()
		{
			if (finishEndSeq != null) finishEndSeq.onShowSegments -= EnableSegments;
		}
	}
}

