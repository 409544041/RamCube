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
		[SerializeField] CanvasGroup badgeCanvasGroup;
		[SerializeField] MMFeedbacks pulseJuice, badgePulseJuice;
		[SerializeField] MapLogicRefHolder mlRef;
		
		private void Start()
		{
			canvasGroup.alpha = 0;
			badgeCanvasGroup.alpha = 0;
			StartCoroutine(ShowButton());
		}

		private IEnumerator ShowButton()
		{
			yield return null; //To ensure 'justCompleted' is set before this runs
			if (E_SegmentsGameplayData.GetEntity(0).f_Rescued &&
				!mlRef.levelPins[0].pinPather.justCompleted)
			{
				canvasGroup.alpha = 1;
				bool showBadge = CheckForUnreturnedObjects();
				if (showBadge) ShowBadge();
			}
		}

		private bool CheckForUnreturnedObjects()
		{
			var foundEntities = E_ObjectsGameplayData.FindEntities(entity =>
				entity.f_ObjectFound == true);

			foreach (var entity in foundEntities)
			{
				if (entity.f_ObjectReturned == false) return true;
			}

			return false;
		}

		private void ShowBadge()
		{
			badgeCanvasGroup.alpha = 1;
			badgePulseJuice.Initialization();
			badgePulseJuice.PlayFeedbacks();
		}

		public void PopInButtonForFirstTime()
		{
			pulseJuice.Initialization();
			pulseJuice.PlayFeedbacks();
			canvasGroup.alpha = 1;
		}
	}
}
