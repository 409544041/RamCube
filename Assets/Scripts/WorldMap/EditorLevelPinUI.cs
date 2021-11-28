using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Qbism.WorldMap
{
	[ExecuteAlways]
	public class EditorLevelPinUI : MonoBehaviour
	{
		//Config parameters
		[SerializeField] TextMeshProUGUI uiText;

		//Cache 
		LevelPinUI pinUI;

		private void Awake() 
		{
			pinUI = GetComponent<LevelPinUI>();
		}

		private void Start()
		{
			NameGameObject();
			SetUIText();
		}

		private void NameGameObject()
		{
			string pinID = pinUI.levelPin.m_levelData.f_Pin.f_name.ToString();
			transform.gameObject.name = "pin UI " + pinID;
		}

		private void SetUIText()
		{
			var uiText = pinUI.levelPin.m_levelData.f_Pin.f_PinTextUI.ToString();
			this.uiText.text = uiText;

			var butRecTrans = this.uiText.rectTransform;
			
			if (uiText == "1" || uiText == "11") 
				butRecTrans.anchoredPosition = new Vector3(-1.5f, butRecTrans.anchoredPosition.y, 0);
		}
	}
}

	