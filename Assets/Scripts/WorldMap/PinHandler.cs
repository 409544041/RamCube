using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class PinHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] PinChecker pinChecker = null;

		public void SetPinUI(LevelPin pin, bool unlockAnimPlayed, bool completed)
		{
			if (unlockAnimPlayed) pin.pinUI.ShowOrHideUI(true);
			else pin.pinUI.ShowOrHideUI(false);

			if (completed) pin.pinUI.SetUIComplete();

			pin.pinUI.DisableLockIcon();
		}

		public void InitiateRaiseAndDrawPaths(E_LevelGameplayData entity, LevelPin pin,
			List<LevelPin> originPins, int locksAmount, int locksLeft, bool dottedAnimPlayed,
			bool unlockAnimPlayed, bool unlocked, bool completed, bool pathDrawn,
			List<E_MapWalls> originWalls, bool biomeUnlocked)
		{
			StartCoroutine(RaiseAndDrawPaths(entity, pin, originPins, locksAmount, locksLeft, 
			dottedAnimPlayed, unlockAnimPlayed, unlocked, completed, pathDrawn,
			originWalls, biomeUnlocked));
		}

		private IEnumerator RaiseAndDrawPaths(E_LevelGameplayData entity, LevelPin pin,
			List<LevelPin> originPins, int locksAmount, int locksLeft, bool dottedAnimPlayed,
			bool unlockAnimPlayed, bool unlocked, bool completed, bool pathDrawn,
			List<E_MapWalls> originWalls, bool biomeUnlocked)
		{
			bool lessLocks = (locksAmount > locksLeft) && locksLeft != 0;
			bool raised = false;

			var wallsFromOrigin = 0;
			if (originWalls != null) wallsFromOrigin = originWalls.Count;
			int loweredWalls = 0;

			for (int i = 0; i < originPins.Count; i++)
			{
				var originPin = originPins[i];
				var originGameplayEntity = E_LevelGameplayData.FindEntity(entity =>
					entity.f_Pin == originPin.m_levelData.f_Pin);
				var linkedWall = false;

				//check for walls between pin and origin pin
				if (originWalls != null)
				{
					for (int j = 0; j < originWalls.Count; j++)
					{
						if (originPin.m_levelData.f_Pin == originWalls[j].f_OriginPin)
							linkedWall = true;
					}
				}

				if (lessLocks)
				{
					//for pins with still one or more locks left
					if (!originGameplayEntity.f_DottedAnimPlayed && originPin.justCompleted)
					{
						if (linkedWall)
							originPin.pinPather.DrawToGate(LineTypes.dotted, pin.pinPather.pathPoint);

						else originPin.pinPather.DrawNewPath(LineTypes.dotted, pin.pinPather.pathPoint);

						originGameplayEntity.f_DottedAnimPlayed = true;
					}
				}

				if (unlocked && !unlockAnimPlayed)
				{
					//for newly unlocked pins that need to be raised
					if (locksLeft == 0)
					{
						//if wall, lower it first. Afterwards raise pin
						if (linkedWall)
						{
							yield return originPin.wallLowerer.InitiateWallLowering();
							loweredWalls++;
						}

						//the !raised is bc this is not originPin specific and only needs to happen once
						if (!raised && loweredWalls == wallsFromOrigin)
						{
							pin.pinRaiser.InitiateRaising(originPins);
							entity.f_UnlockAnimPlayed = true;

							//if unlocking new biome, raise all pins in that biome to correct pos
							if (!biomeUnlocked)
							{
								E_BiomeGameplayData.FindEntity(entity =>
									entity.f_Biome == pin.m_levelData.f_Pin.f_Biome).f_Unlocked = true;

								List<E_LevelData> pinsToRaise = E_LevelData.FindEntities(entity =>
									entity.f_Pin.f_Biome == pin.m_levelData.f_Pin.f_Biome);

								for (int j = 0; j < pinChecker.levelPins.Length; j++)
								{
									var gameplayEntity = E_LevelGameplayData.FindEntity(entity =>
										entity.f_Pin == pinChecker.levelPins[j].m_levelData.f_Pin);

									for (int k = 0; k < pinsToRaise.Count; k++)
									{
										if (pinChecker.levelPins[j].m_levelData.f_Pin == pinsToRaise[k].f_Pin &&
											!gameplayEntity.f_Unlocked)
											pinChecker.levelPins[j].pinRaiser.InitiateBiomeUnlockRaising();
									}
								}
							}
							
							raised = true;
						}
					}
				}
				// for pins that have already been unlocked by another level and a second path is 
				// now coming towards it
				else if (unlocked && unlockAnimPlayed && originPin.justCompleted)
					originPin.pinPather.DrawNewPath(LineTypes.full, pin.pinPather.pathPoint);
			}

			if (completed && !pathDrawn) entity.f_PathDrawn = true;
		}
	}
}