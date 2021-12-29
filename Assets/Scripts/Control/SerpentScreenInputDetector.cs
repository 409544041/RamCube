﻿using System.Collections;
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
		SerpentScreenScroller serpScroller;

		//States
		Vector2 stickValue;
		bool inputting = false;

		private void Awake() 
		{
			controls = new GameControls();
			controls.Gameplay.Movement.performed += ctx => stickValue = ctx.ReadValue<Vector2>();
			controls.Gameplay.DebugKey4.performed += ctx => LoadWorldMap();
			serpScroller = GetComponent<SerpentScreenScroller>();
		}

		private void OnEnable()
		{
			controls.Gameplay.Enable();
		}

		void Update()
		{
			//inputting is to avoid multiple movement inputs at once
			if (stickValue.x > -.1 && stickValue.x < .1 &&
				stickValue.y > -.1 && stickValue.y < .1)
				inputting = false;

			if (!inputting) HandleStickValues();
		}

		private void HandleStickValues()
        {
			if (stickValue.x > .5)
			{
				serpScroller.ScrollLeft();
				inputting = true;
			}
			else if (stickValue.x < -.5)
			{
				serpScroller.ScrollRight();
				inputting = true;
			}
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
