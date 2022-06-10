using Qbism.Environment;
using Qbism.SceneTransition;
using Qbism.ScreenStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class MapLogicRefHolder : MonoBehaviour
	{
		//Config parameters
		public MapCoreRefHolder mcRef;
		public MapDebugCompleter debugCompleter;
		public ScreenStateManager screenStateMngr;
		[Header("Pin Handling")]
		public PinChecker pinChecker;
		public PinHandler pinHandler;
		public PinSelectionTracker pinTracker;
		public Transform pinSelShape;
		public PositionBiomeCenterpoint centerPoint;
		public LevelPinRefHolder[] levelPins;
		[Header("Scene Handling")]
		public WorldMapLoading mapLoader;
		public LevelLoading levelLoader;
		public SerpentScreenLoading serpentLoader;
		[Header("Biome")]
		public BiomeOverwriter bOverwriter;
	}
}
