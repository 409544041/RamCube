using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Qbism.UI
{
	public class LevelDisplay : MonoBehaviour
	{
		private void Start()
		{	
			GetComponent<Text>().text = "Level " + (SceneManager.GetActiveScene().buildIndex + 1).ToString();
		}
	}
}
