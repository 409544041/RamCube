using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
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

		private void DrawNewPath(Transform destination, LineTypes lineType, List<LevelPin> originPins)
		{
			var progHandler = FindObjectOfType<ProgressHandler>();

			foreach(LevelPin pin in originPins)
			{
				LineRenderer[] fullRenders = pin.fullLineRenderers;
				LineRenderer dotRender = pin.dottedLineRenderer;

				if(pin.justCompleted)
				{
					if(lineType == LineTypes.full)
					{
						for (int i = 0; i < fullRenders.Length; i++)
						{
							LineDrawer drawer = fullRenders[i].GetComponent<LineDrawer>();
							if (drawer.drawing) continue;
							drawer.pointToMove = 1;
							drawer.drawing = true;
							drawer.SetPositions(pin.pathPoint.transform, destination, false);
							fullRenders[i].enabled = true;
							return;
						}
					}

					if(lineType == LineTypes.dotted)
					{
						LineDrawer drawer = dotRender.GetComponent<LineDrawer>();
						drawer.pointToMove = 1;
						drawer.drawing = true;
						drawer.SetPositions(pin.pathPoint.transform, destination, false);
						dotRender.enabled = true;
					}
				}
			}
		}

		private void CreatePath(Transform origin, List<LineDrawData> lineDestList, 
			LineRenderer[] fullRenders, LineRenderer dotRender)
		{
			if(lineDestList.Count > 0)
			{
				for (int i = 0; i < lineDestList.Count; i++)
				{
					if (lineDestList[i].lineType == LineTypes.full)
					{
						LineDrawer fullDrawer = fullRenders[i].GetComponent<LineDrawer>();
						fullDrawer.SetPositions(origin, lineDestList[i].destination, false);
						fullRenders[i].enabled = true;
					}
					else if(lineDestList[i].lineType == LineTypes.dotted)
					{
						LineDrawer dotDrawer = dotRender.GetComponent<LineDrawer>();
						dotDrawer.SetPositions(origin, lineDestList[i].destination, false);
						dotRender.enabled = true;
					}
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
