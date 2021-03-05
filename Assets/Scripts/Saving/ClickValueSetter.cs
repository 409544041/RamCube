using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Saving
{
	public class ClickValueSetter : MonoBehaviour
	{
		//Actions, events, delegates etc
		public event Action<bool> onSetClickValue;

		public void SetClickValue(bool value)
		{
			onSetClickValue(value);
		}
	}
}
