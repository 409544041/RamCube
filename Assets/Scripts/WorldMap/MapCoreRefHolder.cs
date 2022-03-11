using Cinemachine;
using Dreamteck.Splines;
using Qbism.General;
using Qbism.Saving;
using Qbism.Serpent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		public MapLogicRefHolder mapLogicRef;
		[Header("Canvasses")]
		public Canvas worldMapCanvas;
		[Header("Music")]
		public AudioSource musicSource;
		public MusicFadeOut musicFadeOut;

		//Cache
		public PersistentRefHolder persistantRef { get; private set; }

		private void Awake()
		{
			persistantRef = FindObjectOfType<PersistentRefHolder>();
		}
	}
}
