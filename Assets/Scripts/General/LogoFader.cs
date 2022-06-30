using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class LogoFader : MonoBehaviour
	{
		//Config parameters
		[SerializeField] CanvasGroup logoGroup, devLogoGroup, pubLogoGroup;
		[SerializeField] float logoDelay, logoVisible, logoInterval, logoFadeTime;
		[SerializeField] SplashMenuHandler splashMenu;
		[SerializeField] FeatureSwitchBoard switchBoard;

		private void Start()
		{
			if (!switchBoard.showLogos) return;
			StartCoroutine(ShowLogos());
		}

		private IEnumerator ShowLogos()
		{
			logoGroup.alpha = 1;

			yield return new WaitForSeconds(logoDelay);
			yield return FadeLogo(0, 1, pubLogoGroup);
			yield return new WaitForSeconds(logoVisible);
			yield return FadeLogo(1, 0, pubLogoGroup);
			yield return new WaitForSeconds(logoInterval);
			yield return FadeLogo(0, 1, devLogoGroup);
			yield return new WaitForSeconds(logoVisible);
			yield return FadeLogo(1, 0, devLogoGroup);
			yield return FadeLogo(1, 0, logoGroup);
			splashMenu.ActivateMenu();
		}

		private IEnumerator FadeLogo(float start, float target, CanvasGroup group)
		{
			float elapsedTime = 0;

			while (!Mathf.Approximately(group.alpha, target))
			{
				elapsedTime += Time.deltaTime;
				var percentComplete = elapsedTime / logoFadeTime;
				group.alpha = Mathf.Lerp(start, target, percentComplete);
				yield return null;
			}
		}
	}
}
