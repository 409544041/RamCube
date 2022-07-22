using System.Collections.Generic;

[System.Serializable]
public class DialogueData
{
	public Expressions firstExpr;
	public List<int> charIndexes;
	public List<Expressions> expressions;
	public List<string> dialogues;
}
