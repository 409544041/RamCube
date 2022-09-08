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
		[SerializeField] EventSystem eventSystem;
		[SerializeField] MapCoreRefHolder mcRef;

		//Cache
		MapLogicRefHolder mlRef;
		LevelPinUI[] pinUIs;

		//States
		public LevelPinRefHolder selectedPin { get; set; } = null;
		public E_Biome currentBiome { get; set; }
		LevelPinRefHolder prevPin;

		//Actions, events, delegates etc
		public Func<LevelPinRefHolder> onSavedPinFetch;
		public Func<E_Biome> onSavedBiomeFetch;

		private void Awake()
		{
			mlRef = mcRef.mlRef;

			var pins = mcRef.mlRef.levelPins;
			pinUIs = new LevelPinUI[pins.Length];

			for (int i = 0; i < pinUIs.Length; i++)
			{
				pinUIs[i] = pins[i].pinUI;
			}
		}

		private void Start()
		{
			selectedPin = onSavedPinFetch();
			currentBiome = onSavedBiomeFetch();
		}

		public void SelectPinOnMapLoad()
		{
			selectedPin = onSavedPinFetch();
			mlRef.mapCursor.PlaceCursor(selectedPin, true, false, true, new Vector2(0,0));
		}

		public void SelectPin(LevelPinUI pinToSelect)
		{
			prevPin = selectedPin;
			selectedPin = pinToSelect.refs;

			if (prevPin == selectedPin) return;
			DeselectPin(false);
			pinToSelect.SelectPinUI();
			currentBiome = selectedPin.m_pin.f_Biome;
			SetPinSelectionLoc();
			selectedPin.pinUIJuicer.SelectionEnlargen(1, selectedPin.pinUIJuicer.selectedSize);
		}

		public void DeselectPin(bool cursorOverEmpty)
		{
			eventSystem.SetSelectedGameObject(null);
			if (prevPin != null) prevPin.pinUIJuicer.SelectionEnlargen(selectedPin.pinUIJuicer.selectedSize, 1);
			
			if (cursorOverEmpty)
			{
				selectedPin = null;
				mlRef.pinSelShapeRend.enabled = false;
			}
			
			prevPin = null;
		}

		public void SetLevelPinButtonsInteractable(bool value)
		{
			foreach (var pin in mlRef.levelPins)
			{
				pin.pinUI.button.interactable = value;
			}
		}

		private void SetPinSelectionLoc()
		{
			mlRef.pinSelShapeRend.enabled = true;
			mlRef.pinSelShapeTrans.position = new Vector3 (selectedPin.transform.position.x, 
				mlRef.pinSelShapeTrans.transform.position.y, selectedPin.transform.position.z);
		}
	}
}
