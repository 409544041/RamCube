using System.Collections;
using System.Collections.Generic;
using Qbism.Rewind;
using UnityEngine;
using UnityEngine.UI;

namespace Qbism.UI
{
	public class RewindDisplay : MonoBehaviour
	{
		//Cache
		RewindHandler rewinder;
		Text rewindText;

		private void Awake() 
		{
			rewinder = FindObjectOfType<RewindHandler>();
			rewindText = GetComponent<Text>();
		}

		private void Update() 
		{
			rewindText.text = rewinder.rewindsAmount.ToString() + " undo's left";	
		}
	}
}
