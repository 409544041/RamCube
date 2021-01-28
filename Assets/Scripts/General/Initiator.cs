using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using UnityEngine;
using UnityEngine.Events;

namespace Qbism.General
{
	public class Initiator : MonoBehaviour
	{
		void Start()
		{
			FindObjectOfType<ProgressHandler>().InitiatePins();
		}
	}
}
