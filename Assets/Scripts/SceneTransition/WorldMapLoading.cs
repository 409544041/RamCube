using System.Collections;
using System.Collections.Generic;
using Qbism.WorldMap;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Qbism.SceneTransition
{
	public class WorldMapLoading : MonoBehaviour
	{
		public void StartLoadingWorldMap()
		{
			StartCoroutine(LoadWorldMap());
		}

		private IEnumerator LoadWorldMap()
		{
			Fader fader = FindObjectOfType<Fader>();

			transform.parent = null;
			DontDestroyOnLoad(gameObject);

			yield return fader.FadeOut(fader.sceneTransTime);

			yield return SceneManager.LoadSceneAsync("WorldMap");
			var centerPoint = FindObjectOfType<PositionBiomeCenterpoint>();
			centerPoint.PositionCenterPointOnMapLoad();

			yield return fader.FadeIn(fader.sceneTransTime);
			Destroy(gameObject);
		}
	}
}
