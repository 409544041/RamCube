using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Qbism.General
{
	public class GreyOutButtons : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float brightnessDelta = .3f, saturationDelta = .2f, alphaDelta = .5f;
		[SerializeField] Image[] elementImages;
		[SerializeField] TextMeshProUGUI[] texts;

		//States
		List<Color> grayedButtonColors = new List<Color>();
		List<Color> grayedTextColors = new List<Color>();
		List<Color> originalButtonColors = new List<Color>();
		List<Color> originalTextColors = new List<Color>();

		private void Start()
		{
			AddColorsToLists();
		}

		private void AddColorsToLists()
		{
			for (int i = 0; i < elementImages.Length; i++)
			{
				originalButtonColors.Add(elementImages[i].color);
			}

			for (int i = 0; i < texts.Length; i++)
			{
				originalTextColors.Add(texts[i].color);
			}

			for (int i = 0; i < elementImages.Length; i++)
			{
				var grayed = SetGrayedOutColors(elementImages[i].color);
				grayedButtonColors.Add(grayed);
			}

			for (int i = 0; i < texts.Length; i++)
			{
				var tmGrayed = SetGrayedOutColors(texts[i].color);
				grayedTextColors.Add(tmGrayed);
			}
		}

		private Color SetGrayedOutColors(Color color)
		{
			float baseHue; float baseSat; float baseBright; ;
			Color.RGBToHSV(color, out baseHue, out baseSat, out baseBright);
			baseSat -= saturationDelta;
			baseBright -= brightnessDelta;
			var newColor = Color.HSVToRGB(baseHue, baseSat, baseBright);
			newColor.a -= alphaDelta;
			return newColor;
		}

		public void GrayOutButton()
		{
			for (int i = 0; i < elementImages.Length; i++)
			{
				elementImages[i].color = grayedButtonColors[i];
			}

			for (int i = 0; i < texts.Length; i++)
			{
				texts[i].color = grayedTextColors[i];
			}
		}

		public void ReturnToOriginalColors()
		{
			for (int i = 0; i < elementImages.Length; i++)
			{
				elementImages[i].color = originalButtonColors[i];
			}

			for (int i = 0; i < texts.Length; i++)
			{
				texts[i].color = originalTextColors[i];
			}
		}
	}
}
