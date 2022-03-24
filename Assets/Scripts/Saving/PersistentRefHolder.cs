using Qbism.General;
using Qbism.SceneTransition;
using Qbism.Serpent;
using Qbism.WorldMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Saving
{
	public class PersistentRefHolder : MonoBehaviour
	{
		//Config parameters
		public FeatureSwitchBoard switchBoard;
		[Header("Progress")]
		public ProgressHandler progHandler;
		public SerpentProgress serpProg;
		public ObjectsProgress objProg;
		[Header("Music")]
		public MusicOrderHandler musicOrder;
		[Header("Material Variety")]
		public VarietyMaterialHandler varMatHandler;
		[Header("Scene Transition")]
		public Fader fader;
		public Canvas fadeCanvas;
		public CanvasGroup fadeCanvasGroup;
		public CircleTransition circTransition;
		public Canvas circCanvas;

		//States
		public Camera cam { get; set; }
		public MapCoreRefHolder mcRef { get; set; }
		public MapLogicRefHolder mlRef { get; set; }
		public GameplayCoreRefHolder gcRef { get; set; }
		public GameLogicRefHolder glRef { get; set; }
		public SerpCoreRefHolder scRef { get; set; }
	}
}