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
		public LevelPinWallLowerer wallLowerer;
		public M_LevelData m_levelData;
		public M_Pin m_Pin;

		//States
		public bool justCompleted { get; set; } = false;		
	}
}
