using Qbism.General;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Control
{
	public class InputDetector : MonoBehaviour
	{
		//Config parameters
		[SerializeField] ScreenStateManager screenStateMngr;

		//Cache
		GameControls controls;

		//States
		Vector2 stickValue;
		public bool inputting { get; set; } = false;

		private void Awake()
		{
			controls = new GameControls();

			controls.Gameplay.Movement.performed += ctx => stickValue = ctx.ReadValue<Vector2>();
			controls.Gameplay.Reset.performed += ctx => screenStateMngr.currentScreenState.HandleResetInput();
			controls.Gameplay.Rewind.performed += ctx => screenStateMngr.currentScreenState.HandleRewindInput();
			controls.Gameplay.Action.performed += ctx => screenStateMngr.currentScreenState.HandleActionInput();
			controls.Gameplay.Escape.performed += ctx => screenStateMngr.currentScreenState.HandleEscapeInput();
			controls.Gameplay.ANYkey.performed += ctx => screenStateMngr.currentScreenState.HandleAnyInput();
			controls.Gameplay.Back.performed += ctx => screenStateMngr.currentScreenState.HandleBackInput();
			
			controls.Gameplay.DebugKey1.performed += ctx =>
				screenStateMngr.currentScreenState.HandleDebugUnlockAllInput();
			controls.Gameplay.DebugKey2.performed += ctx =>
				screenStateMngr.currentScreenState.HandleDebugDeleteProgressInput();
			controls.Gameplay.DebugKey3.performed += ctx =>
				screenStateMngr.currentScreenState.HandleDebugCompleteAllInput();
			controls.Gameplay.CKey.performed += ctx =>
				screenStateMngr.currentScreenState.HandleDebugCompleteInput();
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

			if (!inputting) screenStateMngr.currentScreenState.HandleStickValues(stickValue, this);
		}

		private void OnDisable()
		{
			controls.Gameplay.Disable();
		}
	}

}