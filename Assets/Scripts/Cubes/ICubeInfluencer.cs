using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICubeInfluencer
{
	void PrepareAction(GameObject cube);
	IEnumerator ExecuteActionOnPlayer(GameObject cube);
	IEnumerator ExecuteActionOnFF(GameObject cube);
}
