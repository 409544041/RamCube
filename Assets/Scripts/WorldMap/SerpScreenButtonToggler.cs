using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class SerpScreenButtonToggler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] CanvasGroup canvasGroup;

		private void Start()
		{
			if (E_SegmentsGameplayData.GetEntity(0).f_Rescued)
				canvasGroup.alpha = 1;
			else canvasGroup.alpha = 0;
		}
	}
}
