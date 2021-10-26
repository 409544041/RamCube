using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class DisableAtEndCam : MonoBehaviour
	{
		public void DisableMeshes()
		{
			MeshRenderer[] mRenders = GetComponentsInChildren<MeshRenderer>();

			if (mRenders.Length > 0)
			{
				foreach (var mRender in mRenders)
				{
					if (mRender.enabled == true) mRender.enabled = false;
				}
			}
		}
	}
}
