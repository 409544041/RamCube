using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Qbism.Settings
{
	public class VolumeSetter : MonoBehaviour
	{
		[SerializeField] AudioMixer audioMixer;

		public void ChangeMusicVolume(Slider slider) //Called from slider
		{
			audioMixer.SetFloat("musicVolume", Mathf.Log10(slider.value) * 20);
		}

		public void ChangeSoundVolume(Slider slider) //Called from slider
		{
			audioMixer.SetFloat("sfxVolume", Mathf.Log10(slider.value) * 20);
		}
	}
}
