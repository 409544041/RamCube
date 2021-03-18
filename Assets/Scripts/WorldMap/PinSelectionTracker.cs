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

		//Actions, events, delegates etc
		public event Action<GameObject> onPinFetch;
		public event Action<Biomes> onChangeBiome;

		private void Update() 
		{
			prevBiome = currentBiome;
			GameObject selected = EventSystem.current.currentSelectedGameObject;
			onPinFetch(selected);
			currentBiome = selectedPin.biome;

			if(currentBiome != prevBiome) onChangeBiome(currentBiome);
		}
	}
}
