using Qbism.Serpent;
using Qbism.WorldMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class CheckHud : MonoBehaviour
	{
		//Config parameters
		[SerializeField] CanvasGroup canvasGroup;
		[SerializeField] GameplayCoreRefHolder gcRef;
		[SerializeField] MapCoreRefHolder mcRef;
		[SerializeField] SerpCoreRefHolder scRef;

		private void Awake()
		{
			bool hudVisible = true;
			if (gcRef != null) hudVisible = gcRef.persRef.hudToggler.hudVisible;
			if (mcRef != null) hudVisible = mcRef.persRef.hudToggler.hudVisible;
			if (scRef != null) hudVisible = scRef.persRef.hudToggler.hudVisible;

			if (hudVisible) canvasGroup.alpha = 1;
			else canvasGroup.alpha = 0;
		}
	}
}
