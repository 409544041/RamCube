using Qbism.General;
using Qbism.SceneTransition;
using Qbism.ScreenStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Qbism.Demo
{
	public class DemoEndHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] OverlayButtonHandler[] buttons;
		[SerializeField] Color textColor, selectedTextColor;
		[SerializeField] float selectedButtonSize = 1.5f;
		[SerializeField] ScreenStateManager screenStateMngr;
		[SerializeField] WorldMapLoading mapLoader;

		//States
		public OverlayButtonHandler selectedButtonHandler { get; private set; }
		OverlayButtonHandler prevButtonHandler;

		private void Awake()
		{
			foreach (var button in buttons)
			{
				button.mapLoader = mapLoader;
			}
		}

		private void Start()
		{
			buttons[0].SelectButton(selectedTextColor, selectedButtonSize,
				screenStateMngr);
		}

		private void Update()
		{
			prevButtonHandler = selectedButtonHandler;
			GameObject selected = EventSystem.current.currentSelectedGameObject;

			for (int i = 0; i < buttons.Length; i++)
			{
				var buttonHandler = buttons[i].FetchButtonHandler(selected); //Sets new selected button
				if (buttonHandler == null) continue;

				selectedButtonHandler = buttonHandler;
				break;
			}

			if (selectedButtonHandler != prevButtonHandler)
			{
				selectedButtonHandler.SelectButton(selectedTextColor, selectedButtonSize,
					screenStateMngr);

				if (prevButtonHandler != null) prevButtonHandler.DeselectButton(textColor);
			}
		}
	}
}
