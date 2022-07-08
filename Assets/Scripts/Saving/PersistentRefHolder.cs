using Qbism.Demo;
using Qbism.General;
using Qbism.SceneTransition;
using Qbism.Serpent;
using Qbism.Settings;
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
		public Timer sessionTimer;
		public DebugHUDHandler debugHUD;
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
		[Header("Settings")]
		public SettingsSaveLoad settingsSaveLoad;
		public ToggleHud hudToggler;

		//States
		public Camera cam { get; set; }
		public MapCoreRefHolder mcRef { get; set; }
		public MapLogicRefHolder mlRef { get; set; }
		public GameplayCoreRefHolder gcRef { get; set; }
		public GameLogicRefHolder glRef { get; set; }
		public SerpCoreRefHolder scRef { get; set; }

		private void Awake()
		{
			DontDestroyOnLoad(this.gameObject);
		}
	}
}
