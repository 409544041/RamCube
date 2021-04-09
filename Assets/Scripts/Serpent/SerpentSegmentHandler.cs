using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using Qbism.Cubes;
using Qbism.Saving;
using UnityEngine;

namespace Qbism.Serpent
{
	public class SerpentSegmentHandler : MonoBehaviour
	{
		//Config parameters
		public Transform[] segments = null;

		//Cache
		SerpentProgress serpProg;

		private void Awake() 
		{
			serpProg = FindObjectOfType<SerpentProgress>();	
		}

		private void Start()
		{
			for (int i = 0; i < segments.Length; i++)
			{
				MeshRenderer mRender = segments[i].GetComponentInChildren<MeshRenderer>();
				SpriteRenderer sRender = segments[i].GetComponentInChildren<SpriteRenderer>();
				SplineFollower follower = segments[i].GetComponent<SplineFollower>();

				if (!mRender || !sRender) Debug.LogError
					 (segments[i] + " is missing either a meshrenderer or spriterenderer!");

				if (serpProg.serpentDataList[i] == true)
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

