using Qbism.Serpent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Objects
{
	public class ObjectStatusChecker : MonoBehaviour
	{
		//Config parameters
		[SerializeField] SerpLogicRefHolder slRef;

		private void Start()
		{
			
		}

		/*
		foreach segment in segHandler.segments
		{
		if m_segment.gameplaydata.rescued = false. continue;
		

		list<bool> = entity.objDialoguePlay
		foreach objdialogueplay
		{

			
		}

		*/

		public bool CheckIfObjectReturned(E_Objects obj)
		{
			if (obj.f_GameplayData.f_ObjectReturned == true) return true;
			else return false;
		}
	}
}
