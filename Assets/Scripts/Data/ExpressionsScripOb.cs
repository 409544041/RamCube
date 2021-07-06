using UnityEngine;

[CreateAssetMenu(fileName = "Expression Scip Obj", menuName = "ScriptableObjects/Expressions")]
public class ExpressionsScripOb : ScriptableObject
{
	//Config Parameters
	public Expression[] expressions;

	[System.Serializable]
	public class Expression
	{
		public ExpressionSituations situation;
		public FacialState[] facialStates;
	}

	[System.Serializable]
	public class FacialState
	{
		public BrowStates brows;
		public EyesStates eyes;
		public MouthStates mouth;
	}
}
