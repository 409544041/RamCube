using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Serpent
{
    public class SegmentScroll : MonoBehaviour
    {
		//Config parameters
		public MMFeedbacks scrollJuice, popInJuice;
		[SerializeField] SegmentRefHolder refs;

        //Cache
        SerpentScreenScroller serpScroller;
		SerpCoreRefHolder scRef;
		SerpLogicRefHolder slRef;

        //States
        public int locIndex { get; set; } = -1;
		bool newInput = false;
		float elapsedTime;

		public void SetSegmentsAtStart(int loc, SerpentScreenScroller scroller)
		{
			if (serpScroller == null)
			{
				serpScroller = scroller;
				scRef = serpScroller.scRef;
				slRef = scRef.slRef;
			}

			locIndex = loc;
			InitiateSegmentScroll(0, true);
		}

		public void InitiateSegmentScroll(int value, bool setAtStart)
		{
			//checks if segments is actually active or not
			if (locIndex < 0) return;
			newInput = true;
			locIndex += value;
			StartCoroutine(ScrollSegment(locIndex, setAtStart));
		}

		private IEnumerator ScrollSegment(int loc, bool setAtStart)
		{
			yield return null; //so the while registers newInput

			var startPos = transform.position;
			var target = serpScroller.scrollLocs[loc].transform.position;

			var startRot = transform.rotation;
			var rot = GetRotation(loc);
			var rotTarget = Quaternion.Euler(rot.x, rot.y, rot.z);

			var startScale = refs.meshParent.transform.localScale;
			var targetScale = GetScale(loc);

			var startMarkerPos = new Vector3(0, 0, 0);
			var targetMarkerPos = new Vector3(0, 0, 0);

			if (refs.markerTrans != null)
			{
				startMarkerPos = refs.markerTrans.localPosition;
				targetMarkerPos = GetMarker(loc, startMarkerPos);
			}

			newInput = false;

			elapsedTime = 0;

			//new input breaks the while and skips straight to setting segment to final values
			while (Vector3.Distance(transform.position, target) > .1f && !newInput && !setAtStart)
			{
				elapsedTime += Time.deltaTime;
				var percentageComplete = elapsedTime / serpScroller.scrollDur;

				transform.position = Vector3.Lerp(startPos, target, 
					serpScroller.moveCurve.Evaluate(percentageComplete));
				transform.rotation = Quaternion.Lerp(startRot, rotTarget, 
					serpScroller.moveCurve.Evaluate(percentageComplete));
				refs.meshParent.transform.localScale = Vector3.Lerp(startScale, targetScale, 
					serpScroller.scaleCurve.Evaluate(percentageComplete));
				if (refs.markerTrans != null)
					refs.markerTrans.localPosition = Vector3.Lerp(startMarkerPos, targetMarkerPos,
						serpScroller.moveCurve.Evaluate(percentageComplete));

				yield return null;
			}

			transform.position = target;
			transform.rotation = rotTarget;
			refs.meshParent.transform.localScale = targetScale;
			if (refs.markerTrans != null) refs.markerTrans.localPosition = targetMarkerPos;

			if (setAtStart && refs.uiHandler != null)
				refs.uiHandler.SetScreenEdgePosWithPadding();

			if (loc == serpScroller.focusIndex)
			{
				HandleSegmentInFocus(setAtStart);
			}
		}

		private void HandleSegmentInFocus(bool setAtStart)
		{
			if (!setAtStart) TriggerScaleJuice();

			var e_objs = refs.mSegments.f_Objects;
			slRef.serpScreenUIHandler.ShowObjectUI(e_objs);
			slRef.objSegChecker.DecideOnDialogueToPlay(refs);
			scRef.namePlateText.text = refs.mSegments.f_SegmentName;
			if (refs.exprHandler != null)
				refs.exprHandler.SetFace(Expressions.gleeful, .5f);
		}

		public void PopInJuice()
		{
			var mmScale = popInJuice.GetComponent<MMFeedbackScale>();
			if (refs.dragonAnim != null)
				mmScale.RemapCurveOne = refs.scRef.slRef.scroller.headFocusEnlargement;
			else mmScale.RemapCurveOne = refs.scRef.slRef.scroller.focusEnlargement;

			popInJuice.Initialization();
			popInJuice.PlayFeedbacks();
		}

		private Vector3 GetMarker(int loc, Vector3 currentLocalPos)
		{
			Vector3 newLocalPos;

			if (loc == serpScroller.focusIndex)
				newLocalPos = new Vector3(currentLocalPos.x, refs.uiHandler.loweredMarkerY, currentLocalPos.z);
			else newLocalPos = new Vector3(currentLocalPos.x, refs.uiHandler.markerY, currentLocalPos.z);
			
			return newLocalPos;
		}

		private Vector3 GetScale(int loc)
        {
			Vector3 scale;
			float n = 0;

			if (refs.mSegments.Entity == E_Segments.GetEntity(0))
				n = serpScroller.headFocusEnlargement;
			else n = serpScroller.focusEnlargement;

            if (loc == serpScroller.focusIndex) scale = new Vector3(n, n, n);
            else scale = new Vector3(1, 1, 1);

			return scale;
        }

        private Vector3 GetRotation(int loc)
		{
			Vector3 rot;
			if (loc == serpScroller.focusIndex) rot = new Vector3(0, 180, 0);
			else if (loc == serpScroller.focusIndex + 1 ||
				loc == serpScroller.focusIndex - 1) rot = new Vector3(0, 150, 0);
			else if (loc == serpScroller.focusIndex + 2 ||
				loc == serpScroller.focusIndex - 2) rot = new Vector3(0, 120, 0);
			else rot = new Vector3(0, 90, 0);

			return rot;
		}

        private void TriggerScaleJuice()
		{
            scrollJuice.Initialization();
            scrollJuice.PlayFeedbacks();
		}
    }
}
