using System.Collections;
using System.Collections.Generic;
using Qbism.WorldMap;
using UnityEngine;

namespace Qbism.General
{
	public class LineDrawComm : MonoBehaviour
	{
		//Cache
		LevelPin[] pins;

		private void Awake() 
		{
			pins = FindObjectsOfType<LevelPin>();
		}

		private void OnEnable() 
		{
			foreach(LevelPin pin in pins)
			{
				if(pin != null) 
				{
					pin.onPathDrawing += DrawNewPath;
					pin.onPathCreation += CreatePath;
				}
			}
		}

		private void DrawNewPath(Transform destination, LineRenderer[] lRenders)
		{
			foreach(LevelPin pin in pins)
			{
				if(pin.justCompleted)
				{
					for (int i = 0; i < lRenders.Length; i++)
					{
						LineDrawer drawer = lRenders[i].GetComponent<LineDrawer>();
						if (drawer.drawing) continue;

						drawer.drawing = true;
						drawer.SetPositions(pin.pathPoint.transform, destination.transform);
						lRenders[i].enabled = true;
						return;
					}
				}
			}
		}

		private void CreatePath(Transform origin, List<Transform> destinationList, LineRenderer[] lRenders)
		{
			if(destinationList.Count > 0)
			{
				for (int i = 0; i < destinationList.Count; i++)
				{
					lRenders[i].GetComponent<LineDrawer>().SetPositions(origin, destinationList[i]);
					lRenders[i].enabled = true;
				}
			}
		}

		private void OnDisable() 
		{
			foreach (LevelPin pin in pins)
			{
				if (pin != null) 
				{
					pin.onPathDrawing -= DrawNewPath;
					pin.onPathCreation -= CreatePath;
				}
			}
		}
	}
}
