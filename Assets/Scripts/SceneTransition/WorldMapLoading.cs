using System.Collections;
using System.Collections.Generic;
using Qbism.General;
using Qbism.Saving;
using Qbism.WorldMap;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Qbism.SceneTransition
{
	public class WorldMapLoading : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MapCoreRefHolder mcRef;
		[SerializeField] GameplayCoreRefHolder gcRef;

		//States
		PersistentRefHolder persRef;
		MapLogicRefHolder mlRef;
		MusicFadeOut musicFader;
		Fader fader;

		private void Awake()
		{
			if (mcRef != null)
			{
				persRef = mcRef.persRef;
				mlRef = mcRef.mlRef;
				musicFader = mcRef.musicFader;
				fader = persRef.fader;
			}
			else if (gcRef != null)
			{
				persRef = gcRef.persRef;
				musicFader = gcRef.musicFader;
				fader = persRef.fader;
			}
		}

		public void StartLoadingWorldMap(bool fromLevel)
		{
			StartCoroutine(LoadWorldMap(fromLevel));
		}

		private IEnumerator LoadWorldMap(bool fromLevel)
		{
			transform.parent = null;
			DontDestroyOnLoad(gameObject);

			//TO DO: Remove this when we have serpscreen core refs
			if (musicFader != null) musicFader.FadeMusicOut();

			//TO DO: same here
			if (fader == null) fader = FindObjectOfType<Fader>();
			yield return fader.FadeOut(fader.sceneTransTime);

			if (musicFader != null) musicFader.TurnMusicOff();
			
			yield return SceneManager.LoadSceneAsync("WorldMap");

			if (mcRef == null)
			{
				mcRef = FindObjectOfType<MapCoreRefHolder>();
				persRef = mcRef.persRef;
				mlRef = mcRef.mlRef;
			}

			mlRef.centerPoint.PositionCenterPointOnMapLoad();

			//need this yield return here to avoid race condition with selectedPinUI
			yield return null;
			var selectedPinUI = mlRef.pinTracker.selectedPin.pinUI;

			if (fromLevel)
			{
				persRef.circTransition.SetCirclePos(selectedPinUI.transform.position);
				persRef.circTransition.SetCircleStartState(1);
				persRef.circTransition.DebugFixCircleMask();
				persRef.fader.FadeImmediate(0);
				yield return persRef.circTransition.TransIn();
			}
			else
			{
				persRef.circTransition.ForceCircleSize(1);
				yield return persRef.fader.FadeIn(persRef.fader.sceneTransTime);
			} 

			Destroy(gameObject);
		}
	}
}
