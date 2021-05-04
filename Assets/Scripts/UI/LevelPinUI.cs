using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using Qbism.SceneTransition;
using Qbism.WorldMap;
using UnityEngine;
using UnityEngine.UI;

namespace Qbism.UI
{
	public class LevelPinUI : MonoBehaviour
	{
		//Config parameters
		public LevelPin levelPin = null;
		[SerializeField] Button button = null;
		[SerializeField] Image lockIcon = null;

		//Cache
		EditorLevelPinUI editorPin = null;
		ProgressHandler progHandler = null;
		PinSelectionTracker pinSelTrack = null;
		public Vector3 uiPos { get; set; }

		private void Awake() 
		{
			editorPin = GetComponent<EditorLevelPinUI>();
			progHandler = FindObjectOfType<ProgressHandler>();
			pinSelTrack = FindObjectOfType<PinSelectionTracker>();
			uiPos = levelPin.GetComponentInChildren<LineRenderer>().transform.position;
		}

		private void OnEnable() 
		{
			if (progHandler != null)
			{
				progHandler.onSetUIComplete += SetUIComplete;
				progHandler.onShowOrHideUI += ShowOrHideUI;
				progHandler.onSelectPinUI += SelectPinUI;
				progHandler.onDisableLockIcon += DisableLockIcon;
			}

			if(levelPin != null) levelPin.onShowOrHideUI += ShowOrHideUI;

			if (pinSelTrack != null)
			{
				pinSelTrack.onPinFetch += FetchPin;
				pinSelTrack.onSelectPinUI += SelectPinUI;
			}
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
			LevelIDs id = levelPin.levelID;
			bool hasSerpent = levelPin.GetComponent<EditorSetPinValues>().hasSerpentSegment;
			progHandler.SetCurrentData(id, hasSerpent);
				
			var handler = FindObjectOfType<SceneHandler>();
			int indexToLoad = levelPin.GetComponent<EditorSetPinValues>().levelIndex;
			handler.LoadBySceneIndex(indexToLoad);
		}

		private void SetUIComplete(LevelPin pin)
		{
			if (pin.levelID == levelPin.levelID)
			{
				ColorBlock colors = button.colors;
				colors.normalColor = new Color32(86, 235, 111, 255);
				colors.highlightedColor = new Color32(137, 235, 156, 255);
				colors.pressedColor = new Color32(76, 133, 106, 255);
				button.colors = colors;
			}
		}

		private void SelectPinUI(LevelIDs id)
		{
			if(id == levelPin.levelID) button.Select();
		}

		public void ShowOrHideUI(LevelPin pin, bool value)
		{
			if(pin.levelID == levelPin.levelID)
			{
				GetComponentInChildren<Image>().enabled = value;
				GetComponentInChildren<Text>().enabled = value;
				GetComponentInChildren<Button>().enabled = value;
			}
		}

		private void FetchPin(GameObject selected)
		{
			if(selected == button.gameObject)
				pinSelTrack.selectedPin = levelPin;
		}

		public void DisableLockIcon(LevelIDs id)
		{
			if(id != levelPin.levelID) return;
			
			int locks = 0;
			int sheetLocks = 0;

			foreach (LevelStatusData data in progHandler.levelDataList)
			{
				if(data.levelID == levelPin.levelID)
					locks = data.locks;
			}

			foreach (LevelPin pin in progHandler.levelPinList)
			{
				if(pin.levelID == levelPin.levelID)
					sheetLocks = pin.GetComponent<EditorSetPinValues>().locks;
			}
			
			if(locks == sheetLocks) lockIcon.enabled = false;
		}

		private void OnDisable() 
		{
			if (progHandler != null) 
			{
				progHandler.onSetUIComplete -= SetUIComplete;
				progHandler.onShowOrHideUI -= ShowOrHideUI;
				progHandler.onSelectPinUI -= SelectPinUI;
				progHandler.onDisableLockIcon -= DisableLockIcon;
			}

			if (levelPin != null) levelPin.onShowOrHideUI -= ShowOrHideUI;

			if (pinSelTrack != null)
			{
				pinSelTrack.onPinFetch -= FetchPin;
				pinSelTrack.onSelectPinUI -= SelectPinUI;
			} 
		}
	}
}
