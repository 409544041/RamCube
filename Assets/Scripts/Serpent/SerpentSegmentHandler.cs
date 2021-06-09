using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;
using Qbism.Cubes;

namespace Qbism.Serpent
{
	public class SerpentSegmentHandler : MonoBehaviour
	{
		//Config parameters
		public Transform[] segments = null;

		//Cache
		FinishCube finish = null;
		SerpentScreenSplineHandler splineHandler = null;

		//States
		List<bool> serpDataList = new List<bool>();

		//Actions, events, delegates etc
		public delegate List<bool> GetSerpDataDel();
		public GetSerpDataDel onFetchSerpDataList;

		private void Awake() 
		{
			finish = FindObjectOfType<FinishCube>();
			splineHandler = FindObjectOfType<SerpentScreenSplineHandler>();
		}

		private void OnEnable() 
		{
			if (finish != null) finish.onShowSegments += EnableSegments;
		}

		private void Start()
		{
			if (splineHandler == null) //Checking if we're in serpent screen or not
			{
				for (int i = 0; i < segments.Length; i++)
				{
					var mRender = segments[i].GetComponentInChildren<SkinnedMeshRenderer>();
					var sRender = segments[i].GetComponentInChildren<SpriteRenderer>();

					mRender.enabled = false;
					sRender.enabled = false;
				}
			}
			else EnableSegments();
			
		}

		public void EnableSegments()
		{
			serpDataList = onFetchSerpDataList();

			for (int i = 0; i < segments.Length; i++)
			{
				var mRender = segments[i].GetComponentInChildren<SkinnedMeshRenderer>();
				var sRender = segments[i].GetComponentInChildren<SpriteRenderer>();
				var follower = segments[i].GetComponent<SplineFollower>();

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

		private void OnDisable()
		{
			if (finish != null) finish.onShowSegments -= EnableSegments;
		}
	}
}

