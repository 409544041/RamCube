using Qbism.General;
using Qbism.Saving;
using Qbism.SceneTransition;
using Qbism.WorldMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Qbism.Demo
{
	public class DemoEndScreenLoading : MonoBehaviour
	{
		//Config parameters
		[SerializeField] M_Pin mPin;
		[SerializeField] MapLogicRefHolder mlRef;

		//States
		PersistentRefHolder persRef;
		MusicFadeOut musicFader;
		Fader fader;

		private void Awake()
		{
			persRef = mlRef.mcRef.persRef;
			musicFader = mlRef.mcRef.musicFader;
			fader = persRef.fader;
		}

		private void OnEnable()
		{
			foreach (var pin in mlRef.levelPins)
			{
				pin.pinUIJuicer.onPinCompCheckForScreenTriggers += TriggerScreen;
			}
		}

		private void TriggerScreen(string mPinString)
		{
			if (mPin == null || mPinString != mPin.f_name) return;
			StartCoroutine(LoadEndOfDemoScreen());
		}

		private IEnumerator LoadEndOfDemoScreen()
		{
			transform.parent = null;
			DontDestroyOnLoad(gameObject);

			if (musicFader != null) musicFader.FadeMusicOut();

			yield return fader.FadeOut(fader.sceneTransTime);

			if (musicFader != null) musicFader.TurnMusicOff();

			yield return SceneManager.LoadSceneAsync("EndOfDemoScene");

			persRef.circTransition.ForceCircleSize(1);
			yield return persRef.fader.FadeIn(persRef.fader.sceneTransTime);

			Destroy(gameObject);
		}

		private void OnDisable()
		{
			foreach (var pin in mlRef.levelPins)
			{
				pin.pinUIJuicer.onPinCompCheckForScreenTriggers -= TriggerScreen;
			}
		}
	}
}
