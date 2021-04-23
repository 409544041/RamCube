using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Saving
{
	public class GameplayDelegateEnabler : MonoBehaviour
	{
		private void Awake()
		{
			SerpentProgress serpProg = FindObjectOfType<SerpentProgress>();
			serpProg.FixGameplayDelegateLinks();
		}
	}
}
