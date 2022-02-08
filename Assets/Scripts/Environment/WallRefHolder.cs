using Qbism.SpriteAnimations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Qbism.Environment
{
	public class WallRefHolder : MonoBehaviour
	{
		public Collider col;
		public PopUpWall popUpWall;
		public PopUpWallJuicer wallJuicer;
		public ExpressionHandler expressHandler;
		public NavMeshObstacle navMeshOb;
	}
}
