using BansheeGz.BGDatabase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Settings
{
	public class LanguageSetter : MonoBehaviour
	{
		//Config parameters
		public Languages activeLanguage;

		//States
		Languages prevLanguage;
		Languages currentLanguage;

		private void Start()
		{
			currentLanguage = activeLanguage;
			BGAddonLocalization.DefaultRepoCurrentLocale = activeLanguage.ToString();
		}

		private void Update()
		{
			prevLanguage = currentLanguage;
			currentLanguage = activeLanguage;

			if (currentLanguage != prevLanguage)
				BGAddonLocalization.DefaultRepoCurrentLocale = activeLanguage.ToString();
		}
	}
}
