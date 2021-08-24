using System.Collections;
using System.Collections.Generic;
using Qbism.SceneTransition;
using Qbism.Serpent;
using UnityEngine;

namespace Qbism.Control
{
	public class SerpentScreenInputDetector : MonoBehaviour
	{
		//Cache
		GameControls controls;

		//States
		Vector2 stickValue;

		private void Awake() 
		{
			controls = new GameControls();
			controls.Gameplay.Movement.performed += ctx => stickValue = ctx.ReadValue<Vector2>();
			controls.Gameplay.DebugKey4.performed += ctx => LoadWorldMap();
		}

		private void OnEnable()
		{
			controls.Gameplay.Enable();
		}

		void Update()
		{
			GetComponent<SerpentScreenSplineHandler>().Scroll(stickValue);
		}

		private void LoadWorldMap()
		{
			SceneHandler sceneHandler = GetComponent<SceneHandler>();
			sceneHandler.LoadWorldMap();
		}

		private void OnDisable()
		{
			controls.Gameplay.Disable();
		}
	}
}
