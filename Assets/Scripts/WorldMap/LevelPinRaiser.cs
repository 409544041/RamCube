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
		[SerializeField] float lockedYPos = -14, biomeUnlockedYPos = -11;
		public float unlockedYPos = -9;
		[SerializeField] float raiseStep = .25f;
		[SerializeField] float raiseSpeed = .05f;
		[SerializeField] LevelPinRaiseJuicer raiseJuicer;

		//Cache
		public MeshRenderer mRender { get; private set; }

		//States
		bool raising = false;

		private void Awake()
		{
			mRender = pin.GetComponentInChildren<MeshRenderer>();
		}

		public void CheckRaiseStatus(bool unlocked, bool unlockAnimPlayed)
		{
			if (!unlocked || (unlocked && !unlockAnimPlayed))
			{
				mRender.enabled = false;
				mRender.transform.position =
					new Vector3(transform.position.x, lockedYPos, transform.position.z);
			}

			else if (unlocked && unlockAnimPlayed)
			{
				mRender.enabled = true;
				mRender.transform.position =
				new Vector3(transform.position.x, unlockedYPos, transform.position.z);
			}
		}

		public void InitiateRaising(List<LevelPin> originPins)
		{
			mRender.transform.position = new Vector3
				(transform.position.x, lockedYPos, transform.position.z);

			StartCoroutine(RaiseCliff(originPins));
		}

		private IEnumerator RaiseCliff(List<LevelPin> originPins)
		{
			mRender.enabled = true;
			raiseJuicer.PlayRaiseJuice();
			raising = true;

			while (raising)
			{
				mRender.transform.position += new Vector3(0, raiseStep, 0);

				yield return new WaitForSeconds(raiseSpeed);

				if (mRender.transform.position.y >= unlockedYPos) raising = false;
			}

			mRender.transform.position = new Vector3
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
