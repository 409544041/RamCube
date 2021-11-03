using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using Qbism.SceneTransition;
using UnityEngine;
using UnityEngine.UI;
using BansheeGz.BGDatabase;

namespace Qbism.WorldMap
{
	public class LevelPinUI : MonoBehaviour
	{
		//Config parameters
		public LevelPin levelPin = null;
		[SerializeField] Button button = null;
		[SerializeField] Image lockIcon = null;

		//Cache
		EditorLevelPinUI editorPin = null;
		PinSelectionTracker pinSelTrack = null;
		public Vector3 uiPos { get; set; }

		//Actions, events, delegates etc
		public event Action<E_Pin, bool, E_Biome> onSetCurrentData;

		private void Awake() 
		{
			editorPin = GetComponent<EditorLevelPinUI>();
			pinSelTrack = FindObjectOfType<PinSelectionTracker>();
			uiPos = levelPin.GetComponentInChildren<LineRenderer>().transform.position;
		}

		private void OnEnable() 
		{
			if (pinSelTrack != null)
				pinSelTrack.onPinFetch += FetchPin;
		}

		private void Update() 
		{
			PositionElement();
		}

		private void PositionElement()
		{
			Vector2 screenPoint = Camera.main.WorldToScreenPoint(uiPos);
			transform.position = screenPoint;
		}

		public void LoadAssignedLevel() //Called from Unity Event 
		{				
			onSetCurrentData(levelPin.m_levelData.f_Pin, levelPin.m_levelData.f_SegmentPresent,
				levelPin.m_Pin.f_Biome);
				
			var handler = FindObjectOfType<SceneHandler>();
			handler.LoadBySceneIndex(levelPin.m_levelData.f_Pin.f_Index);
		}

		public void SetUIComplete()
		{
			ColorBlock colors = button.colors;
			colors.normalColor = new Color32(86, 235, 111, 255);
			colors.highlightedColor = new Color32(137, 235, 156, 255);
			colors.pressedColor = new Color32(76, 133, 106, 255);
			button.colors = colors;
		}

		public void SelectPinUI()
		{
			button.Select();
		}

		public void ShowOrHideUI(bool value)
		{
			GetComponentInChildren<Image>().enabled = value;
			GetComponentInChildren<Text>().enabled = value;
			GetComponentInChildren<Button>().enabled = value;
		}

		private void FetchPin(GameObject selected)
		{
			if(selected == button.gameObject)
				pinSelTrack.selectedPin = levelPin;
		}

		public void DisableLockIcon()
		{	
			if (!lockIcon.isActiveAndEnabled) return;	

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
