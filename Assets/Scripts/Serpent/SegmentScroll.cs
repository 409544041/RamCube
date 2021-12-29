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

        private void ScrollSegment(int value)
        {
            //checks if segments is actually active or not
            if (locIndex < 0) return;

            locIndex += value;
            transform.position = serpScroller.scrollLocs[locIndex].transform.position;
        }

        private void OnDisable()
        {
            serpScroller.onScrollSegments -= ScrollSegment;
        }

    }
}
