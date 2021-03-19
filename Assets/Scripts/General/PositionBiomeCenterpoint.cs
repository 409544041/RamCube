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
		//Config Parameters
		[SerializeField] BiomeCenterLimits centerLimits;
		//Cache
		ProgressHandler progHandler = null;
		PinSelectionTracker selTracker = null;

		//States
		Biomes currentBiome;
		Biomes prevBiome;
		float leftest, rightest;
		bool firstValueAssigned = false;

		private void Awake()
		{
			progHandler = FindObjectOfType<ProgressHandler>();
			selTracker = FindObjectOfType<PinSelectionTracker>();
		}

		private void OnEnable() 
		{
			if(selTracker != null)
			{
				selTracker.onSetCenterPos += StartPositionCenterPoint;
			} 
		}

		void Start()
		{
			GameObject selected = EventSystem.current.currentSelectedGameObject;
			LevelPin selPin = selected.GetComponentInParent<LevelPinUI>().levelPin;

			StartCoroutine(FetchCurrentBiome());
			StartCoroutine(PositionCenterPoint(currentBiome, selPin));
		}

		private IEnumerator FetchCurrentBiome()
		{
			yield return new WaitForSeconds(.1f); //To avoid race condition
			GameObject selected = EventSystem.current.currentSelectedGameObject;
			currentBiome = selected.GetComponentInParent<LevelPinUI>().levelPin.biome;
		}

		private void StartPositionCenterPoint(Biomes biome, LevelPin selPin)
		{
			prevBiome = currentBiome;
			currentBiome = biome;
			StartCoroutine(PositionCenterPoint(biome, selPin));
		}

		private IEnumerator PositionCenterPoint(Biomes biome, LevelPin selPin)
		{
			yield return new WaitForSeconds(.1f); //To avoid race condition

			float xPos;
			if(currentBiome != prevBiome) xPos = FindXPos(biome);
			else xPos = transform.position.x;

			float yPos = selPin.unlockedYPos;

			float zPos = FindZPos(biome, selPin);

			transform.position = new Vector3(xPos, yPos, zPos);
		}

		private float FindXPos(Biomes biome)
		{
			firstValueAssigned = false;
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

		private float FindZPos(Biomes biome, LevelPin selPin)
		{
			Vector3 selPos = selPin.pathPoint.transform.position;
			float zPos = selPos.z;

			foreach (BiomeCenterLimits.BiomeLimit limit in centerLimits.biomeLimits)
			{
				if(limit.biome != biome) continue;

				if(zPos <= limit.minZ) zPos = limit.minZ;
				if(zPos >= limit.maxZ) zPos = limit.maxZ;
			}

			return zPos;
		}

		private void OnDisable()
		{
			if (selTracker != null)
			{
				selTracker.onSetCenterPos -= StartPositionCenterPoint;
			}
		}
	}
}
