using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using Qbism.General;
using Qbism.WorldMap;
using UnityEngine;
using UnityEngine.SceneManagement;
using Qbism.Saving;

namespace Qbism.SceneTransition
{
	public class LevelLoading : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float transDelay = .5f;
		[SerializeField] MapCoreRefHolder mapCoreRef;

		//States
		PersistentRefHolder persRef;
		MapLogicRefHolder logicRef;

		private void Awake()
		{
			persRef = mapCoreRef.persistantRef;
			logicRef = mapCoreRef.mapLogicRef;
		}

		public void StartLoadingLevel(string levelName)
		{
			StartCoroutine(LoadLevel(levelName));
		}

		private IEnumerator LoadLevel(string levelName)
		{
			var selectedPinUI = logicRef.pinTracker.selectedPin.pinUI;

			transform.parent = null;
			DontDestroyOnLoad(gameObject);

			mapCoreRef.serpMapHandler.ActivateSerpent(selectedPinUI);

			yield return new WaitForSeconds(transDelay);

			persRef.circTransition.SetCirclePos(selectedPinUI.transform.position);
			persRef.circTransition.SetCircleStartState(0);
			persRef.circTransition.DebugFixCircleMask();

			mapCoreRef.musicFadeOut.FadeMusicOut();
			yield return persRef.circTransition.TransOut();
			mapCoreRef.musicFadeOut.TurnMusicOff();

			yield return SceneManager.LoadSceneAsync(levelName);

			//need this yield return here for debugFixCircle to work correctly
			yield return null;
			var finish = FindObjectOfType<FinishCube>();

			persRef.circTransition.SetCirclePos(finish.transform.position);
			persRef.circTransition.SetCircleStartState(1);
			persRef.circTransition.DebugFixCircleMask();
			persRef.fader.FadeImmediate(0);
			yield return persRef.circTransition.TransIn();

			Destroy(gameObject);
		}
	}
}
