using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using Qbism.WorldMap;
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

		private void OnEnable() 
		{
			if(progHandler != null) progHandler.onSetClickValue += SetCanClick;
		}

		public void ClickReaction()
		{
			if (!canClick) return;
			onClickEvent.Invoke();
		}

		private void SetCanClick(LevelPin pin, bool value)
		{
			if(pin.levelID == GetComponent<LevelPin>().levelID)
			{
				canClick = value;
			}
		}

		private void OnDisable()
		{
			if (progHandler != null) progHandler.onSetClickValue -= SetCanClick;
		}
	}
}