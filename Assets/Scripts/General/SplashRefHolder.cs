using Qbism.SceneTransition;
using Qbism.ScreenStateMachine;
using Qbism.Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Qbism.Saving;

namespace Qbism.General
{
	public class SplashRefHolder : MonoBehaviour
	{
		//Config parameters
		public Camera cam;
		public PersistentRefHolder persRef;
		public ScreenStateManager screenStateMngr;
		public SplashSceneLoading splashSceneLoading;
		public InitialDisplaySetter displaySetter;
		public SplashMenuHandler menuHandler;
		public LogoFader logoFader;
		public OverlayMenuHandler settingsMenu;
		public GaussianCanvas gausCanvas;
		public Canvas splashCanvas;
	}
}
