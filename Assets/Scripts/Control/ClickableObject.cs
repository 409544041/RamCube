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
		ClickValueSetter valueSetter;
		//States
		public bool canClick { get; set; }

		//Actions, events, delegates etc
		public UnityEvent onClickEvent;

		private void Awake() 
		{
			valueSetter = GetComponent<ClickValueSetter>();
		}

		private void OnEnable() 
		{
			if(valueSetter != null) valueSetter.onSetClickValue += SetCanClick;
		}

		public void ClickReaction()
		{
			if (!canClick) return;
			onClickEvent.Invoke();
		}

		private void SetCanClick(bool value)
		{
			canClick = value;
		}

		private void OnDisable()
		{
			if (valueSetter != null) valueSetter.onSetClickValue -= SetCanClick;
		}
	}
}