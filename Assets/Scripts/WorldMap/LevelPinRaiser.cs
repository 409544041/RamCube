using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class LevelPinRaiser : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float biomeLockedYPos = -14, lockedYPos = -11;
		public float unlockedYPos = -9;
		[SerializeField] float raiseStep = .25f, raiseSpeed = .05f, raiseDelay = 1f;
		[SerializeField] GameObject pinVisuals = null;
		[SerializeField] LevelPinRefHolder refs;

		//States
		bool raising = false;

		//Actions, events, delegates etc
		public event Action<string> onRaisedCheckForDialogueTriggers;

		public void CheckRaiseStatus(bool unlocked, bool unlockAnimPlayed, bool biomeUnlocked)
		{
			if (!biomeUnlocked) SetVisualValues(false, biomeLockedYPos);

			else if (biomeUnlocked && (!unlocked || (unlocked && !unlockAnimPlayed)))
				SetVisualValues(true, lockedYPos);

			else if (biomeUnlocked && unlocked && unlockAnimPlayed)
				SetVisualValues(true, unlockedYPos);
		}

		private void SetVisualValues(bool renderValue, float yPos)
		{
			pinVisuals.transform.position =
				new Vector3(transform.position.x, yPos, transform.position.z);
		}

		public void InitiateBiomeUnlockRaising()
		{
			StartCoroutine(BiomeUnlockRaising());
		}

		private IEnumerator BiomeUnlockRaising()
		{
			yield return new WaitForSeconds(raiseDelay);

			raising = true;

			while (raising)
			{
				pinVisuals.transform.position += new Vector3(0, raiseStep, 0);

				yield return new WaitForSeconds(raiseSpeed);

				if (pinVisuals.transform.position.y >= lockedYPos) raising = false;
			}

			pinVisuals.transform.position = new Vector3
				(transform.position.x, lockedYPos, transform.position.z);
		}

		public void InitiateRaising(List<LevelPinRefHolder> originPins, bool locksLeft,
			List<LevelPinRefHolder> unlockPins)
		{
			pinVisuals.transform.position = new Vector3
				(transform.position.x, lockedYPos, transform.position.z);

			StartCoroutine(RaiseCliff(originPins, locksLeft, unlockPins));
		}

		private IEnumerator RaiseCliff(List<LevelPinRefHolder> originPins, bool locksLeft, 
			List<LevelPinRefHolder> unlockPins)
		{
			yield return new WaitForSeconds(raiseDelay);
			
			refs.pinRaiseJuicer.PlayRaiseJuice();
			raising = true;
			LevelPinRefHolder justCompletedPin = null;

			foreach (var pin in originPins)
			{
				if (pin.pinPather.justCompleted) justCompletedPin = pin;
			}

			while (raising)
			{
				pinVisuals.transform.position += new Vector3(0, raiseStep, 0);

				yield return new WaitForSeconds(raiseSpeed);

				if (pinVisuals.transform.position.y >= unlockedYPos) raising = false;
			}

			pinVisuals.transform.position = new Vector3
					(transform.position.x, unlockedYPos, transform.position.z);

			refs.pinRaiseJuicer.StopRaiseJuice();
			refs.pinUIJuicer.StartPlayingUnCompJuice();

			yield return new WaitForSeconds(refs.pinUIJuicer.unCompJuiceDelay);

			for (int i = 0; i < originPins.Count; i++)
			{
				var entity = E_LevelGameplayData.FindEntity(entity =>
					entity.f_Pin == originPins[i].m_levelData.f_Pin);
				bool originUnlocked = entity.f_Unlocked;

				if (originPins[i].pinPather.justCompleted || (originUnlocked && !locksLeft))
					originPins[i].pinPather.DrawNewPath(LineTypes.full, refs.pathPoint);
			}

			for (int i = 0; i < unlockPins.Count; i++)
			{
				var entity = E_LevelGameplayData.FindEntity(entity =>
					entity.f_Pin == unlockPins[i].m_levelData.f_Pin);
				bool unlockPinAlreadyUnlocked = entity.f_Unlocked;

				if (unlockPinAlreadyUnlocked && !locksLeft)
					refs.pinPather.DrawNewPath(LineTypes.full, unlockPins[i].pathPoint);
			}

			if (justCompletedPin.m_pin.Entity == E_Pin.GetEntity(0))
				refs.mcRef.serpButtonToggler.PopInButtonForFirstTime();

			onRaisedCheckForDialogueTriggers(justCompletedPin.m_pin.f_name);
		}
	}
}
