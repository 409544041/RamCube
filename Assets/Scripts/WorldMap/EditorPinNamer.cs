﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.WorldMap
{
	[ExecuteInEditMode]
	public class EditorPinNamer : MonoBehaviour
	{
		void Start()
		{
			string pinID = GetComponent<EditorSetPinValues>().levelID.ToString();
			transform.gameObject.name = "pin " + pinID;
		}
	}
}

