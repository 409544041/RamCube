using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	[ExecuteInEditMode]
	public class EditorFaceCamera : MonoBehaviour
	{
		void Start()
		{
			transform.forward = FindObjectOfType<Camera>().transform.forward;
		}
	}
}

