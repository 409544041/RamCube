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
		[SerializeField] MapCoreRefHolder mcRef;

		//Actions, events, delegates etc
		public Func<LevelPinRefHolder> onSavedPinFetch;
		public Func<E_Biome> onSavedBiomeFetch;

		public void StartPositionCenterPoint(E_Biome biome, LevelPinRefHolder selPin, bool onMapLoad,
			bool specificPos, bool checkMinMax, Vector2 pos)
		{
			PositionCenterPoint(selPin, onMapLoad, specificPos, checkMinMax, pos);
		}

		public void PositionCenterPointOnMapLoad()
		{
			var selPin = onSavedPinFetch();
			PositionCenterPoint(selPin, true, false, true, new Vector2(0, 0));
		}

		private void PositionCenterPoint(LevelPinRefHolder selPin, bool onMapLoad, 
			bool specificPos, bool checkMinMax, Vector2 pos)
		{
			Vector3 camToPointDiff = new Vector3(0, 0, 0);

			if (onMapLoad)
			{
				mcRef.mapCam.enabled = false;
				mcRef.camBrain.enabled = false;
				camToPointDiff = mcRef.mapCam.transform.position - transform.position;
			}

			float xPos = 0;
			float zPos = 0;

			if (!specificPos) FindPos(selPin, out xPos, out zPos);

			else if (specificPos && checkMinMax) 
				ComparePosToMinMaxValues(out xPos, out zPos, pos.x, pos.y);
				
			else if (specificPos && !checkMinMax)
			{
				xPos = pos.x;
				zPos = pos.y;
			}

			transform.position = new Vector3(xPos, 0, zPos);

			if (onMapLoad)
			{
				mcRef.mapCam.transform.position = transform.position +
					camToPointDiff;
				mcRef.mapCam.enabled = true;
				mcRef.camBrain.enabled = true;
			}
		}

		private void FindPos(LevelPinRefHolder selPin, out float xPos, out float zPos)
		{
			Vector3 selPos = selPin.pathPoint.transform.position;

			var selPosX = selPos.x;
			var selPosZ = selPos.z;

			ComparePosToMinMaxValues(out xPos, out zPos, selPosX, selPosZ);
		}

		private void ComparePosToMinMaxValues(out float xPos, out float zPos, float currentPosX, float currentPosZ)
		{
			float minX, maxX, minZ, maxZ;
			FetchMinMaxValues(out minX, out maxX, out minZ, out maxZ);

			if (currentPosX <= minX) xPos = minX;
			else if (currentPosX >= maxX) xPos = maxX;
			else xPos = currentPosX;

			if (currentPosZ <= minZ) zPos = minZ;
			else if (currentPosZ >= maxZ) zPos = maxZ;
			else zPos = currentPosZ;
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
