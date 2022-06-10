using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Objects
{
	public class ObjectRenderSelector : MonoBehaviour
	{
		//Config parameters
		[SerializeField] SegObjectRefHolder[] objectsToRender;

		public void ShowCorrespondingObject(E_Objects objToRender)
		{
			foreach (var obj in objectsToRender)
			{
				if (objToRender.f_name == obj.m_objects.f_name)
				{
					obj.spawnJuice.Initialization();
					obj.spawnJuice.PlayFeedbacks();
					obj.juicer.TriggerOnlyRotation();

					foreach (var mesh in obj.meshes)
					{
						mesh.enabled = true;
					}
				}
				else if (obj.gameObject.activeSelf == true)
				{
					foreach (var mesh in obj.meshes)
					{
						mesh.enabled = false;
					}
				}
			}
		}
	}
}
