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

            var n = serpScroller.focusEnlargement;
            if (locIndex == serpScroller.focusIndex) transform.localScale = new Vector3(n, n, n);
            else transform.localScale = new Vector3(1, 1, 1);
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
