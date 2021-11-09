using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class LevelPinRaiser : MonoBehaviour
	{
		//Config parameters
		[SerializeField] LevelPin pin;
		[SerializeField] float biomeLockedYPos = -14, lockedYPos = -11;
		public float unlockedYPos = -9;
		[SerializeField] float raiseStep = .25f;
		[SerializeField] float raiseSpeed = .05f;
		[SerializeField] GameObject pinVisuals = null;
		[SerializeField] LevelPinRaiseJuicer raiseJuicer;

		//States
		bool raising = false;

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

		public IEnumerator BiomeUnlockRaising()
		{
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

		public void InitiateRaising(List<LevelPin> originPins)
		{
			pinVisuals.transform.position = new Vector3
				(transform.position.x, lockedYPos, transform.position.z);

			StartCoroutine(RaiseCliff(originPins));
		}

		private IEnumerator RaiseCliff(List<LevelPin> originPins)
		{
			raiseJuicer.PlayRaiseJuice();
			raising = true;

			while (raising)
			{
				pinVisuals.transform.position += new Vector3(0, raiseStep, 0);

				yield return new WaitForSeconds(raiseSpeed);

				if (pinVisuals.transform.position.y >= unlockedYPos) raising = false;
			}

			pinVisuals.transform.position = new Vector3
					(transform.position.x, unlockedYPos, transform.position.z);

			raiseJuicer.StopRaiseJuice();
			pin.pinUI.ShowOrHideUI(true);

			for (int i = 0; i < originPins.Count; i++)
			{
				bool originDottedAnimPlayed = E_LevelGameplayData.FindEntity(entity =>
					entity.f_Pin == originPins[i].m_levelData.f_Pin).f_DottedAnimPlayed;

				if (originPins[i].justCompleted || !originPins[i].justCompleted && originDottedAnimPlayed)
					originPins[i].pinPather.DrawNewPath(LineTypes.full, pin.pinPather.pathPoint);
			}
		}
	}
}
