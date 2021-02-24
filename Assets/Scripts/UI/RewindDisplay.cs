using System.Collections;
using System.Collections.Generic;
using Qbism.Rewind;
using UnityEngine;
using UnityEngine.UI;

namespace Qbism.UI
{
	public class RewindDisplay : MonoBehaviour
	{
		//----- TO DO: Do we remove this component completely? No longer needed?
		//Cache
		RewindHandler rewinder;
		Text rewindText;

		private void Awake() 
		{
			rewinder = FindObjectOfType<RewindHandler>();
			rewindText = GetComponent<Text>();
		}
	}
}
