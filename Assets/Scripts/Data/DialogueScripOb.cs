using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue Scip Obj", menuName = "ScriptableObjects/Dialogue")]

public class DialogueScripOb : ScriptableObject
{
	//Config paramters
	public SegmentIDs[] characters;
	public Expressions partnerFirstExpression;
	public DialogueData[] dialogues;

	[System.Serializable]
	public class DialogueData
	{
		public int characterSpeaking;
		public Expressions expression;
		[TextArea(3, 10)]
		public string dialogueText;
	}
}
