using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using Qbism.UI;
using Qbism.WorldMap;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Qbism.General
{
	public class PositionBiomeCenterpoint : MonoBehaviour
	{
		//Cache
		ProgressHandler progHandler = null;
		PinSelectionTracker selTracker = null;

		//States
		Biomes currentBiome;
		float leftest, rightest;
		bool firstValueAssigned = false;

		private void Awake()
		{
			progHandler = FindObjectOfType<ProgressHandler>();
			selTracker = FindObjectOfType<PinSelectionTracker>();
		}

		private void OnEnable() 
		{
			if(selTracker != null) selTracker.onChangeBiome += StartPositionCenterPoint;
		}

		void Start()
		{
			StartCoroutine(FetchCurrentBiome());
			StartCoroutine(PositionCenterPoint(currentBiome));
		}

		private IEnumerator FetchCurrentBiome()
		{
			yield return new WaitForSeconds(.1f); //To avoid race condition
			GameObject selected = EventSystem.current.currentSelectedGameObject;
			currentBiome = selected.GetComponentInParent<LevelPinUI>().levelPin.biome;
		}

		private void StartPositionCenterPoint(Biomes biome)
		{
			firstValueAssigned = false;
			StartCoroutine(PositionCenterPoint(biome));
		}

		private IEnumerator PositionCenterPoint(Biomes biome)
		{
			yield return new WaitForSeconds(.1f); //To avoid race condition
			float xPos = FindXPos(biome);
			float yPos = FindYPos(biome);
			float zPos = FindZPos();
			transform.position = new Vector3(xPos, yPos, zPos);
		}

		private float FindXPos(Biomes biome)
		{
			FindEdgePins(biome);

			float xPos = leftest + (rightest - leftest) / 2;
			return xPos;
		}

		private void FindEdgePins(Biomes biome)
		{
			foreach (LevelPin pin in progHandler.levelPinList)
			{
				if (pin.biome != biome) continue;

				if (!firstValueAssigned)
				{
					leftest = pin.transform.position.x;
					rightest = pin.transform.position.x;

					firstValueAssigned = true;
				}

				if (pin.transform.position.x < leftest) leftest = pin.transform.position.x;
				if (pin.transform.position.x > rightest) rightest = pin.transform.position.x;
			}
		}

		private float FindYPos(Biomes biome)
		{
			foreach (LevelPin pin in progHandler.levelPinList)
			{
				if (pin.biome != biome) continue;

				return pin.unlockedYPos;
			}

			Debug.LogError("Couldn't find unlockedYPos of correct biome");
			return 0;
		}

		private float FindZPos()
		{
			GameObject selected = EventSystem.current.currentSelectedGameObject;
			Vector3 selPos = selected.GetComponentInParent<LevelPinUI>().
				levelPin.pathPoint.transform.position;

			float zPos = selPos.z;
			return zPos;
		}

		private void OnDisable()
		{
			if (selTracker != null) selTracker.onChangeBiome -= StartPositionCenterPoint;
		}
	}
}
