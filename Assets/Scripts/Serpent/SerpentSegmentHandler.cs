using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;
using Qbism.Cubes;
using System;
using Qbism.WorldMap;

namespace Qbism.Serpent
{
	public class SerpentSegmentHandler : MonoBehaviour
	{
		//Config parameters
		public Transform[] segments = null;
		[SerializeField] MapCoreRefHolder mcRef;
		[SerializeField] FinishRefHolder finishRef;
		[SerializeField] SerpCoreRefHolder scRef;

		//States
		int billyArrayIndex;
		Transform[] segmentsReordered = null;
		Transform[] segmentsUpToBilly = null;
		bool inMap = false, inSerpScreen = false, inLevel = false;

		private void Start()
		{
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
			for (int i = 0; i < E_SegmentsGameplayData.CountEntities; i++)
			{
				if (E_SegmentsGameplayData.GetEntity(i).f_Rescued == false)
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
			inMap = (mcRef != null);
			inSerpScreen = (scRef != null);
			inLevel = (finishRef != null);

			for (int i = 0; i < segmentArray.Length; i++)
			{
				var mRenders = segmentArray[i].GetComponentsInChildren<Renderer>();
				SpriteRenderer[] sRenders = segmentArray[i].GetComponentsInChildren<SpriteRenderer>();

				if ((mRenders.Length == 0 || sRenders.Length == 0) && i != 0) Debug.Log
						(segmentArray[i] + " is missing either a meshrenderer or spriterenderer!");

				//Are we in map, don't enable last segment (which is Billy) unless Billy is
				//actually in the very last position
				if ((inMap || inSerpScreen) && i == segmentArray.Length - 1 && 
					billyArrayIndex != segmentArray.Length - 1) 
					SwitchRenderers(mRenders, sRenders, false);

				//Checking if we're in a level where we're rescuing a segment and
				//making sure that segment isn't visible on pickup
				else if (inLevel && finishRef.endSeq.FetchHasSegment() &&
					E_SegmentsGameplayData.GetEntity(i).f_Rescued == true &&
					E_SegmentsGameplayData.GetEntity(i + 1).f_Rescued == false)
					SwitchRenderers(mRenders, sRenders, false);

				else if (E_SegmentsGameplayData.GetEntity(i).f_Rescued == true) 
					SwitchRenderers(mRenders, sRenders, true);

				else if ((inMap || inSerpScreen) && i == billyArrayIndex) 
					SwitchRenderers(mRenders, sRenders, true);

				else SwitchRenderers(mRenders, sRenders, false);
			}
		}

        private void SwitchRenderers(Renderer[] mRenders, SpriteRenderer[] sRenders, bool value)
		{
			for (int i = 0; i < mRenders.Length; i++)
			{
				mRenders[i].enabled = value;
			}

			if (inMap) mcRef.splineFollower.useTriggers = value;
			else if (inLevel) finishRef.follower.useTriggers = value;

			if (sRenders.Length == 0) return;

			for (int i = 0; i < sRenders.Length; i++)
			{
				sRenders[i].enabled = value;
			}
		}
	}
}

