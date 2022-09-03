using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class LevelPinUIJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MMFeedbacks unCompJuice, compJuice, compDiamondJuice, selectedJuice, enterLevelJuice;
		[SerializeField] ParticleSystem selectVFX;
		[SerializeField] LevelPinUI pinUI;
		public float compJuiceDelay, unCompJuiceDelay;
		public float selectedSize = 1.35f;

		//Actions, events, delegates etc
		public event Action<string> onRaisedCheckForDialogueTriggers;
		public event Action<string> onPinCompCheckForScreenTriggers;

		private void Awake() 
		{
			selectedJuice.Initialization();
		}

		public void StartPlayingUnCompJuice()
		{
			StartCoroutine(PlayUnCompJuice());
		}

		private IEnumerator PlayUnCompJuice()
		{
			yield return new WaitForSeconds(unCompJuiceDelay);

			unCompJuice.Initialization();
			unCompJuice.PlayFeedbacks();
			pinUI.SetUIState(false, false, true, true, true, true, true);
		}

		public void StartPlayingCompJuice()
		{
			StartCoroutine(PlayCompJuice());
		}

		private IEnumerator PlayCompJuice()
		{
			pinUI.refs.mcRef.mlRef.screenStateMngr.mapScreenState.AddRemoveNotAllowingInput(1);

			MMFeedbackScale scaleUnComp = null;
			MMFeedbackScale scaleComp = null;

			var scalers = compJuice.GetComponents<MMFeedbackScale>();			

			for (int i = 0; i < scalers.Length; i++)
			{
				if (scalers[i].Label == "ScaleUncomp") scaleUnComp = scalers[i];
				if (scalers[i].Label == "ScaleComp") scaleComp = scalers[i];
			}

			pinUI.SetUIState(false, false, true, true, false, true, false);

			yield return new WaitForSeconds(compJuiceDelay);

			compJuice.Initialization();
			compJuice.PlayFeedbacks();

			yield return new WaitForSeconds(scaleUnComp.AnimateScaleDuration + .05f);

			pinUI.SetUIState(true, false, false, false, true, true, true);

			var compJuiceDur = compJuice.GetComponent<MMFeedbackScale>().
				AnimateScaleDuration;
			yield return new WaitForSeconds(compJuiceDur);

			compDiamondJuice.Initialization();
			compDiamondJuice.PlayFeedbacks();

			var diamondDelay = compDiamondJuice.GetComponent<MMFeedbackScale>().
				Timing.InitialDelay;
			yield return new WaitForSeconds(diamondDelay);

			pinUI.compDiamond.enabled = true;
			pinUI.uiText.enabled = true;

			yield return new WaitForSeconds(.25f);
			onPinCompCheckForScreenTriggers(pinUI.refs.m_pin.f_name);
			onRaisedCheckForDialogueTriggers(pinUI.refs.m_pin.f_name);

			pinUI.refs.mcRef.mlRef.screenStateMngr.mapScreenState.AddRemoveNotAllowingInput(-1);
		}

		public void SelectionEnlargen(float curveZero, float curveOne)
		{
			var scaler = selectedJuice.GetComponent<MMFeedbackScale>();

			scaler.RemapCurveZero = curveZero;
			scaler.RemapCurveOne = curveOne;
			
			selectedJuice.PlayFeedbacks();
			if (curveOne > 1) selectVFX.Play();
			else selectVFX.Stop();
		}

		public void PlayEnterLevelJuice()
        {
			enterLevelJuice.Initialization();
			enterLevelJuice.PlayFeedbacks();
        }
	}

}