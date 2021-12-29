using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Serpent
{
    public class SerpentScreenScroller : MonoBehaviour
    {
        //Config paramters
        public Transform[] scrollLocs;
        [SerializeField] SerpentSegmentHandler segHandler;
        public int focusIndex;
        public float focusEnlargement = 2f;

        //States
        SegmentScroll segInFocusAtStart = null;
        int segToFocusOn = 0;
        SegmentScroll[] segments = null;

        //Actions, events, delegates etc
        public event Action<int> onScrollSegments;

        private IEnumerator Start()
        {
            yield return null; //to avoid race condition with onFetchSerpDataList in segHandler
            PlaceSegmentsAtStart();
        }

        private void PlaceSegmentsAtStart()
        {
            Transform[] segmentsTrans = GetRescuedSegments();

            SetSegmentInFocus();

            SetRestOfSegments();

            segHandler.EnableSegments(segmentsTrans);
        }

        private Transform[] GetRescuedSegments()
        {
            var segmentsTrans = segHandler.PrepareSegmentsUpToBilly();

            segments = new SegmentScroll[segmentsTrans.Length];
            for (int i = 0; i < segmentsTrans.Length; i++)
            {
                segments[i] = segmentsTrans[i].GetComponent<SegmentScroll>();
            }

            return segmentsTrans;
        }

        private void SetSegmentInFocus()
        {
            segToFocusOn = segments.Length - 1;
            segInFocusAtStart = segments[segToFocusOn];
            segInFocusAtStart.SetSegmentToLoc(focusIndex);
        }

        private void SetRestOfSegments()
        {
            int locIndex = focusIndex + 1;
            for (int i = segToFocusOn - 1; i >= 0; i--)
            {
                segments[i].SetSegmentToLoc(locIndex);
                locIndex++;
                if (locIndex > scrollLocs.Length - 1) locIndex = scrollLocs.Length - 1;

            }

            //Place segments higher in segment array to the left of focus loc
            locIndex = focusIndex - 1;
            for (int i = segToFocusOn + 1; i < segments.Length; i++)
            {
                segments[i].SetSegmentToLoc(locIndex);
                locIndex--;
                if (locIndex < 0) locIndex = 0;
            }
        }

        public void ScrollLeft()
        {
            if (segments[0].locIndex == focusIndex) return;
            onScrollSegments(-1);
        }

        public void ScrollRight()
        {
            if (segments[segments.Length - 1].locIndex == focusIndex) return;
            onScrollSegments(1);
        }
    }
}
