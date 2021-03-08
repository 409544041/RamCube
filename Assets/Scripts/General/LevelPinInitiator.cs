using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using UnityEngine;

namespace Qbism.General
{
	public class LevelPinInitiator : MonoBehaviour
	{
		void Start()
		{
			FindObjectOfType<ProgressHandler>().InitiatePins();
		}
	}
}
