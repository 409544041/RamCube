using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using UnityEngine;
using UnityEngine.Events;

namespace Qbism.Control
{
	public class ClickableObject : MonoBehaviour
	{
		//States
		public bool canClick = true;

		//Actions, events, delegates etc
		public UnityEvent onClickEvent;

		public void ClickReaction()
		{
			if (!canClick) return;
			onClickEvent.Invoke();
		}
	}
}