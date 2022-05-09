using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

namespace Qbism.General
{
	public class OverlayButtonHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] TextMeshProUGUI buttonText;
		[SerializeField] Button button;
		[SerializeField] OverlayButtons buttonType;

		public OverlayButtons SelectButton(Color selectedColor)
		{
			button.Select();
			buttonText.color = selectedColor;
			return buttonType;
		}

		public void DeselectButton(Color defaultColor)
		{
			buttonText.color = defaultColor;
		}

		public OverlayButtonHandler FetchButtonHandler(GameObject selectedGameObject)
		{
			if (selectedGameObject == this.gameObject) return this;
			else return null;
		}

		
	}
}
