using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
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

			//need this yield return here for debugFixCircle to work correctly
			yield return null;
			var finish = FindObjectOfType<FinishCube>();

			transition.SetCirclePos(finish.transform.position);
			transition.SetCircleStartState(1);
			transition.DebugFixCircleMask();
			fader.FadeImmediate(0);
			yield return transition.TransIn();

			Destroy(gameObject);
		}
	}
}
