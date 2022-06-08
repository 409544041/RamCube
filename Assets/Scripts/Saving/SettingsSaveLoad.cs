using Qbism.General;
using Qbism.Saving;
using Qbism.Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Qbism.Saving
{
	public class SettingsSaveLoad : MonoBehaviour
	{
		//Config parameters
		[SerializeField] AudioMixer audioMixer;

		//States
		public SettingsValueData settingsData { get; private set; }

		private void Awake()
		{
			BuildNewData();
			LoadSettingsData();
		}

		private void BuildNewData()
		{
			settingsData = new SettingsValueData();
			settingsData.musicSliderValue = 1;
			settingsData.sfxSliderValue = 1;
			settingsData.display = "windowed";
		}

		private void LoadSettingsData()
		{
			ProgData data = SavingSystem.LoadProgData();

			if (data != null) settingsData = data.savedSettingsData;
		}

		public void AssignLoadedSettingsValues(Slider musicSlider, Slider sfxSlider,
			OverlayButtonHandler displayButton)
		{
			musicSlider.value = settingsData.musicSliderValue;
			sfxSlider.value = settingsData.sfxSliderValue;
			audioMixer.SetFloat("musicVolume", Mathf.Log10(settingsData.musicSliderValue) * 20);
			audioMixer.SetFloat("sfxVolume", Mathf.Log10(settingsData.sfxSliderValue) * 20);

			displayButton.valueText.text = settingsData.display;
			DisplayTypes displayEnum = (DisplayTypes)System.Enum.Parse(typeof(DisplayTypes),
				settingsData.display);
			displayButton.GetComponent<DisplaySwapper>().currentDisplay = displayEnum;
		}

		public void SaveSettingsValues(Slider musicSlider, Slider sfxSlider,
			OverlayButtonHandler displayButton)
		{
			settingsData.musicSliderValue = musicSlider.value;
			settingsData.sfxSliderValue = sfxSlider.value;
			settingsData.display = 
				displayButton.GetComponent<DisplaySwapper>().currentDisplay.ToString();
		}
	}
}
