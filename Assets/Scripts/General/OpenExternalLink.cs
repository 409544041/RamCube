using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class OpenExternalLink : MonoBehaviour
	{
		//Config parameters
		[SerializeField] string URL = null;

		public void OpenLink()
		{
			Application.OpenURL(URL);
		}
	}
}
