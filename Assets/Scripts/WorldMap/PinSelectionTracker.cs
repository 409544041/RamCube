﻿using System;
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

		//Cache
		PositionBiomeCenterpoint centerPoint;

		//States
		public LevelPin selectedPin { get; set; } = null;
		public E_Biome currentBiome { get; set; }
		E_Biome prevBiome;
		LevelPin prevPin;
		
		//Actions, events, delegates etc
		public event Action<GameObject> onPinFetch;
		public Func<LevelPin> onSavedPinFetch;
		public Func<E_Biome> onSavedBiomeFetch;

		private void Awake() 
		{
			centerPoint = FindObjectOfType<PositionBiomeCenterpoint>();
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
			prevBiome = currentBiome;
			prevPin = selectedPin;

			GameObject selected = EventSystem.current.currentSelectedGameObject;
			onPinFetch(selected); //Sets new selectedPin
			currentBiome = selectedPin.m_Pin.f_Biome;

			if(selectedPin != prevPin)
			{
				centerPoint.StartPositionCenterPoint(currentBiome, selectedPin, false, 
					false, true, new Vector2(0, 0));
				SetPinSelectionLoc();
				selectedPin.pinUI.pinUIJuice.SelectionEnlargen(1, selectedPin.pinUI.pinUIJuice.selectedSize);
				prevPin.pinUI.pinUIJuice.SelectionEnlargen(selectedPin.pinUI.pinUIJuice.selectedSize, 1);
			} 
		}

		private void SetPinSelectionLoc()
		{
			pinSelectionFX.transform.position = new Vector3 (selectedPin.transform.position.x, 
				pinSelectionFX.transform.position.y, selectedPin.transform.position.z);
		}
	}
}
