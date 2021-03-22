using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class LevelPinInitiator : MonoBehaviour
	{
		//Actions, events, delegates etc
		public event Action onPinInitation;

		private void Start()
		{
			onPinInitation();
		}
	}
}
