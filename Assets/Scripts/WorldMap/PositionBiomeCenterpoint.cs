using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Qbism.Saving;
using UnityEngine;

namespace Qbism.WorldMap
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

		//Actions, events, delegates etc
		public Func<LevelPin> onSavedPinFetch;
		public Func<E_Biome> onSavedBiomeFetch;

		private void Awake()
		{
			progHandler = FindObjectOfType<ProgressHandler>();
			selTracker = FindObjectOfType<PinSelectionTracker>();
		}

		public void StartPositionCenterPoint(E_Biome biome, LevelPin selPin, bool onMapLoad)
		{
			prevBiome = currentBiome;
			currentBiome = biome;
			StartCoroutine(PositionCenterPoint(biome, selPin, onMapLoad));
		}

		public Coroutine PositionCenterPointOnMapLoad()
		{
			var biome = onSavedBiomeFetch();
			var selPin = onSavedPinFetch();
			return StartCoroutine(PositionCenterPoint(biome, selPin, true));
		}

		private IEnumerator PositionCenterPoint(E_Biome biome, LevelPin selPin, bool onMapLoad)
		{
			CinemachineVirtualCamera virtCam = null; 
			CinemachineBrain brain = null;
			Vector3 camToPointDiff = new Vector3(0, 0, 0);

			if (onMapLoad) StartCamHardCut(out virtCam, out brain, out camToPointDiff);

			if (onMapLoad) yield return new WaitForSeconds(.1f); //To avoid race condition

			float xPos;
			if(currentBiome != prevBiome || onMapLoad) xPos = FindXPos(biome);
			else xPos = transform.position.x;

			float yPos = selPin.pinRaiser.unlockedYPos;
			float zPos = FindZPos(biome, selPin);
			transform.position = new Vector3(xPos, yPos, zPos);

			if (onMapLoad) FinishCamHardCut(virtCam, brain, camToPointDiff);
		}

		private void StartCamHardCut(out CinemachineVirtualCamera virtCam, out CinemachineBrain brain, out Vector3 camToPointDiff)
		{
			virtCam = GameObject.FindGameObjectWithTag("MapCam").
			GetComponent<CinemachineVirtualCamera>();
			brain = Camera.main.GetComponent<CinemachineBrain>();

			virtCam.enabled = false;
			brain.enabled = false;

			camToPointDiff = virtCam.transform.position - transform.position;
		}

		private void FinishCamHardCut(CinemachineVirtualCamera virtCam, CinemachineBrain brain, Vector3 camToPointDiff)
		{
			virtCam.transform.position = transform.position +
			camToPointDiff;

			virtCam.enabled = true;
			brain.enabled = true;
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
	}
}
