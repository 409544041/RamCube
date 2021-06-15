using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using Qbism.SceneTransition;
using UnityEngine;
using UnityEngine.UI;

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
		public event Action<LevelIDs, bool, Biomes> onSetCurrentData;
		
		public delegate List<LevelStatusData> LevelDataDel();
		public LevelDataDel onFetchLevelData;

		public delegate List<LevelPin> PinDataDel();
		public PinDataDel onFetchLevelPins;

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
			LevelIDs id = levelPin.levelID;
			var editorPinVal = levelPin.GetComponent<EditorSetPinValues>();
			bool hasSerpent = editorPinVal.hasSerpentSegment;
			Biomes biome = editorPinVal.biome;
			onSetCurrentData(id, hasSerpent, biome);
				
			var handler = FindObjectOfType<SceneHandler>();
			int indexToLoad = levelPin.GetComponent<EditorSetPinValues>().levelIndex;
			handler.LoadBySceneIndex(indexToLoad);
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
			int locks = 0;
			int sheetLocks = 0;

			List<LevelStatusData> levelDataList = onFetchLevelData();
			List<LevelPin> levelPinList = onFetchLevelPins();

			foreach (LevelStatusData data in levelDataList)
			{
				if(data.levelID == levelPin.levelID)
					locks = data.locks;
			}

			foreach (LevelPin pin in levelPinList)
			{
				if(pin.levelID == levelPin.levelID)
					sheetLocks = pin.GetComponent<EditorSetPinValues>().locks;
			}
			
			if(locks == sheetLocks) lockIcon.enabled = false;
		}

		private void OnDisable() 
		{
			if (pinSelTrack != null)
				pinSelTrack.onPinFetch -= FetchPin;
		}
	}
}
