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
		[SerializeField] MapCoreRefHolder mcRef;

		//States
		PersistentRefHolder persRef;
		MapLogicRefHolder logicRef;

		private void Awake()
		{
			persRef = mcRef.persRef;
			logicRef = mcRef.mlRef;
		}

		public void StartLoadingLevel(string levelName)
		{
			persRef.progHandler.SaveProgData();
			StartCoroutine(LoadLevel(levelName));
		}

		private IEnumerator LoadLevel(string levelName)
		{
			var selectedPin = logicRef.pinTracker.selectedPin;

			transform.parent = null;
			DontDestroyOnLoad(gameObject);

			mcRef.serpMapHandler.ActivateSerpent(selectedPin);

			yield return new WaitForSeconds(transDelay);

			persRef.circTransition.SetCirclePos(selectedPin.pinUI.transform.position);
			persRef.circTransition.SetCircleStartState(0);
			persRef.circTransition.DebugFixCircleMask();

			mcRef.musicFader.FadeMusicOut();
			yield return persRef.circTransition.TransOut();
			mcRef.musicFader.TurnMusicOff();

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
