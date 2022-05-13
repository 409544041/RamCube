using Qbism.Control;
using Qbism.PlayerCube;
using Qbism.Saving;
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

		//Cache
		public PersistentRefHolder persRef { get; private set; }

		//States
		public IScreenBaseState currentScreenState { get; private set; }
		public IScreenBaseState prevScreenState { get; private set; }
		public ScreenStates currentStateEnum { get; private set; }
		public ScreenStates prevStateEnum { get; private set; }

		private void Awake()
		{
			if (gcRef != null)
			{
				currentScreenState = levelIntroState;
				currentStateEnum = ScreenStates.levelIntroState;
				persRef = gcRef.persRef;
			}

			if (mcRef != null)
			{
				currentScreenState = mapScreenState;
				currentStateEnum = ScreenStates.mapScreenState;
				persRef = mcRef.persRef;
			}

			if (scRef != null)
			{
				currentScreenState = serpentScreenState;
				currentStateEnum = ScreenStates.serpentScreenState;
				persRef = scRef.persRef;
			}

			if (isSplash)
			{
				currentScreenState = splashScreenState;
				currentStateEnum = ScreenStates.splashScreenState;
			}

			if (isDemoIntro)
			{
				currentScreenState = demoIntroScreenState;
				currentStateEnum = ScreenStates.demoIntroScreenState;
			}

			if (isDemoEnd)
			{
				currentScreenState = demoEndScreenState;
				currentStateEnum = ScreenStates.demoEndScreenState;
			}
		}

		private void Start()
		{
			currentScreenState.StateEnter(this);
		}

		public void SwitchState(IScreenBaseState state, ScreenStates stateEnum)
		{
			prevScreenState = currentScreenState;
			prevStateEnum = currentStateEnum;

			currentScreenState = state;
			currentStateEnum = stateEnum;

			prevScreenState.StateExit();
			currentScreenState.StateEnter(this);
		}
	}
}
