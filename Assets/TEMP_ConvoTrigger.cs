using Qbism.Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_ConvoTrigger : MonoBehaviour
{
	public bool trigger;
	public InGameDialogueScripOb scripOb;

	bool hasTriggered = false;
    void Update()
    {
        if (trigger && !hasTriggered)
		{
			hasTriggered = true;
			FindObjectOfType<InGameDialogueManager>().StartInGameDialogue(scripOb);
		}
    }
}
