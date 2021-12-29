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
        MMFeedbackPosition mmPos;

        //States
        public int locIndex { get; set; } = -1;
        float posOriginalCurveZero;
		bool newInput = false;

        private void Awake()
        {
            serpScroller = FindObjectOfType<SerpentScreenScroller>();
            mmPos = scrollJuice.GetComponent<MMFeedbackPosition>();
        }

        private void OnEnable()
        {
            if (serpScroller != null) serpScroller.onScrollSegments += ScrollSegment;
        }

		private void Start()
		{
            posOriginalCurveZero = mmPos.RemapCurveZero;
		}

		public void SetSegmentsAtStart(int loc)
		{
			locIndex = loc;
			transform.position = serpScroller.scrollLocs[loc].transform.position;
			ScaleIfInFocus();
			var rot = GetRotation(loc);
			transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
		}

		private void ScrollSegment(int value)
		{
			//checks if segments is actually active or not
			if (locIndex < 0) return;
			newInput = true;
			locIndex += value;
			StartCoroutine(MoveSegmentToLoc(locIndex));
			ScaleIfInFocus();

			//TriggerScrollJuice(value);
		}

		private IEnumerator MoveSegmentToLoc(int loc)
		{
			yield return null; //so the while registers newInput

			var target = serpScroller.scrollLocs[loc].transform.position;
			var step = serpScroller.scrollSpeed * Time.deltaTime;

			var rot = GetRotation(loc);
			var rotTarget = new Vector3(rot.x, rot.y, rot.z);
			var rotStep = serpScroller.rotateSpeed * Time.deltaTime;
			newInput = false;

			while (Vector3.Distance(transform.position, target) > .1f && !newInput)
			{
				transform.position = Vector3.MoveTowards(transform.position, target, step);
				transform.rotation = Quaternion.RotateTowards(transform.rotation,
					Quaternion.Euler(rot.x, rot.y, rot.z), rotStep);
				print(this.gameObject.name + " is moving");
				yield return null;
			}

			transform.position = target;
		}

        private void ScaleIfInFocus()
        {
            var n = serpScroller.focusEnlargement;
            if (locIndex == serpScroller.focusIndex) transform.localScale = new Vector3(n, n, n);
            else transform.localScale = new Vector3(1, 1, 1);
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

        private void TriggerScrollJuice(int value)
		{
            mmPos.RemapCurveZero = value * posOriginalCurveZero;
            scrollJuice.Initialization();
            scrollJuice.PlayFeedbacks();
		}

        private void OnDisable()
        {
            if (serpScroller != null) serpScroller.onScrollSegments -= ScrollSegment;
        }

    }
}
