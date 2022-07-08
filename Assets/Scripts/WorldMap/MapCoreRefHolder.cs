using Cinemachine;
using Dreamteck.Splines;
using Qbism.Environment;
using Qbism.General;
using Qbism.Saving;
using Qbism.Serpent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Qbism.WorldMap
{
	public class MapCoreRefHolder : MonoBehaviour
	{
		//Config parameters
		[Header("Cameras")]
		public Camera cam;
		public CinemachineBrain camBrain;
		public CinemachineVirtualCamera mapCam;
		[Header("Mother Dragon")]
		public SerpentMapHandler serpMapHandler;
		public SerpentSegmentHandler serpSegHandler;
		public SerpentMovement serpMover;
		public SplineFollower splineFollower;
		public SplineComputer[] splines;
		[Header("Logic")]
		public MapLogicRefHolder mlRef;
		[Header("Map Canvas")]
		public Canvas worldMapCanvas;
		public CanvasGroup mapCanvasGroup;
		public SerpScreenButtonToggler serpButtonToggler;
		[Header("Pause Canvas")]
		public OverlayMenuHandler pauseOverlayHandler;
		[Header("Settings Canvas")]
		public OverlayMenuHandler settingsOverlayHandler;
		public CanvasGroup settingsOverlayCanvasGroup;
		public GaussianCanvas gausCanvas;
		[Header("Music")]
		public AudioSource musicSource;
		public MusicFadeOut musicFader;

		//Cache
		public PersistentRefHolder persRef { get; private set; }
		public BiomeVisualSwapper[] visualSwappers { get; private set; }
		public SegmentRefHolder[] segRefs { get; private set; }

		private void Awake()
		{
			persRef = FindObjectOfType<PersistentRefHolder>();
			persRef.cam = cam;
			persRef.mcRef = this;
			persRef.mlRef = mlRef;

			visualSwappers = FindObjectsOfType<BiomeVisualSwapper>();
			foreach (var swapper in visualSwappers)
			{
				swapper.progHandler = persRef.progHandler;
				swapper.matHandler = persRef.varMatHandler;
				swapper.mcRef = this;
			}

			foreach (var pin in mlRef.levelPins)
			{
				pin.mcRef = this;
			}

			segRefs = FindObjectsOfType<SegmentRefHolder>();
			foreach (var segRef in segRefs)
			{
				segRef.mcRef = this;
				segRef.cam = cam;
			}

			persRef.debugHUD.NewScene(SceneManager.GetActiveScene().name.ToString());
		}
	}
}
