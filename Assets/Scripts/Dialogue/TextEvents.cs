using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Qbism.Dialogue
{
	public class TextEvents : MonoBehaviour
	{
		//Config parameters
		[SerializeField] TextAnimator textAnim;
		[SerializeField] TextMeshProUGUI text;
		[SerializeField] float whisperFontSize;

		//States
		float originalFontSize;

		private void OnEnable()
		{
			textAnim.onEvent += OnEvent;
		}

		private void Start()
		{
			originalFontSize = text.fontSize;	
		}

		void OnEvent(string message)
		{
			if (message == "whisper") text.fontSize = whisperFontSize;
			if (message == "normal") text.fontSize = originalFontSize;
		}

		private void OnDisable()
		{
			textAnim.onEvent -= OnEvent;
		}
	}
}
