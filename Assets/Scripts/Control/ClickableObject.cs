using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickableObject : MonoBehaviour
{
	//States
	public bool canClick { get; set; }

	//Actions, events, delegates etc
	public UnityEvent onClickEvent;

	public void ClickReaction()
	{
		if(!canClick) return; 
		onClickEvent.Invoke();
	}
}
