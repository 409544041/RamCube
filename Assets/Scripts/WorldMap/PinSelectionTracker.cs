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
		public LevelPin selectedPin { get; set; }
		public Biomes currentBiome { get; private set; }
		Biomes prevBiome;
		LevelPin prevPin;

		//Actions, events, delegates etc
		public event Action<GameObject> onPinFetch;
		public event Action<Biomes, LevelPin> onSetCenterPos;

		private void Update() 
		{
			prevBiome = currentBiome;
			prevPin = selectedPin;

			GameObject selected = EventSystem.current.currentSelectedGameObject;
			onPinFetch(selected); //Sets new selectedPin
			currentBiome = selectedPin.biome;

			if(selectedPin != prevPin) onSetCenterPos(currentBiome, selectedPin);
		}
	}
}
