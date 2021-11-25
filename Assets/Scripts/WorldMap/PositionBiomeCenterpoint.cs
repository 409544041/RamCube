﻿using System;
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
			StartCoroutine(PositionCenterPoint(selPin, onMapLoad));
		}

		public Coroutine PositionCenterPointOnMapLoad()
		{
			var selPin = onSavedPinFetch();
			return StartCoroutine(PositionCenterPoint(selPin, true));
		}

		private IEnumerator PositionCenterPoint(LevelPin selPin, bool onMapLoad)
		{
			CinemachineVirtualCamera virtCam = null; 
			CinemachineBrain brain = null;
			Vector3 camToPointDiff = new Vector3(0, 0, 0);

			if (onMapLoad) StartCamHardCut(out virtCam, out brain, out camToPointDiff);

			if (onMapLoad) yield return new WaitForSeconds(.1f); //To avoid race condition

			float xPos;
			float zPos;
			FindPos(selPin, out xPos, out zPos);

			float yPos = selPin.pinRaiser.unlockedYPos;
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

		private void FindPos(LevelPin selPin, out float xPos, out float zPos)
		{
			Vector3 selPos = selPin.pinPather.pathPoint.transform.position;

			var selPosX = selPos.x;
			var selPosZ = selPos.z;

			float minX, maxX, minZ, maxZ;
			FetchMinMaxValues(out minX, out maxX, out minZ, out maxZ);

			if(selPosX <= minX) xPos = minX;
			else if(selPosX >= maxX) xPos = maxX;
			else xPos = selPosX;

			if(selPosZ <= minZ) zPos = minZ;
			else if(selPosZ >= maxZ) zPos = maxZ;
			else zPos = selPosZ;
		}

		private void FetchMinMaxValues(out float minX, out float maxX, 
			out float minZ, out float maxZ)
		{
			bool firstValueAssigned = false;

			float tempMinX = 0, tempMaxX = 0, tempMinZ = 0, tempMaxZ = 0;

			for (int i = 0; i < E_Biome.CountEntities; i++)
			{
				var biomeEnt = E_Biome.GetEntity(i);
				var biomeGameplayEnt = E_BiomeGameplayData.GetEntity(i);
				
				if (!biomeGameplayEnt.f_Unlocked) continue;

				if (!firstValueAssigned)
				{
					tempMinX = biomeEnt.f_MinMaxX.x;
					tempMaxX = biomeEnt.f_MinMaxX.y;
					tempMinZ = biomeEnt.f_MinMaxZ.x;
					tempMaxZ = biomeEnt.f_MinMaxZ.y;

					firstValueAssigned = true;
				}

				if (biomeEnt.f_MinMaxX.x < tempMinX) tempMinX = biomeEnt.f_MinMaxX.x;
				if (biomeEnt.f_MinMaxX.y > tempMaxX) tempMaxX = biomeEnt.f_MinMaxX.y;
				if (biomeEnt.f_MinMaxZ.x < tempMinZ) tempMinZ = biomeEnt.f_MinMaxZ.x;
				if (biomeEnt.f_MinMaxZ.y > tempMaxZ) tempMaxZ = biomeEnt.f_MinMaxZ.y;
			}

			minX = tempMinX; maxX = tempMaxX; minZ = tempMinZ; maxZ = tempMaxZ;
		}
	}
}
