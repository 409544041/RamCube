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
		[SerializeField] OverlayButtonHandler buttonHandler;
		[SerializeField] string[] valueTexts;

		//States
		public DisplayTypes currentDisplay { get; set; }

		public void SetDisplayValueText()
		{
			if (Screen.fullScreenMode == FullScreenMode.Windowed)
			{
				buttonHandler.valueText.text = valueTexts[0];
				currentDisplay = DisplayTypes.windowed;
			}
			else if (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
			{
				buttonHandler.valueText.text = valueTexts[1];
				currentDisplay = DisplayTypes.full;
			}
		}

		public void SwapDisplay() //Called from button event
		{
			if (currentDisplay == DisplayTypes.full)
			{
				buttonHandler.valueText.text = valueTexts[0];
				currentDisplay = DisplayTypes.windowed;
			}
			else if (currentDisplay == DisplayTypes.windowed)
			{
				buttonHandler.valueText.text = valueTexts[1];
				currentDisplay = DisplayTypes.full;
			}

			SetDisplay();
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
	}
}
