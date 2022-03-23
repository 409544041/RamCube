using Qbism.Serpent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Objects
{
	public class ObjectUIHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] SerpLogicRefHolder slRef;

		public void ShowObjectUI(List<E_Objects> e_objs)
		{
			if (e_objs == null)
			{
				foreach (var selector in slRef.objRenderSelectors)
				{
					selector.HideObjects();
				}
				return;
			}

			for (int i = 0; i < e_objs.Count; i++)
			{
				ShowIndividualObjectUI(e_objs[i], i);
			}
		}

		private void ShowIndividualObjectUI(E_Objects e_obj, int i)
		{
			if (slRef.objStatusChecker.CheckIfObjectReturned(e_obj))
				slRef.objRenderSelectors[i].ShowCorrespondingObject(e_obj);
			else slRef.objRenderSelectors[i].HideObjects();
		}
	}
}
