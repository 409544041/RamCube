using UnityEngine;

[CreateAssetMenu(fileName = "Segment Expression Scip Obj", menuName = "ScriptableObjects/Segment Expressions")]
public class SegmentExpressionsScripOb : ScriptableObject
{
	//Config Parameters
	public ExpressionsData neutral, content, sad, veryHappy, shocked,
		happySmile, laughingAloud, slightlyPainful, veryPainful, annoyed,
		lookingUpSad, lookingUpShocked, confusedLeft, confusedRight;
}
