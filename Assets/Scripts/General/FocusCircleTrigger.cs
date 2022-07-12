using Qbism.SceneTransition;
using Qbism.WorldMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class FocusCircleTrigger : MonoBehaviour
	{
		//Config parameters
		[SerializeField] GameObject objToFocusOn;
		[SerializeField] bool isUIObj;
		[SerializeField] float circleSize = .1f, focusDur = 1;
		[SerializeField] CircleTransition focusCircle;
		[SerializeField] MapCoreRefHolder mcRef;

		//States
		Camera cam;
		LevelPinUI selectedPinUI;

		private void Awake()
		{
			if (mcRef != null) cam = mcRef.cam;
		}

		public void TriggerFocus(LevelPinUI selPinUI)
		{
			if (selPinUI != null) selectedPinUI = selPinUI;
			else selectedPinUI = null;

			StartCoroutine(Focus());
		}

		private IEnumerator Focus()
		{
			focusCircle.SetCirclePos(objToFocusOn.transform.position, !isUIObj);
			focusCircle.SetCircleStartState(0);
			focusCircle.DebugFixCircleMask();
			yield return focusCircle.TransToFocus(circleSize);
			yield return new WaitForSeconds(focusDur);
			yield return focusCircle.TransIn();

			if (selectedPinUI == null) yield break;
			mcRef.mlRef.pinTracker.SetLevelPinButtonsInteractable(true);
			mcRef.mlRef.pinTracker.SelectPin(selectedPinUI);
		}
	}
}
