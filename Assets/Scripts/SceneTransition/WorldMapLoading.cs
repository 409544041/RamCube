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
		[SerializeField] MapCoreRefHolder mapCoreRef;

		//States
		PersistentRefHolder persRef;
		MapLogicRefHolder logicRef;

		private void Awake()
		{
			if (mapCoreRef != null)
			{
				persRef = mapCoreRef.persistantRef;
				logicRef = mapCoreRef.mapLogicRef;
			}
		}

		public void StartLoadingWorldMap(bool fromLevel)
		{
			StartCoroutine(LoadWorldMap(fromLevel));
		}

		private IEnumerator LoadWorldMap(bool fromLevel)
		{
			Fader fader;
			transform.parent = null;
			DontDestroyOnLoad(gameObject);

			//TO DO: link to musicfadeout in gameplay refs when in gameplay
			if (mapCoreRef != null) mapCoreRef.musicFadeOut.FadeMusicOut();
			else FindObjectOfType<MusicFadeOut>().FadeMusicOut();

			//TO DO: same here
			if (mapCoreRef != null)
				yield return persRef.fader.FadeOut(persRef.fader.sceneTransTime);
			else
			{
				fader = FindObjectOfType<Fader>();
				yield return fader.FadeOut(fader.sceneTransTime);
			}

			if (mapCoreRef != null) mapCoreRef.musicFadeOut.TurnMusicOff();
			else FindObjectOfType<MusicFadeOut>().TurnMusicOff();

			yield return SceneManager.LoadSceneAsync("WorldMap");

			if (mapCoreRef == null)
			{
				mapCoreRef = FindObjectOfType<MapCoreRefHolder>();
				persRef = mapCoreRef.persistantRef;
				logicRef = mapCoreRef.mapLogicRef;
			}

			logicRef.centerPoint.PositionCenterPointOnMapLoad();

			//need this yield return here to avoid race condition with selectedPinUI
			yield return null;
			var selectedPinUI = logicRef.pinTracker.selectedPin.pinUI;

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
