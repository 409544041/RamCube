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
		public float lockedYPos = -14;
		public float unlockedYPos = -9;
		[SerializeField] float raiseStep = .25f;
		[SerializeField] float raiseSpeed = .05f;
		[SerializeField] LevelPinRaiseJuicer raiseJuicer;

		bool raising = false;

		public void InitiateRaising(List<LevelPin> originPins, bool dottedAnimPlayed)
		{
			pin.mRender.transform.position = new Vector3
				(transform.position.x, lockedYPos, transform.position.z);

			StartCoroutine(RaiseCliff(pin.mRender, originPins, dottedAnimPlayed));
		}

		private IEnumerator RaiseCliff(MeshRenderer mRender, List<LevelPin> originPins,
			bool dottedAnimPlayed)
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
				if (originPins[i].justCompleted) 
					originPins[i].pinPather.DrawNewPath(LineTypes.full, 
					pin.pinPather.pathPoint, false);
					
				if (!originPins[i].justCompleted && dottedAnimPlayed)
					originPins[i].pinPather.DrawNewPath(LineTypes.dotted, 
					pin.pinPather.pathPoint, true);
			}
		}
	}
}
