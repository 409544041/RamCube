using System;
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
		public Image compIcon, compDiamond, unCompIcon, lockIcon;
		public TextMeshProUGUI uiText;
		public Color unCompText,  compText;
		[ColorUsage(true, true)]
		public Color unCompTextOutline, compTextOutline;
		public LevelPinUIJuicer pinUIJuice;

		//Cache
		EditorLevelPinUI editorPin = null;
		PinSelectionTracker pinSelTrack = null;

		//Actions, events, delegates etc
		public event Action<E_Pin, bool, E_Biome> onSetCurrentData;

		private void Awake() 
		{
			editorPin = GetComponent<EditorLevelPinUI>();
			pinSelTrack = FindObjectOfType<PinSelectionTracker>();
		}

		private void OnEnable() 
		{
			if (pinSelTrack != null)
				pinSelTrack.onPinFetch += FetchPin;
		}

		public void LoadAssignedLevel() //Called from Unity Event 
		{				
			onSetCurrentData(levelPin.m_levelData.f_Pin, levelPin.m_levelData.f_SegmentPresent,
				levelPin.m_Pin.f_Biome);
				
			var handler = FindObjectOfType<SceneHandler>();
			handler.LoadBySceneIndex(levelPin.m_levelData.f_Pin.f_Index);
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

		private void FetchPin(GameObject selected)
		{
			if(selected == button.gameObject)
				pinSelTrack.selectedPin = levelPin;
		}

		public void DisableLockIcon()
		{	
			var ent = E_LevelGameplayData.FindEntity(entity =>
				entity.f_Pin == levelPin.m_levelData.f_Pin);
			int locksLeft = ent.f_LocksLeft;
			bool lockDisabled = ent.f_LockIconDisabled;
			
			if(locksLeft == levelPin.m_levelData.f_LocksAmount || 
				(locksLeft == 0 && lockDisabled))
				lockIcon.enabled = false;	
		}

		private void OnDisable() 
		{
			if (pinSelTrack != null)
				pinSelTrack.onPinFetch -= FetchPin;
		}
	}
}
