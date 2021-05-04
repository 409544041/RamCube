using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Qbism.WorldMap
{
	public class PinSelectionTracker : MonoBehaviour
	{
		//States
		public LevelPin selectedPin { get; set; } = null;
		public Biomes currentBiome { get; set; }
		Biomes prevBiome;
		LevelPin prevPin;

		//Actions, events, delegates etc
		public event Action<GameObject> onPinFetch;
		public event Action<Biomes, LevelPin> onSetCenterPos;
		public event Action onSavedPinFetch;

		private IEnumerator Start() 
		{
			//To make sure selectedPin values are in before onSelectPinUI starts
			//TO DO: Find a permanent solution to avoid race conditions. Perhaps LazyValues?
			yield return new WaitForEndOfFrame();

			//Sets pin and biome of saved 'currentLevelID' as selectedPin and currentBiome
			onSavedPinFetch(); 
			selectedPin.pinUI.SelectPinUI();
			onSetCenterPos(currentBiome, selectedPin);
		}

		private void Update() 
		{
			prevBiome = currentBiome;
			prevPin = selectedPin;

			GameObject selected = EventSystem.current.currentSelectedGameObject;
			onPinFetch(selected); //Sets new selectedPin
			currentBiome = selectedPin.GetComponent<EditorSetPinValues>().biome;
			if(selectedPin != prevPin) onSetCenterPos(currentBiome, selectedPin);
		}
	}
}
