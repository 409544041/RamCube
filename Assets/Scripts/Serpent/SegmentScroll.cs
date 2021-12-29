using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Serpent
{
    public class SegmentScroll : MonoBehaviour
    {
        //Config parameters
        [SerializeField] MMFeedbacks scrollJuice;

        //Cache
        SerpentScreenScroller serpScroller;

        //States
        public int locIndex { get; set; } = -1;
		bool newInput = false;
		float elapsedTime;

        private void Awake()
        {
            serpScroller = FindObjectOfType<SerpentScreenScroller>();
        }

        private void OnEnable()
        {
            if (serpScroller != null) serpScroller.onScrollSegments += InitiateSegmentScroll;
        }

		public void SetSegmentsAtStart(int loc)
		{
			locIndex = loc;
			InitiateSegmentScroll(0, true);
		}

		private void InitiateSegmentScroll(int value, bool setAtStart)
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

				transform.position = Vector3.Lerp(startPos, target, percentageComplete);
				transform.rotation = Quaternion.Lerp(startRot, rotTarget, percentageComplete);
				transform.localScale = Vector3.Lerp(startScale, targetScale, percentageComplete);

				yield return null;
			}

			transform.position = target;
			transform.rotation = rotTarget;
			transform.localScale = targetScale;

			if (loc == serpScroller.focusIndex && !setAtStart) TriggerScaleJuice(targetScale.x);
		}

		private Vector3 GetScale(int loc)
        {
			Vector3 scale;
            var n = serpScroller.focusEnlargement;

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

        private void TriggerScaleJuice(float targetScale)
		{
            scrollJuice.Initialization();
            scrollJuice.PlayFeedbacks();
		}

        private void OnDisable()
        {
            if (serpScroller != null) serpScroller.onScrollSegments -= InitiateSegmentScroll;
        }

    }
}
