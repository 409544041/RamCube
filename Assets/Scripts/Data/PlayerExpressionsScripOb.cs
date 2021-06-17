using UnityEngine;

[CreateAssetMenu(fileName = "Player Expression Scip Obj", menuName = "ScriptableObjects/Player Expressions")]
public class PlayerExpressionsScripOb : ScriptableObject
{
	//Config Parameters
	public ExpressionsData[] boostExpressions;
	public ExpressionsData[] wallHitExpressions;
	public ExpressionsData[] turningExpressions;
	public ExpressionsData[] randomPlayExpressions;
	public ExpressionsData[] fartExpressions;
	public ExpressionsData sad, shocked, slightlyPainful, 
		veryPainful, annoyed, wailingPain, lovingIt, angry;
}
