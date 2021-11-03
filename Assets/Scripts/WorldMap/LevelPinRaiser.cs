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

		public void InitiateRaising(List<LevelPin> originPins)
		{
			pin.mRender.transform.position = new Vector3
				(transform.position.x, lockedYPos, transform.position.z);

			StartCoroutine(RaiseCliff(pin.mRender, originPins));
		}

		private IEnumerator RaiseCliff(MeshRenderer mRender, List<LevelPin> originPins)
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
			DrawNewPath(LineTypes.full, originPins);
		}

		public void DrawNewPath(LineTypes lineType, List<LevelPin> originPins)
		{
			var progHandler = FindObjectOfType<ProgressHandler>();

			foreach (LevelPin originPin in originPins)
			{
				LineRenderer[] fullRenders = originPin.pinPather.fullLineRenderers;
				LineRenderer dotRender = originPin.pinPather.dottedLineRenderer;

				if (originPin.justCompleted)
				{
					if (lineType == LineTypes.full)
					{
						for (int i = 0; i < fullRenders.Length; i++)
						{
							LineDrawer drawer = fullRenders[i].GetComponent<LineDrawer>();
							if (drawer.drawing) continue;
							drawer.pointToMove = 1;
							drawer.drawing = true;
							drawer.SetPositions(originPin.pinPather.pathPoint, 
								pin.pinPather.pathPoint, false);
							fullRenders[i].enabled = true;
							return;
						}
					}

					if (lineType == LineTypes.dotted)
					{
						LineDrawer drawer = dotRender.GetComponent<LineDrawer>();
						drawer.pointToMove = 1;
						drawer.drawing = true;
						drawer.SetPositions(originPin.pinPather.pathPoint, 
							pin.pinPather.pathPoint.transform, false);
						dotRender.enabled = true;
					}
				}
			}
		}
	}
}
