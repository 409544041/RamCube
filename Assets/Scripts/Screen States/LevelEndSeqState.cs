using Qbism.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.ScreenStateMachine
{
	public class LevelEndSeqState : MonoBehaviour, IScreenBaseState
	{
		//Cache
		ScreenStateManager stateMngr;
		GameplayCoreRefHolder gcRef;

		public void StateEnter(ScreenStateManager ssm)
		{
			if (stateMngr == null)
			{
				stateMngr = ssm;
				gcRef = stateMngr.gcRef;
			}

			gcRef.pRef.outroJuicer.EnableSwallowColl();
		}

		public void StateExit()
		{
		}

		public void HandleActionInput()
		{
		}
		public void HandleAnyInput()
		{
		}
		public void HandleDebugCompleteAllInput()
		{
		}
		public void HandleDebugCompleteInput()
		{
		}
		public void HandleDebugDeleteProgressInput()
		{
		}
		public void HandleDebugUnlockAllInput()
		{
		}
		public void HandleEscapeInput()
		{
		}
		public void HandleResetInput()
		{
		}
		public void HandleRewindInput()
		{
		}
		public void HandleStickValues(Vector2 stickValue, InputDetector inputDetector)
		{
		}
		public void HandleBackInput()
		{
		}
		public void HandleDebugToggleHudInput()
		{
		}
	}
}