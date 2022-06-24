using UnityEngine;

[CreateAssetMenu(fileName = "In-Game Dialogue Scip Obj", menuName = "ScriptableObjects/In-Game Dialogue")]

public class InGameDialogueScripOb : ScriptableObject
{
	//Config parameters
	public SegmentIDs character;
	public DialogueData[] dialogues;

	[System.Serializable]
	public class DialogueData
	{
		public Expressions expression;
		[TextArea(3, 10)]
		public string dialogueText;
	}
}
