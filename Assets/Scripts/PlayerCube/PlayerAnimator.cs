using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerAnimator : MonoBehaviour
	{
		//Actions, events, delegates etc
		public event Action onActivateSegmentSquish;

		private void ActivateSegmentSquish()
		{
			onActivateSegmentSquish();
		}
	}
}
