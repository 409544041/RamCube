using Qbism.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Qbism.General
{
	public class SettingsSaveLoad : MonoBehaviour
	{
		//Config parameters
		[SerializeField] AudioMixer audioMixer;
		[SerializeField] PersistentRefHolder persRef;

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
		}

		private void LoadSettingsData()
		{
			ProgData data = SavingSystem.LoadProgData();

			if (data != null) settingsData = data.savedSettingsData;
		}

		public void AssignLoadedSettingsValues(Slider musicSlider, Slider sfxSlider)
		{
			musicSlider.value = settingsData.musicSliderValue;
			sfxSlider.value = settingsData.sfxSliderValue;
			audioMixer.SetFloat("musicVolume", Mathf.Log10(settingsData.musicSliderValue) * 20);
			audioMixer.SetFloat("sfxVolume", Mathf.Log10(settingsData.sfxSliderValue) * 20);
		}

		public void SaveSettingsValues(Slider musicSlider, Slider sfxSlider)
		{
			settingsData.musicSliderValue = musicSlider.value;
			settingsData.sfxSliderValue = sfxSlider.value;
		}
	}
}
