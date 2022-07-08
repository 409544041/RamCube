using Qbism.Saving;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace Qbism.Demo
{
	public class DebugHUDHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] PersistentRefHolder persRef;
		[SerializeField] TextMeshProUGUI buildText, sceneText, timeText;
		[SerializeField] CanvasGroup debugCanvasGroup;

		//States
		bool currentVisState = false, prevVisState = false;
		float timeInScene = 0;

		private void Start()
		{
			buildText.text = persRef.switchBoard.currentBuild;
			sceneText.text = SceneManager.GetActiveScene().name.ToString();
		}

		private void Update()
		{
			ShowOrHideUI();
			timeInScene += Time.deltaTime;
			timeText.text = Mathf.Round(timeInScene).ToString();
		}

		public void NewScene(string currentScene)
		{
			sceneText.text = currentScene;
			timeInScene = 0;
		}

		private void ShowOrHideUI()
		{
			prevVisState = currentVisState;
			currentVisState = persRef.switchBoard.showDebugTextInfo;
			
			if (currentVisState != prevVisState) 
				debugCanvasGroup.alpha = Convert.ToInt32(currentVisState);
		}


	}
}
