using System.Collections;
using System.Collections.Generic;
using Qbism.General;
using Qbism.WorldMap;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Qbism.SceneTransition
{
	public class WorldMapLoading : MonoBehaviour
	{
		public void StartLoadingWorldMap(bool fromLevel)
		{
			StartCoroutine(LoadWorldMap(fromLevel));
		}

		private IEnumerator LoadWorldMap(bool fromLevel)
		{
			var fader = FindObjectOfType<Fader>();
			var transition = FindObjectOfType<CircleTransition>();
			var musicFader = FindObjectOfType<MusicFadeOut>();
			

			transform.parent = null;
			DontDestroyOnLoad(gameObject);

			if (musicFader) musicFader.FadeMusicOut();
			yield return fader.FadeOut(fader.sceneTransTime);
			if (musicFader) musicFader.TurnMusicOff();
			yield return SceneManager.LoadSceneAsync("WorldMap");
			
			var centerPoint = FindObjectOfType<PositionBiomeCenterpoint>();
			centerPoint.PositionCenterPointOnMapLoad();

			//need this yield return here to avoid race condition with selectedPinUI
			yield return null;
			var selectedPinUI = FindObjectOfType<PinSelectionTracker>().selectedPin.pinUI;

			if (fromLevel)
			{
				transition.SetCirclePos(selectedPinUI.transform.position);
				transition.SetCircleStartState(1);
				transition.DebugFixCircleMask();
				fader.FadeImmediate(0);
				yield return transition.TransIn();
			}
			else
			{
				transition.ForceCircleSize(1);
				yield return fader.FadeIn(fader.sceneTransTime);
			} 

			Destroy(gameObject);
		}
	}
}
