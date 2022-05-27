using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class SerpScreenButtonToggler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] CanvasGroup canvasGroup;
		[SerializeField] MapLogicRefHolder mlRef;
		[SerializeField] MMFeedbacks pulseJuice;

		private void Start()
		{
			canvasGroup.alpha = 0;
			StartCoroutine(ShowButton());
		}

		private IEnumerator ShowButton()
		{
			yield return null; //To ensure 'justCompleted' is set before this runs
			if (E_SegmentsGameplayData.GetEntity(0).f_Rescued &&
				!mlRef.levelPins[0].pinPather.justCompleted) canvasGroup.alpha = 1;
		}

		public void PopInButtonForFirstTime()
		{
			pulseJuice.Initialization();
			pulseJuice.PlayFeedbacks();
			canvasGroup.alpha = 1;
		}
	}
}
