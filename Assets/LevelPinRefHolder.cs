using MoreMountains.Feedbacks;
using Qbism.Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Qbism.WorldMap
{
	public class LevelPinRefHolder : MonoBehaviour
	{
		//Config parameters
		[Header("Logic")]
		public LevelPinPathHandler pinPather;
		public LevelPinRaiser pinRaiser;
		public LevelPinWallLowerer pinWallLowerer;
		public Transform pathPoint;
		public Transform wallPathPoint;
		[Header("Lines")]
		public LineDrawer[] fullLineDrawers;
		public LineRenderer[] fullLineRenderers;
		public LineDrawer dottedLineDrawer;
		public LineRenderer dottedLineRenderer;
		[Header("UI")]
		public LevelPinUI pinUI;
		public Canvas pinUICanvas;
		[Header("Juice")]
		public LevelPinRaiseJuicer pinRaiseJuicer;
		public LevelPinUIJuicer pinUIJuicer;

		//Cache
		public M_LevelData m_levelData { get; private set; }
		public M_Pin m_pin { get; private set; }

		private void Awake()
		{
			m_levelData = GetComponent<M_LevelData>();
			m_pin = GetComponent<M_Pin>();
		}
	}
}
