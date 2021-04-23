using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

namespace Qbism.Serpent
{
	public class SerpentSegmentHandler : MonoBehaviour
	{
		//Config parameters
		public Transform[] segments = null;

		//States
		List<bool> serpDataList = new List<bool>();

		//Actions, events, delegates etc
		public delegate List<bool> GetSerpDataDel();
		public GetSerpDataDel onFetchSerpDataList;

		private void Start()
		{
			serpDataList = onFetchSerpDataList();

			for (int i = 0; i < segments.Length; i++)
			{
				MeshRenderer mRender = segments[i].GetComponentInChildren<MeshRenderer>();
				SpriteRenderer sRender = segments[i].GetComponentInChildren<SpriteRenderer>();
				SplineFollower follower = segments[i].GetComponent<SplineFollower>();

				if (!mRender || !sRender) Debug.LogError
					 (segments[i] + " is missing either a meshrenderer or spriterenderer!");

				if (serpDataList[i] == true)
				{
					mRender.enabled = true;
					sRender.enabled = true;
					if (follower) follower.useTriggers = true;
				}
				else
				{
					mRender.enabled = false;
					sRender.enabled = false;
					if (follower) follower.useTriggers = false;
				}
			}
		}
	}
}

