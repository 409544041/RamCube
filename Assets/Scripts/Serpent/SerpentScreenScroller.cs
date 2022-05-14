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
		public float focusEnlargement = 2f, headFocusEnlargement = 1.5f;
		public float scrollDur = .2f;
		public float scaleJuiceBounce = .3f;
		public AnimationCurve moveCurve, scaleCurve;
		public SerpCoreRefHolder scRef;

		//States
		SegmentRefHolder segInFocusAtStart;
        int segToFocusOn = 0;
		SegmentRefHolder[] segments;
		List<SegmentRefHolder> newSegments = new List<SegmentRefHolder>();

        private IEnumerator Start()
        {
            yield return null; //to avoid race condition with onFetchSerpDataList in segHandler
            PlaceSegmentsAtStart();
        }

        private void PlaceSegmentsAtStart()
		{
			segments = segHandler.PrepareSegmentsUpToBilly();
			SetSegmentInFocus();
			SetRestOfSegments();
			segHandler.EnableSegments(segments);
			CheckForNewSegmentsToAdd();
		}

		private void CheckForNewSegmentsToAdd()
		{
			for (int i = 0; i < segHandler.segRefs.Length; i++)
			{
				var seg = segHandler.segRefs[i];

				if (seg.mSegments.f_GameplayData == null) continue;

				if (seg.mSegments.f_GameplayData.f_Rescued == true &&
					seg.mSegments.f_GameplayData.f_AddedToSerpScreen == false)
					newSegments.Add(seg);
			}

			if (newSegments.Count > 0) StartCoroutine(PopInNewSegments());
			else ActivateSegmentsUI();
		}

		private IEnumerator PopInNewSegments()
		{
			scRef.slRef.screenStateMngr.serpentScreenState.allowInput = false;

			yield return new WaitForSeconds(.5f);

			foreach (var seg in newSegments)
			{
				seg.mSegments.f_GameplayData.f_AddedToSerpScreen = true;
			}

			segments = segHandler.PrepareSegmentsUpToBilly();

			for (int i = newSegments.Count - 1; i >= 0; i--)
			{
				MakeRoomForNewSegment();
				yield return new WaitForSeconds(scrollDur);
				newSegments[i].segScroll.SetSegmentsAtStart(focusIndex, this);
				yield return null; //Here to make sure all elements of segment have been moved to new loc
				newSegments[i].segScroll.PopInJuice();
				SpriteRenderer[] sRenders = newSegments[i].GetComponentsInChildren<SpriteRenderer>();
				segHandler.SwitchRenderers(newSegments[i].meshes, sRenders, true);
				yield return new WaitForSeconds(.5f);
			}

			ActivateSegmentsUI();
			scRef.slRef.screenStateMngr.serpentScreenState.allowInput = true;
		}

		private void SetSegmentInFocus()
        {
            segToFocusOn = segments.Length - 1;
            segInFocusAtStart = segments[segToFocusOn];
            segInFocusAtStart.segScroll.SetSegmentsAtStart(focusIndex, this);
        }

        private void SetRestOfSegments()
        {
            int locIndex = focusIndex + 1;
            for (int i = segToFocusOn - 1; i >= 0; i--)
            {
                segments[i].segScroll.SetSegmentsAtStart(locIndex, this);
                locIndex++;
            }

            //Place segments higher in segment array to the left of focus loc
            locIndex = focusIndex - 1;
            for (int i = segToFocusOn + 1; i < segments.Length; i++)
            {
                segments[i].segScroll.SetSegmentsAtStart(locIndex, this);
                locIndex--;
            }
        }

		private void MakeRoomForNewSegment()
		{
			foreach (var segment in segments)
			{
				if (segment.segScroll.locIndex > focusIndex) continue;
				segment.segScroll.InitiateSegmentScroll(-1, false);
			}
		}

		private void ActivateSegmentsUI()
		{
			foreach (var seg in segHandler.segRefs)
			{
				if (seg.uiHandler == null) continue;
				seg.uiHandler.ToggleUIDependingOnObjectStatus();
			}
		}

        public void ScrollLeft()
        {
            if (segments[0].segScroll.locIndex == focusIndex) return;

			foreach(var segment in segments)
			{
				segment.segScroll.InitiateSegmentScroll(-1, false);
			}
        }

        public void ScrollRight()
        {
            if (segments[segments.Length - 1].segScroll.locIndex == focusIndex) return;

			foreach (var segment in segments)
			{
				segment.segScroll.InitiateSegmentScroll(1, false);
			}
		}
    }
}
