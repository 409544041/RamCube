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

		public void InitiateRaising(List<LevelPinRefHolder> originPins)
		{
			pinVisuals.transform.position = new Vector3
				(transform.position.x, lockedYPos, transform.position.z);

			StartCoroutine(RaiseCliff(originPins));
		}

		private IEnumerator RaiseCliff(List<LevelPinRefHolder> originPins)
		{
			yield return new WaitForSeconds(raiseDelay);
			
			refs.pinRaiseJuicer.PlayRaiseJuice();
			raising = true;

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
				bool originDottedAnimPlayed = E_LevelGameplayData.FindEntity(entity =>
					entity.f_Pin == originPins[i].m_levelData.f_Pin).f_DottedAnimPlayed;

				if (originPins[i].pinPather.justCompleted || 
					!originPins[i].pinPather.justCompleted && originDottedAnimPlayed)
					originPins[i].pinPather.DrawNewPath(LineTypes.full, refs.pathPoint);
			}
		}
	}
}
