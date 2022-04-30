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

			var startScale = transform.localScale;
			var targetScale = GetScale(loc);

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
				transform.localScale = Vector3.Lerp(startScale, targetScale, 
					serpScroller.scaleCurve.Evaluate(percentageComplete));

				yield return null;
			}

			transform.position = target;
			transform.rotation = rotTarget;
			transform.localScale = targetScale;

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
			refs.exprHandler.SetFace(Expressions.gleeful, .5f);
		}

		public void PopInJuice()
		{
			popInJuice.Initialization();
			popInJuice.PlayFeedbacks();
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
