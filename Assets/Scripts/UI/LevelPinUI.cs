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
		//Cache
		EditorLevelPinUI editorPin;
		ProgressHandler progHandler;

		private void Awake() 
		{
			editorPin = GetComponent<EditorLevelPinUI>();
			progHandler = FindObjectOfType<ProgressHandler>();
		}

		private void OnEnable() 
		{
			if (progHandler != null)
			{
				progHandler.onSetUIComplete += SetUIComplete;
				progHandler.onShowOrHideUI += ShowOrHideUI;
			}
		}

		public void LoadAssignedLevel() //Called from Unity Event on Clickable Object
		{
			editorPin.levelPin.GetComponent<LevelPin>().SetCurrentLevelID();
			var handler = FindObjectOfType<SceneHandler>();
			int indexToLoad = editorPin.levelPin.GetComponent<EditorSetPinValues>().levelIndex;
			handler.LoadBySceneIndex(indexToLoad);
		}

		private void SetUIComplete(LevelPin pin)
		{
			if (pin.levelID == editorPin.levelPin.levelID)
			{
				Button button = GetComponentInChildren<Button>();
				ColorBlock colors = button.colors;
				colors.normalColor = new Color32(86, 235, 111, 255);
				colors.highlightedColor = new Color32(137, 235, 156, 255);
				colors.pressedColor = new Color32(76, 133, 106, 255);
				button.colors = colors;
			}
		}

		public void ShowOrHideUI(LevelPin pin, bool value)
		{
			if(pin.levelID == editorPin.levelPin.levelID)
			{
				GetComponentInChildren<Image>().enabled = value;
				GetComponentInChildren<Text>().enabled = value;
			}
		}

		private void OnDisable() 
		{
			if (progHandler != null) 
			{
				progHandler.onSetUIComplete -= SetUIComplete;
				progHandler.onShowOrHideUI -= ShowOrHideUI;
			}
		}
	}
}
