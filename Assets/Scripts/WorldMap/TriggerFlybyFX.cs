using System.Collections;
using System.Collections.Generic;
using Qbism.Serpent;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class TriggerFlybyFX : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other) 
		{
			var headAnimator = other.GetComponentInChildren<MotherDragonAnimator>();
			var segAnimator = other.GetComponentInChildren<SegmentAnimator>();

			if (headAnimator) headAnimator.ActivateFlyByJuice();
			else if (segAnimator) segAnimator.ActivateFlyByJuice();

		}
	}
}
