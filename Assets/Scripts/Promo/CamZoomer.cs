using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Qbism.Promo
{
	public class CamZoomer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] CinemachineVirtualCamera cam;
		[SerializeField] float targetZoom, zoomSpeed;
		[SerializeField] bool pulse;

		//States
		int mult = 1;
		bool zooming = true;
		float orthoSize = 0;

		private void Start() 
		{
			orthoSize = cam.m_Lens.OrthographicSize;
			if (orthoSize > targetZoom) mult = -1;
		}

		private void Update() 
		{
			if (!pulse) Zoom();
			if (pulse) PulseZoom();				
		}

		private void Zoom()
		{
			if (zooming)
			{
				cam.m_Lens.OrthographicSize += zoomSpeed * mult * Time.deltaTime;

				if (mult == 1)
				{
					if (cam.m_Lens.OrthographicSize >= targetZoom) zooming = false;
				}
				else
				{
					if (cam.m_Lens.OrthographicSize <= targetZoom) zooming = false;
				}
			}
		}

		private void PulseZoom()
		{
			cam.m_Lens.OrthographicSize += zoomSpeed * mult * Time.deltaTime;

			if (mult == 1)
			{
				if (cam.m_Lens.OrthographicSize >= targetZoom) SwitchTarget();
			}
			else
			{
				if (cam.m_Lens.OrthographicSize <= targetZoom) SwitchTarget();
			}
		}

		private void SwitchTarget()
		{
			var target = orthoSize;
			orthoSize = targetZoom;
			targetZoom = target;
			mult *= -1;
		}

		private void OnDisable() 
		{
			cam.m_Lens.OrthographicSize = orthoSize;
		}
	}
}
