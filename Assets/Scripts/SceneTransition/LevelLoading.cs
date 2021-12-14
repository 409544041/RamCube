using System.Collections;
using System.Collections.Generic;
using Qbism.General;
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
			var musicFader = FindObjectOfType<MusicFadeOut>();

			transform.parent = null;
			DontDestroyOnLoad(gameObject);

			transition.SetCirclePos(selectedPinUI.transform.position);
			transition.SetCircleStartState(0);
			transition.DebugFixCircleMask();

			musicFader.FadeMusicOut();
			yield return transition.TransOut();
			musicFader.TurnMusicOff();

			yield return SceneManager.LoadSceneAsync(index);

			fader.FadeImmediate(1);
			transition.ForceCircleSize(1);
			yield return fader.FadeIn(fader.sceneTransTime);

			Destroy(gameObject);
		}
	}
}
