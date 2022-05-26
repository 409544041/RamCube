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

		//Cache
		SerpCoreRefHolder scRef;

		//States
		Color elementOriginalColor, iconOriginalColor;
		Vector3[] elementOriginalPos;

		private void Awake()
		{
			scRef = slRef.scRef;
			elementOriginalPos = new Vector3[scRef.objSlotElements.Length];
		}
		private void Start()
		{
			elementOriginalColor = speechElement.color;
			iconOriginalColor = speechIcon.color;

			for (int i = 0; i < scRef.objSlotElements.Length; i++)
			{
				var slotElement = scRef.objSlotElements[i];

				elementOriginalPos[i] = slotElement.rectTransform.anchoredPosition;
			}
		}

		public void ShowObjectUI(List<E_Objects> e_objs)
		{
			foreach (var slot in scRef.objSlotElements)
			{
				slot.gameObject.SetActive(false);
			}

			if (e_objs == null) return;

			bool obj1Return = slRef.objSegChecker.CheckIfObjectReturned(e_objs[0]);
			bool obj2Return = slRef.objSegChecker.CheckIfObjectReturned(e_objs[1]);

			if (!obj1Return && !obj2Return) return;

			if (obj1Return && !obj2Return)
			{
				slRef.objRenderSelectors[0].ShowCorrespondingObject(e_objs[0]);
				scRef.objSlotElements[0].rectTransform.anchoredPosition = 
					new Vector3(0, elementOriginalPos[0].y, elementOriginalPos[0].z);
				scRef.objSlotElements[0].gameObject.SetActive(true);
			}

			if (obj1Return && obj2Return)
			{
				slRef.objRenderSelectors[0].ShowCorrespondingObject(e_objs[0]);
				slRef.objRenderSelectors[1].ShowCorrespondingObject(e_objs[1]);
				scRef.objSlotElements[0].rectTransform.anchoredPosition = elementOriginalPos[0];
				scRef.objSlotElements[0].gameObject.SetActive(true);
				scRef.objSlotElements[1].gameObject.SetActive(true);
			}
		}

		public void SetSerpScreenAlpha(int value)
		{
			scRef.serpScreenCanvasGroup.alpha = value;
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
