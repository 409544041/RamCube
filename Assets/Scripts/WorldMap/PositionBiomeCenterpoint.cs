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
		//Config parameters
		[SerializeField] MapCoreRefHolder mcRef;

		//States
		MapLogicRefHolder mlRef;
		Camera cam;
		float distToCam;
		public bool syncCenterToCursor { get; set; } = true;
		bool checkMinMaxAtSync = false;

		//Actions, events, delegates etc
		public Func<LevelPinRefHolder> onSavedPinFetch;
		public Func<E_Biome> onSavedBiomeFetch;

		private void Awake()
		{
			mlRef = mcRef.mlRef;
			cam = mcRef.cam;
		}

		private void Start()
		{
			distToCam = mcRef.mapCam.
				GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance;
		}

		private void Update()
		{
			if (syncCenterToCursor) PlaceCenterPointAtCursor();
		}

		public void SetMinMaxCheckAtSync(bool value)
		{
			checkMinMaxAtSync = value;
		}

		public void PlaceCenterPointAtCursor()
		{
			var pos = cam.ScreenToWorldPoint(mlRef.mapCursor.cursor.transform.position);
			transform.position = pos;
			transform.position += cam.transform.forward * distToCam;

			float xPos = transform.position.x, zPos = transform.position.z;
			if (checkMinMaxAtSync) ComparePosToMinMaxValues(out xPos, out zPos,
				transform.position.x, transform.position.z);
			transform.position = new Vector3(xPos, 0, zPos);
		}

		public void FindPos(LevelPinRefHolder selPin, out float xPos, out float zPos)
		{
			Vector3 selPos = selPin.pathPoint.transform.position;

			var selPosX = selPos.x;
			var selPosZ = selPos.z;

			ComparePosToMinMaxValues(out xPos, out zPos, selPosX, selPosZ);
		}

		public void ComparePosToMinMaxValues(out float xPos, out float zPos, float currentPosX, float currentPosZ)
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
