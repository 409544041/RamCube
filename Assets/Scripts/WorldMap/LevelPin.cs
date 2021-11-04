using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.SceneTransition;
using UnityEngine;
using BansheeGz.BGDatabase;

namespace Qbism.WorldMap
{
	public class LevelPin : MonoBehaviour
	{
		//Config parameters
		public LevelPinUI pinUI;
		public LevelPinPathHandler pinPather;
		public LevelPinRaiser pinRaiser;
		public M_LevelData m_levelData;
		public M_Pin m_Pin;

		//Cache
		public MeshRenderer mRender { get; set; }

		//States
		public bool justCompleted { get; set; } = false;

		private void Awake()
		{
			mRender = GetComponentInChildren<MeshRenderer>();
		}

		public void CheckRaiseStatus(bool unlocked, bool unlockAnimPlayed)
		{
			if (!unlocked)
			{
				mRender.enabled = false;
				mRender.transform.position = 
					new Vector3 (transform.position.x, pinRaiser.lockedYPos, transform.position.z);
			} 

			else if (unlocked && unlockAnimPlayed)
			{
				mRender.enabled = true;
				mRender.transform.position = 
				new Vector3 (transform.position.x, pinRaiser.unlockedYPos, transform.position.z);
			}
		}
	}
}
