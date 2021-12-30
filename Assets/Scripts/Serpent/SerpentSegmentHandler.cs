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

		//States
		public List<bool> serpDataList { get; set; } = new List<bool>();
		int billyArrayIndex;
		Transform[] segmentsReordered = null;
		Transform[] segmentsUpToBilly = null;

		//Actions, events, delegates etc
		public Func<List<bool>> onFetchSerpDataList;


		private void Awake() 
		{
			finishEndSeq = FindObjectOfType<FinishEndSeqHandler>();
		}

		private void OnEnable() 
		{
			if (finishEndSeq != null) finishEndSeq.onShowSegments += EnableSegmentsWithoutBilly;
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

        public void EnableSegmentsWithoutBilly()
		{
			var segmentsWithoutBilly = new Transform[segments.Length - 1];

            for (int i = 0; i < segmentsWithoutBilly.Length; i++)
            {
				segmentsWithoutBilly[i] = segments[i];
            }

			EnableSegments(segmentsWithoutBilly);
		}

		public Transform[] PrepareSegmentsWithBilly()
        {
            segmentsReordered = new Transform[segments.Length];
            var billyIndex = segments.Length - 1;

			billyIndex = GetBillyArrayIndex(billyIndex);
			billyArrayIndex = billyIndex;

            PlaceBillyInNewArray(segmentsReordered, billyIndex);

			return segmentsReordered;
		}

		public Transform[] PrepareSegmentsUpToBilly()
        {
			var billyIndex = segments.Length - 1;
			billyIndex = GetBillyArrayIndex(billyIndex);
			billyArrayIndex = billyIndex;

			segmentsUpToBilly = new Transform[billyIndex + 1];
			FillArrayUpToBilly(segmentsUpToBilly, billyIndex);

			return segmentsUpToBilly;
		}

		private int GetBillyArrayIndex(int billyIndex)
		{
			for (int i = 0; i < serpDataList.Count; i++)
			{
				if (serpDataList[i] == false)
				{
					billyIndex = i;
					break;
				}
			}
			return billyIndex;
		}

		private void PlaceBillyInNewArray(Transform[] segmentsReordered, int billyIndex)
        {
            for (int i = 0; i < segments.Length; i++)
            {
                if (i < billyIndex) segmentsReordered[i] = segments[i];

                else if (i == billyIndex)
                    segmentsReordered[i] = segments[segments.Length - 1];

                else if (i > billyIndex && i < segments.Length)
                    segmentsReordered[i] = segments[i - 1];
            }
        }

		private void FillArrayUpToBilly(Transform[] array, int billyIndex)
		{
			for (int i = 0; i < segments.Length; i++)
			{
				if (i < billyIndex) array[i] = segments[i];

				else if (i == billyIndex) array[i] = segments[segments.Length - 1];
			}
		}

		public void EnableSegments(Transform[] segmentArray)
		{
			bool inMap = GetComponent<SerpentMapHandler>();
			bool inSerpScreen = FindObjectOfType<SerpentScreenScroller>();

			for (int i = 0; i < segmentArray.Length; i++)
			{
				var mRenders = segmentArray[i].GetComponentsInChildren<Renderer>();
				SpriteRenderer[] sRenders = segmentArray[i].GetComponentsInChildren<SpriteRenderer>();
				var follower = segmentArray[i].GetComponent<SplineFollower>();

				if ((mRenders.Length == 0 || sRenders.Length == 0) && i != 0) Debug.Log
						(segmentArray[i] + " is missing either a meshrenderer or spriterenderer!");

				//Are we in map, don't enable last segment (which is Billy) unless Billy is
				//actually in the very last position
				if ((inMap || inSerpScreen) && i == segmentArray.Length - 1 && 
					billyArrayIndex != segmentArray.Length - 1) 
					SwitchRenderers(mRenders, sRenders, follower, false);

				else if (serpDataList[i] == true) 
					SwitchRenderers(mRenders, sRenders, follower, true);

				else if ((inMap || inSerpScreen) && i == billyArrayIndex) 
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
			if (finishEndSeq != null) finishEndSeq.onShowSegments -= EnableSegmentsWithoutBilly;
		}
	}
}

