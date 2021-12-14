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
			if (serpProg != null) serpProg.FixGameplayDelegateLinks();
			else Debug.Log("Serpent Progress not found.");
		}
	}
}
