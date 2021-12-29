using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Serpent
{
    public class SegmentScroll : MonoBehaviour
    {
        //Cache
        SerpentScreenScroller serpScroller;

        //States
        public int locIndex { get; set; } = -1;

        private void Awake()
        {
            serpScroller = FindObjectOfType<SerpentScreenScroller>();
        }

        private void OnEnable()
        {
            serpScroller.onScrollSegments += ScrollSegment;
        }

        public void SetSegmentToLoc(int loc)
		{
			locIndex = loc;
			transform.position = serpScroller.scrollLocs[loc].transform.position;

			ScaleIfInFocus();

			SetRotationTowardsCam();
		}

        private void ScaleIfInFocus()
        {
            var n = serpScroller.focusEnlargement;
            if (locIndex == serpScroller.focusIndex) transform.localScale = new Vector3(n, n, n);
            else transform.localScale = new Vector3(1, 1, 1);
        }

        private void SetRotationTowardsCam()
		{
			if (locIndex == serpScroller.focusIndex) transform.rotation =
				Quaternion.Euler(0, 180, 0);
			else if (locIndex == serpScroller.focusIndex + 1 ||
				locIndex == serpScroller.focusIndex - 1) transform.rotation =
					Quaternion.Euler(0, 150, 0);
			else if (locIndex == serpScroller.focusIndex + 2 ||
				locIndex == serpScroller.focusIndex - 2) transform.rotation =
					Quaternion.Euler(0, 120, 0);
			else transform.rotation = Quaternion.Euler(0, 90, 0);
		}

        private void ScrollSegment(int value)
        {
            //checks if segments is actually active or not
            if (locIndex < 0) return;

            SetSegmentToLoc(locIndex += value);
        }

        private void OnDisable()
        {
            serpScroller.onScrollSegments -= ScrollSegment;
        }

    }
}
