using Qbism.General;
using Qbism.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Settings
{
	public class DisplaySwapper : MonoBehaviour
	{
		//Config parameters
		[SerializeField] ButtonScroller scroller;

		//States
		public DisplayTypes currentDisplay { get; set; }

		private void OnEnable()
		{
			if (scroller.buttonHandler.label == "display")
				scroller.onScroll += HandleScroll;
		}

		public void HandleScroll(string valueText)
		{
			if (currentDisplay == DisplayTypes.full)
				SetValues(DisplayTypes.windowed);
			else if (currentDisplay == DisplayTypes.windowed)
				SetValues(DisplayTypes.full);

			SetDisplay();
		}

		public void SetInitialValues()
		{
			if (Screen.fullScreenMode == FullScreenMode.Windowed)
				SetValues(DisplayTypes.windowed);
			else if (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
				SetValues(DisplayTypes.full);
		}

		private void SetValues(DisplayTypes displayType)
		{
			for (int i = 0; i < scroller.valueTexts.Length; i++)
			{
				if (scroller.valueTexts[i] != displayType.ToString()) continue;

				scroller.currentValueText = scroller.valueTexts[i];
				scroller.currentValueIndex = i;
			}

			currentDisplay = displayType;
		}

		private void SetDisplay()
		{
			if (currentDisplay == DisplayTypes.full && 
				Screen.fullScreenMode != FullScreenMode.ExclusiveFullScreen)
				Screen.SetResolution(Screen.currentResolution.width,
					Screen.currentResolution.height, true);

			else if (currentDisplay == DisplayTypes.windowed &&
				Screen.fullScreenMode != FullScreenMode.Windowed)
				Screen.SetResolution(1366, 768, false);
		}

		private void OnDisable()
		{
			if (scroller.buttonHandler.label == "display")
				scroller.onScroll -= HandleScroll;
		}
	}
}
