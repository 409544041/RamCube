using Qbism.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Settings
{
	public class InitialDisplaySetter : MonoBehaviour
	{
		//Config parameters
		[SerializeField] SettingsSaveLoad settingsLoader;

		private void Start()
		{
			DisplayTypes displayEnum = (DisplayTypes)System.Enum.Parse(typeof(DisplayTypes),
				settingsLoader.settingsData.display);

			if (displayEnum == DisplayTypes.windowed &&
			Screen.fullScreenMode != FullScreenMode.Windowed)
				Screen.SetResolution(1366, 768, false);

			else if (displayEnum == DisplayTypes.full &&
				Screen.fullScreenMode != FullScreenMode.ExclusiveFullScreen)
				Screen.SetResolution(Screen.currentResolution.width,
					Screen.currentResolution.height, true);
		}
	}
}
