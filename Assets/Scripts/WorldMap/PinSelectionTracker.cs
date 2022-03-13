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
		[SerializeField] MapCoreRefHolder mcRef;

		//Cache
		MapLogicRefHolder mlRef;
		LevelPinUI[] pinUIs;

		//States
		public LevelPin selectedPin { get; set; } = null;
		public E_Biome currentBiome { get; set; }
		LevelPin prevPin;
		
		//Actions, events, delegates etc
		public Func<LevelPin> onSavedPinFetch;
		public Func<E_Biome> onSavedBiomeFetch;

		private void Awake() 
		{
			mlRef = mcRef.mlRef;
			pinUIs = FindObjectsOfType<LevelPinUI>();
		}

		private void Start()
		{
			//Sets pin and biome of saved 'currentLevelID' as selectedPin and currentBiome
			selectedPin = onSavedPinFetch();
			currentBiome = onSavedBiomeFetch();

			selectedPin.pinUI.SelectPinUI();
			SetPinSelectionLoc();
			selectedPin.pinUI.pinUIJuice.SelectionEnlargen(1, selectedPin.pinUI.pinUIJuice.selectedSize);
		}

		private void Update() 
		{
			prevPin = selectedPin;

			GameObject selected = EventSystem.current.currentSelectedGameObject;

			for (int i = 0; i < pinUIs.Length; i++)
			{
				LevelPin pin = pinUIs[i].FetchPin(selected); //Sets new selectedPin
				if (pin == null) continue;
				
				selectedPin = pin;
				break;
			}

			currentBiome = selectedPin.m_Pin.f_Biome;

			if(selectedPin != prevPin)
			{
				mlRef.centerPoint.StartPositionCenterPoint(currentBiome, selectedPin, false, 
					false, true, new Vector2(0, 0));
				SetPinSelectionLoc();
				selectedPin.pinUI.pinUIJuice.SelectionEnlargen(1, selectedPin.pinUI.pinUIJuice.selectedSize);
				prevPin.pinUI.pinUIJuice.SelectionEnlargen(selectedPin.pinUI.pinUIJuice.selectedSize, 1);
			} 
		}

		private void SetPinSelectionLoc()
		{
			mlRef.pinSelShape.transform.position = new Vector3 (selectedPin.transform.position.x, 
				mlRef.pinSelShape.transform.position.y, selectedPin.transform.position.z);
		}
	}
}
