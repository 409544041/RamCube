using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using Qbism.WorldMap;
using UnityEngine;

namespace Qbism.General
{
	public class PositionBiomeCenterpoint : MonoBehaviour
	{
		//Cache
		ProgressHandler progHandler = null;
		PinSelectionTracker selTracker = null;

		//States
		E_Biome currentBiome;
		E_Biome prevBiome;
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

		private void StartPositionCenterPoint(E_Biome biome, LevelPin selPin)
		{
			prevBiome = currentBiome;
			currentBiome = biome;
			StartCoroutine(PositionCenterPoint(biome, selPin));
		}

		private IEnumerator PositionCenterPoint(E_Biome biome, LevelPin selPin)
		{
			yield return new WaitForSeconds(.1f); //To avoid race condition

			float xPos;
			if(currentBiome != prevBiome) xPos = FindXPos(biome);
			else xPos = transform.position.x;

			float yPos = selPin.pinRaiser.unlockedYPos;
			float zPos = FindZPos(biome, selPin);

			transform.position = new Vector3(xPos, yPos, zPos);
		}

		private float FindXPos(E_Biome biome)
		{
			firstValueAssigned = false;
			FindEdgePins(biome);

			float xPos = leftest + (rightest - leftest) / 2;
			return xPos;
		}

		private void FindEdgePins(E_Biome biome)
		{
			foreach (LevelPin pin in progHandler.FetchLevelPins())
			{
				if (pin.m_Pin.f_Biome != biome) continue;

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

		private float FindZPos(E_Biome biome, LevelPin selPin)
		{
			Vector3 selPos = selPin.pinPather.pathPoint.transform.position;
			float zPos = selPos.z;

			if(zPos <= biome.f_MinZ) zPos = biome.f_MinZ;
			if(zPos >= biome.f_MaxZ) zPos = biome.f_MaxZ;

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
