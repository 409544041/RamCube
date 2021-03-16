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

		//Cache
		EditorLevelPinUI editorPin = null;
		ProgressHandler progHandler = null;
		Button button = null;

		private void Awake() 
		{
			editorPin = GetComponent<EditorLevelPinUI>();
			progHandler = FindObjectOfType<ProgressHandler>();
			button = GetComponentInChildren<Button>();
		}

		private void OnEnable() 
		{
			if (progHandler != null)
			{
				progHandler.onSetUIComplete += SetUIComplete;
				progHandler.onShowOrHideUI += ShowOrHideUI;
				progHandler.onSelectPinUI += SelectPinUI;
			}

			if(levelPin != null) levelPin.onShowOrHideUI += ShowOrHideUI;
		}

		public void LoadAssignedLevel() //Called from Unity Event on Clickable Object
		{
			levelPin.SetCurrentLevelID();
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

		private void OnDisable() 
		{
			if (progHandler != null) 
			{
				progHandler.onSetUIComplete -= SetUIComplete;
				progHandler.onShowOrHideUI -= ShowOrHideUI;
				progHandler.onSelectPinUI -= SelectPinUI;
			}

			if (levelPin != null) levelPin.onShowOrHideUI -= ShowOrHideUI;
		}
	}
}
