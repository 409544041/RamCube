using Qbism.SceneTransition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Qbism.General
{
	public class SplashMenuHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] FeatureSwitchBoard switchBoard;
		public SplashSceneLoading splashLoader;
		[SerializeField] CanvasGroup normalCanvasGroup, demoCanvasGroup;
		[SerializeField] OverlayButtonHandler[] demoCanvasButtons;
		[SerializeField] Color textColor, selectedTextColor;
		[SerializeField] float selectedButtonSize = 1.5f;
		[SerializeField] ScreenStateManager screenStateMngr;

		//States
		public OverlayButtonHandler selectedButtonHandler { get; private set; }
		OverlayButtonHandler prevButtonHandler;

		private void Awake()
		{
			if (switchBoard.isPublicDemo)
			{
				normalCanvasGroup.alpha = 0;
				demoCanvasGroup.alpha = 1;
				SetButtonsInteractable(true);
			}
			else
			{
				normalCanvasGroup.alpha = 1;
				demoCanvasGroup.alpha = 0;
				SetButtonsInteractable(false);
			}
		}

		private void Update()
		{
			prevButtonHandler = selectedButtonHandler;
			GameObject selected = EventSystem.current.currentSelectedGameObject;

			for (int i = 0; i < demoCanvasButtons.Length; i++)
			{
				var buttonHandler = demoCanvasButtons[i].FetchButtonHandler(selected); //Sets new selected button
				if (buttonHandler == null) continue;

				selectedButtonHandler = buttonHandler;
				break;
			}

			if (selectedButtonHandler != prevButtonHandler)
			{
				selectedButtonHandler.SelectButton(selectedTextColor, selectedButtonSize,
					null, this, screenStateMngr);

				if (prevButtonHandler != null) prevButtonHandler.DeselectButton(textColor);
			}
		}

		private void SetButtonsInteractable(bool value)
		{
			foreach (var buttonHandler in demoCanvasButtons)
			{
				buttonHandler.button.interactable = value;
			}
		}
	}
}
