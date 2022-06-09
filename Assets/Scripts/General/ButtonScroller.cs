using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Qbism.General
{
	public class ButtonScroller : MonoBehaviour
	{
		//Config parameters
		public OverlayButtonHandler buttonHandler;
		public string[] valueTexts;
		[SerializeField] float scrollDelay = .5f;

		//States
		public string currentValueText { get; set; }
		public int currentValueIndex;
		float scrollDelayTime = 0;
		bool scrollPaused = false;

		//Actions, events, delegates etc
		public event Action<string> onScroll;

		private void Start()
		{
			for (int i = 0; i < valueTexts.Length; i++)
			{
				if (valueTexts[i] == currentValueText) currentValueIndex = i;
			}
		}

		private void Update()
		{
			if (scrollPaused)
			{
				scrollDelayTime += Time.deltaTime;
				if (scrollDelayTime >= scrollDelay)
				{
					scrollPaused = false;
					scrollDelayTime = 0;
				}
			}
		}

		public void Scroll(int scrollValue)
		{
			if (scrollPaused) return;
			scrollPaused = true;

			var i = currentValueIndex + scrollValue;
			if (i >= valueTexts.Length)
			{
				currentValueIndex = 0;
				currentValueText = valueTexts[0];
			}
			else if (i < 0)
			{
				currentValueIndex = valueTexts.Length - 1;
				currentValueText = valueTexts[valueTexts.Length - 1];
			}
			else
			{
				currentValueIndex = i;
				currentValueText = valueTexts[i];
			}

			buttonHandler.valueText.text = currentValueText;
			onScroll(currentValueText);
		}
	}
}
