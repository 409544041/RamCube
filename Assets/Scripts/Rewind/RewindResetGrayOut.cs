using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Qbism.Rewind
{
	public class RewindResetGrayOut : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float brightnessDelta = .3f, saturationDelta = .2f, alphaDelta = .5f;
		[SerializeField] Image[] elementImages;
		[SerializeField] TextMeshProUGUI[] texts;
		[SerializeField] GameplayCoreRefHolder gcRef;

		//States
		List<Color> grayedOutColors = new List<Color>();
		List<Color> originalColors = new List<Color>();
		bool rewindAllowed, prevFrameRewindAllowed;

		private void Awake()
		{
			for (int i = 0; i < elementImages.Length; i++)
			{
				originalColors.Add(elementImages[i].color);
			}

			for (int i = 0; i < texts.Length; i++)
			{
				originalColors.Add(texts[i].color);

			}

			for (int i = 0; i < elementImages.Length; i++)
			{
				var grayed = SetGrayedOutColors(elementImages[i].color);
				grayedOutColors.Add(grayed);
			}

			for (int i = 0; i < texts.Length; i++)
			{
				var tmGrayed = SetGrayedOutColors(texts[i].color);
				grayedOutColors.Add(tmGrayed);
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

		private void Update()
		{
			prevFrameRewindAllowed = rewindAllowed;
			rewindAllowed = gcRef.pRef.playerMover.allowRewind;

			if (!rewindAllowed && rewindAllowed != prevFrameRewindAllowed)
				GrayOutButtons();
			if (rewindAllowed && rewindAllowed != prevFrameRewindAllowed)
				ReturnToOriginalColors();
		}

		private void GrayOutButtons()
		{
			for (int i = 0; i < elementImages.Length; i++)
			{
				elementImages[i].color = grayedOutColors[i];
			}

			for (int i = 0; i < texts.Length; i++)
			{
				texts[i].color = grayedOutColors[grayedOutColors.Count - 1];
			}
		}

		private void ReturnToOriginalColors()
		{
			for (int i = 0; i < elementImages.Length; i++)
			{
				elementImages[i].color = originalColors[i];
			}

			for (int i = 0; i < texts.Length; i++)
			{
				texts[i].color = originalColors[originalColors.Count - 1];
			}
		}
	}
}
