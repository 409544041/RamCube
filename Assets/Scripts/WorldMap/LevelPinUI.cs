﻿using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using Qbism.SceneTransition;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Qbism.WorldMap
{
	public class LevelPinUI : MonoBehaviour
	{
		//Config parameters
		public LevelPin levelPin = null;
		[SerializeField] Button button = null;
		public Image compIcon, compDiamond, unCompIcon;
		public TextMeshProUGUI uiText;
		public Color unCompText,  compText;
		[ColorUsage(true, true)]
		public Color unCompTextOutline, compTextOutline;
		public LevelPinUIJuicer pinUIJuice;

		//Actions, events, delegates etc
		public event Action<E_Pin, bool, bool, E_Biome> onSetCurrentData;

		public void LoadAssignedLevel() //Called from Unity Event 
		{				
			onSetCurrentData(levelPin.m_levelData.f_Pin, levelPin.m_levelData.f_SegmentPresent,
				levelPin.m_levelData.f_ObjectPresent, levelPin.m_Pin.f_Biome);

			pinUIJuice.PlayEnterLevelJuice();
				
			var loader = FindObjectOfType<LevelLoading>();
			loader.StartLoadingLevel(levelPin.m_levelData.f_Level);
		}

		public void SetUIState(bool compValue, bool diamondValue, bool unCompValue, 
			bool textValue, bool buttonValue)
		{
			if (compValue == true)
			{
				uiText.color = compText;
				uiText.outlineColor = compTextOutline;
			}

			else
			{
				uiText.color = unCompText; 
				uiText.outlineColor = unCompTextOutline;
			}

			compIcon.enabled = compValue;
			compDiamond.enabled = diamondValue;
			unCompIcon.enabled = unCompValue;
			uiText.enabled = textValue;
			button.enabled = buttonValue;
		}

		public void SelectPinUI()
		{
			button.Select();
		}

		public LevelPin FetchPin(GameObject selected)
		{
			if (selected == button.gameObject) return levelPin;
			else return null;
		}
	}
}
