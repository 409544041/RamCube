using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Qbism.WorldMap
{
	public class PinSelectionTracker : MonoBehaviour
	{
		//Config parameters
		[SerializeField] GameObject pinSelectionFX;

		//States
		public LevelPin selectedPin { get; set; } = null;
		public E_Biome currentBiome { get; set; }
		E_Biome prevBiome;
		LevelPin prevPin;
		
		//Actions, events, delegates etc
		public event Action<GameObject> onPinFetch;
		public event Action<E_Biome, LevelPin> onSetCenterPos;
		public event Action onSavedPinFetch;

		private void Start() 
		{
			//Sets pin and biome of saved 'currentLevelID' as selectedPin and currentBiome
			onSavedPinFetch(); 
			selectedPin.pinUI.SelectPinUI();
			SetPinSelectionLoc();
			onSetCenterPos(currentBiome, selectedPin);
		}

		private void Update() 
		{
			prevBiome = currentBiome;
			prevPin = selectedPin;

			GameObject selected = EventSystem.current.currentSelectedGameObject;
			onPinFetch(selected); //Sets new selectedPin
			currentBiome = selectedPin.m_Pin.f_Biome;
			if(selectedPin != prevPin)
			{
				onSetCenterPos(currentBiome, selectedPin);
				SetPinSelectionLoc();
			} 
		}

		private void SetPinSelectionLoc()
		{
			pinSelectionFX.transform.position = new Vector3 (selectedPin.transform.position.x, 
				pinSelectionFX.transform.position.y, selectedPin.transform.position.z);
		}
	}
}
