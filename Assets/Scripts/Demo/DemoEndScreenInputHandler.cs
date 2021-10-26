using System.Collections;
using System.Collections.Generic;
using Qbism.SceneTransition;
using UnityEngine;

namespace Qbism.Demo
{
	public class DemoEndScreenInputHandler : MonoBehaviour
	{
		//Cache
		GameControls controls;
		SceneHandler loader;

		private void Awake()
		{
			loader = GetComponent<SceneHandler>();
			controls = new GameControls();
			controls.Gameplay.Rewind.performed += ctx => PlayFromStart();
			controls.Gameplay.QuitGame.performed += ctx => QuitApplication();
		}

		private void OnEnable()
		{
			controls.Gameplay.Enable();
		}

		private void PlayFromStart()
		{
			loader.LoadBySceneIndex(0);
		}

		private void QuitApplication()
		{
			Application.Quit();
		}

		private void OnDisable()
		{
			controls.Gameplay.Disable();
		}
	}
}
