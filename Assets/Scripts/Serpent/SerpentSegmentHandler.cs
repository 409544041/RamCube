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
		public List<bool> serpDataList { get; set; } = new List<bool>();
		int BillyArrayIndex;

		//Actions, events, delegates etc
		public Func<List<bool>> onFetchSerpDataList;
		public Transform[] segmentsReordered;

		private void Awake() 
		{
			finishEndSeq = FindObjectOfType<FinishEndSeqHandler>();
			splineHandler = FindObjectOfType<SerpentScreenSplineHandler>(); 
		}

		private void OnEnable() 
		{
			if (finishEndSeq != null) finishEndSeq.onShowSegments += EnableSegmentsForEndSeq;
		}

		private void Start()
		{
			serpDataList = onFetchSerpDataList();
			DisableSegmentsAtStart();
		}

        private void DisableSegmentsAtStart()
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

        public void EnableSegmentsForEndSeq()
		{
			var segmentsWithoutBilly = new Transform[segments.Length - 1];
            for (int i = 0; i < segmentsWithoutBilly.Length; i++)
            {
				segmentsWithoutBilly[i] = segments[i];
            }
			
			EnableSegments(segmentsWithoutBilly);
		}

		public Transform[] PrepareSegmentsInMap()
        {
            segmentsReordered = new Transform[segments.Length];
            BillyArrayIndex = segments.Length - 1;

            BillyArrayIndex = GetBillyArrayIndex(BillyArrayIndex);

            PlaceBillyInNewArray(segmentsReordered, BillyArrayIndex);

			return segmentsReordered;
		}

		private int GetBillyArrayIndex(int BillyArrayIndex)
		{
			for (int i = 0; i < serpDataList.Count; i++)
			{
				if (serpDataList[i] == false)
				{
					BillyArrayIndex = i;
					break;
				}
			}

			return BillyArrayIndex;
		}

		private void PlaceBillyInNewArray(Transform[] segmentsReordered, int BillyArrayIndex)
        {
            for (int i = 0; i < segments.Length; i++)
            {
                if (i < BillyArrayIndex) segmentsReordered[i] = segments[i];

                else if (i == BillyArrayIndex)
                    segmentsReordered[i] = segments[segments.Length - 1];

                else if (i > BillyArrayIndex && i < segments.Length)
                    segmentsReordered[i] = segments[i - 1];
            }
        }

		public void EnableSegments(Transform[] segmentArray)
		{
			bool inMap = GetComponent<SerpentMapHandler>();

			for (int i = 0; i < segmentArray.Length; i++)
			{
				var mRenders = segmentArray[i].GetComponentsInChildren<Renderer>();
				SpriteRenderer[] sRenders = segmentArray[i].GetComponentsInChildren<SpriteRenderer>();
				var follower = segmentArray[i].GetComponent<SplineFollower>();

				if ((mRenders.Length == 0 || sRenders.Length == 0) && i != 0) Debug.Log
						(segmentArray[i] + " is missing either a meshrenderer or spriterenderer!");

				//Are we in map, don't enable last segment unless last segment is billy
				if (inMap && i == segmentArray.Length - 1 && BillyArrayIndex != segmentArray.Length - 1) 
					SwitchRenderers(mRenders, sRenders, follower, false);

				else if (serpDataList[i] == true) 
					SwitchRenderers(mRenders, sRenders, follower, true);

				else if (inMap && i == BillyArrayIndex) 
					SwitchRenderers(mRenders, sRenders, follower, true);

				else SwitchRenderers(mRenders, sRenders, follower, false);
			}
		}

        private void SwitchRenderers(Renderer[] mRenders, SpriteRenderer[] sRenders, 
			SplineFollower follower, bool value)
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
			if (finishEndSeq != null) finishEndSeq.onShowSegments -= EnableSegmentsForEndSeq;
		}
	}
}

