using Qbism.Control;
using Qbism.PlayerCube;
using Qbism.Serpent;
using Qbism.WorldMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class ScreenStateManager : MonoBehaviour
	{
		//Config parameters
		public LevelScreenState levelScreenState;
		public LevelIntroState levelIntroState;
		public LevelEndSeqState levelEndSeqState;
		public MapScreenState mapScreenState;
		public SerpentScreenState serpentScreenState;
		public PauseOverlayState pauseOverlayState;
		public ConfirmOverlayState confirmOverlayState;
		public SettingsOverlayState settingsOverlayState;
		public DialogueOverlayState dialogueOverlayState;
		public ObjectOverlayState objectOverlayState;
		public SplashScreenState splashScreenState;
		public DemoIntroScreenState demoIntroScreenState;
		public DemoEndScreenState demoEndScreenState;
		public GameplayCoreRefHolder gcRef;
		public MapCoreRefHolder mcRef;
		public SerpCoreRefHolder scRef;
		[SerializeField] bool isSplash = false, isDemoIntro = false, isDemoEnd = false;

		//States
		public IScreenBaseState currentScreenState { get; private set; }
		public IScreenBaseState prevScreenState { get; private set; }
		string currentStateString; //just for easy reading in debug inspector
		string prevStateString;

		private void Awake()
		{
			if (gcRef != null) currentScreenState = levelIntroState;
			if (mcRef != null) currentScreenState = mapScreenState;
			if (scRef != null) currentScreenState = serpentScreenState;
			if (isSplash) currentScreenState = splashScreenState;
			if (isDemoIntro) currentScreenState = demoIntroScreenState;
			if (isDemoEnd) currentScreenState = demoEndScreenState;
		}

		private void Start()
		{
			currentStateString = currentScreenState.ToString();
			currentScreenState.StateEnter(this);
		}

		public void SwitchState(IScreenBaseState state)
		{
			prevScreenState = currentScreenState;
			currentScreenState = state;

			currentStateString = currentScreenState.ToString();
			prevStateString = prevScreenState.ToString();

			prevScreenState.StateExit();
			currentScreenState.StateEnter(this);
		}
	}
}
