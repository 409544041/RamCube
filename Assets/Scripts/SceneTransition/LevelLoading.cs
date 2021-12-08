using System.Collections;
using System.Collections.Generic;
using Qbism.WorldMap;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Qbism.SceneTransition
{
	public class LevelLoading : MonoBehaviour
	{
		public void StartLoadingLevel(int index)
		{
			StartCoroutine(LoadLevel(index));
		}

		private IEnumerator LoadLevel(int index)
		{
			var transition = FindObjectOfType<CircleTransition>();
			var fader = FindObjectOfType<Fader>();
			var selectedPinUI = FindObjectOfType<PinSelectionTracker>().selectedPin.pinUI;

			transform.parent = null;
			DontDestroyOnLoad(gameObject);

			transition.SetCirclePos(selectedPinUI.transform.position);
			transition.SetCircleStartState(0);
			transition.DebugFixCircleMask();
			yield return transition.TransOut();

			yield return SceneManager.LoadSceneAsync(index);

			fader.FadeImmediate(1);
			transition.ForceCircleSize(1);
			yield return fader.FadeIn(fader.sceneTransTime);

			Destroy(gameObject);
		}
	}
}
