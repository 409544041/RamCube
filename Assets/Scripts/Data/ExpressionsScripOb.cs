
using UnityEngine;

[CreateAssetMenu(fileName = "Expression Scip Obj", menuName = "ScriptableObjects/Expressions")]
public class ExpressionsScripOb : ScriptableObject
{
	//Config parameters
	public ExpressionFaces[] expressionFaces;

	[System.Serializable]
	public class ExpressionFaces
	{
		public Expressions expression;
		public Face face;
	}

	[System.Serializable]
	public class Face
	{
		public BrowStates brows;
		public EyesStates eyes;
		public MouthStates mouth;
		public bool canBlink;
	}
}
