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
					var mRender = segments[i].GetComponentInChildren<SkinnedMeshRenderer>();
					var sRender = segments[i].GetComponentInChildren<SpriteRenderer>();

					mRender.enabled = false;
					sRender.enabled = false;
				}
			}
			else EnableSegments();
			
		}

		public void EnableSegments()
		{
			serpDataList = onFetchSerpDataList();

			for (int i = 0; i < segments.Length; i++)
			{
				var mRender = segments[i].GetComponentInChildren<SkinnedMeshRenderer>();
				SpriteRenderer[] sRenders = segments[i].GetComponentsInChildren<SpriteRenderer>();
				var follower = segments[i].GetComponent<SplineFollower>();

				if (!mRender || sRenders.Length == 0) Debug.LogError
						(segments[i] + " is missing either a meshrenderer or spriterenderer!");

				if (serpDataList[i] == true)
				{
					mRender.enabled = true;
					if (follower) follower.useTriggers = true;

					foreach (var sRender in sRenders)
					{
						sRender.enabled = true;
					}
				}
				else
				{
					mRender.enabled = false;
					if (follower) follower.useTriggers = false;

					foreach (var sRender in sRenders)
					{
						sRender.enabled = false;
					}
				}
			}
		}

		private void OnDisable()
		{
			if (finishEndSeq != null) finishEndSeq.onShowSegments -= EnableSegments;
		}
	}
}

