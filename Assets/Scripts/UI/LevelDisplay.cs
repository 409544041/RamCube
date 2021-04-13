using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Qbism.UI
{
	public class LevelDisplay : MonoBehaviour
	{
		//Config parameters
		[SerializeField] int firstLevelIndex;

		private void Start()
		{	
			GetComponent<Text>().text = "Level " + 
				(SceneManager.GetActiveScene().buildIndex - firstLevelIndex + 1).ToString();
		}
	}
}
