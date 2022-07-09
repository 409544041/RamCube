using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Qbism.General
{
	public class TextField : MonoBehaviour
	{
		//Config parameters
		[SerializeField] TextMeshProUGUI tmProText;
		[SerializeField] string actualText;

		private void Awake()
		{
			tmProText.text = actualText;
		}
	}
}
