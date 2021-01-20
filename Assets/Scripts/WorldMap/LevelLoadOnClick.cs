using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qbism.SceneTransition;

namespace Qbism.WorldMap
{
	public class LevelLoadOnClick : MonoBehaviour
	{
		public void LoadAssignedLevel()
		{
			var handler = FindObjectOfType<SceneHandler>();
			int indexToLoad = GetComponent<EditorSetPinValues>().levelIndex;
			handler.LoadBySceneIndex(indexToLoad);
		}
	}
}


