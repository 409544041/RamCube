using Qbism.Serpent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Qbism.Objects
{
	public class SerpScreenUIHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Color elementFaded, iconFaded;
		[SerializeField] Image speechElement, speechIcon;
		[SerializeField] SerpLogicRefHolder slRef;

		//States
		Color elementOriginalColor, iconOriginalColor;

		private void Start()
		{
			elementOriginalColor = speechElement.color;
			iconOriginalColor = speechIcon.color;
		}

		public void ShowObjectUI(List<E_Objects> e_objs)
		{
			if (e_objs == null)
			{
				foreach (var selector in slRef.objRenderSelectors)
				{
					selector.HideObjects();
				}
				return;
			}

			for (int i = 0; i < e_objs.Count; i++)
			{
				ShowIndividualObjectUI(e_objs[i], i);
			}
		}

		private void ShowIndividualObjectUI(E_Objects e_obj, int i)
		{
			if (slRef.objSegChecker.CheckIfObjectReturned(e_obj))
				slRef.objRenderSelectors[i].ShowCorrespondingObject(e_obj);
			else slRef.objRenderSelectors[i].HideObjects();
		}

		public void SetSerpScreenAlpha(int value)
		{
			slRef.scRef.serpScreenCanvasGroup.alpha = value;
		}

		public void FadeOutSpeechElement()
		{
			speechElement.color = elementFaded;
			speechIcon.color = iconFaded;
		}

		public void ResetSpeechElementColor()
		{
			speechElement.color = elementOriginalColor;
			speechIcon.color = iconOriginalColor;
		}
	}
}
