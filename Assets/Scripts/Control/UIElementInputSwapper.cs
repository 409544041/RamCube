using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace Qbism.Control
{
	public class UIElementInputSwapper : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Image keyboardButton, controllerButton;
		[SerializeField] TextMeshProUGUI keyboardText, controllerText;
		[SerializeField] Color blackButtonColor, blueButtonColor, yellowButtonColor,
			redButtonColor, greenButtonColor, blackTextColor, blueTextColor, yellowTextColor,
			redTextColor, greenTextColor;
		[SerializeField] bool blackButton, blueButton, yellowButton, redButton, greenButton;

		//States
		public PlayerInput pInput { get; set; }

		private void Awake()
		{
			SetControllerButtonColors();

			UpdateButtonImage(pInput.currentControlScheme);
		}

		private void OnEnable()
		{
			InputUser.onChange += HandleInputChange;
		}

		private void HandleInputChange(InputUser user, InputUserChange change, InputDevice device)
		{
			if (change == InputUserChange.ControlSchemeChanged)
			{
				UpdateButtonImage(user.controlScheme.Value.name);
			}
		}

		private void UpdateButtonImage(string schemeName)
		{
			if (schemeName == "Gamepad")
			{
				keyboardButton.enabled = false;
				keyboardText.enabled = false;
				controllerButton.enabled = true;
				controllerText.enabled = true;
			}
			else if (schemeName == "Keyboard")
			{
				keyboardButton.enabled = true;
				keyboardText.enabled = true;
				controllerButton.enabled = false;
				controllerText.enabled = false;
			}
		}

		private void SetControllerButtonColors()
		{
			if (blackButton)
			{
				controllerButton.color = blackButtonColor;
				controllerText.color = blackTextColor;
			}
			else if (blueButton)
			{
				controllerButton.color = blueButtonColor;
				controllerText.color = blueTextColor;
			}
			else if (yellowButton)
			{
				controllerButton.color = yellowButtonColor;
				controllerText.color = yellowTextColor;
			}
			else if (redButton)
			{
				controllerButton.color = redButtonColor;
				controllerText.color = redTextColor;
			}
			else if (greenButton)
			{
				controllerButton.color = greenButtonColor;
				controllerText.color = greenTextColor;
			}
		}

		private void OnDisable()
		{
			InputUser.onChange -= HandleInputChange;
		}
	}
}
