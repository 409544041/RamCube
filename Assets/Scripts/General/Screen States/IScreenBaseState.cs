using Qbism.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public interface IScreenBaseState
	{
		void StateEnter(ScreenStateManager ssm);
		void HandleStickValues(Vector2 stickValue, InputDetector inputDetector);
		void HandleActionInput();
		void HandleRewindInput();
		void HandleResetInput();
		void HandleEscapeInput();
		void HandleDebugCompleteInput();
		void HandleDebugUnlockAllInput();
		void HandleDebugDeleteProgressInput();
		void HandleDebugCompleteAllInput();
		void HandleAnyInput();
		void HandleBackInput();
		void StateExit();
	}
}
