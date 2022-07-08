using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class DisableAtEndCam : MonoBehaviour
	{
		public void DisableMeshes()
		{
			Renderer[] renderers = GetComponentsInChildren<Renderer>();

			if (renderers.Length > 0)
			{
				foreach (var mRender in renderers)
				{
					if (mRender.enabled == true) mRender.enabled = false;
				}
			}
		}
	}
}
