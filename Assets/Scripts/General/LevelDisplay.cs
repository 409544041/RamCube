using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Qbism.General
{
	public class LevelDisplay : MonoBehaviour
	{
		//Config parameters
		[SerializeField] int firstLevelIndex;

		private void Start()
		{	
			GetComponent<Text>().text = SceneManager.GetActiveScene().name;
		}
	}
}
