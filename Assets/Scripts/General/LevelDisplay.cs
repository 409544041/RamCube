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
		[SerializeField] Text nameText;
		[SerializeField] GameplayCoreRefHolder gcRef;

		private void Start()
		{
			if (gcRef.persRef.switchBoard.showLevelName)
				nameText.text = SceneManager.GetActiveScene().name;
			else nameText.enabled = false;
		}
	}
}
