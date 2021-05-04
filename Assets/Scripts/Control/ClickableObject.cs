using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using UnityEngine;
using UnityEngine.Events;

namespace Qbism.Control
{
	public class ClickableObject : MonoBehaviour
	{
		//Cache
		ProgressHandler progHandler = null;

		//States
		public bool canClick { get; set; }

		//Actions, events, delegates etc
		public UnityEvent onClickEvent;

		private void Awake() 
		{
			progHandler = FindObjectOfType<ProgressHandler>();
		}

		public void ClickReaction()
		{
			if (!canClick) return;
			onClickEvent.Invoke();
		}
	}
}