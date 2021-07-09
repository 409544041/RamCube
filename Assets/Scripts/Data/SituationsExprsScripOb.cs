using UnityEngine;

[CreateAssetMenu(fileName = "Situation Expressions Scip Obj", menuName = "ScriptableObjects/Situation Expressions")]
public class SituationsExprsScripOb : ScriptableObject
{
	//Config Parameters
	public SituationExpressions[] situationExpressions;

	[System.Serializable]
	public class SituationExpressions
	{
		public ExpressionSituations situation;
		public Expressions[] expressions;
	}
}
