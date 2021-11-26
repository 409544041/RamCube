using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class LevelPinWallLowerer : MonoBehaviour
	{
		//Config parameters
		public bool hasWall;
		[SerializeField] GameObject wall;
		[SerializeField] float normalYPos, loweredYPos = -12;
		[SerializeField] float loweringSpeed = 1;
		[SerializeField] LevelPin pin = null;

		public Coroutine InitiateWallLowering()
		{
			return StartCoroutine(LowerWall());
		}

		private IEnumerator LowerWall()
		{
			var step = loweringSpeed * Time.deltaTime;

			while (wall.transform.position.y > loweredYPos)	
			{
				wall.transform.position = Vector3.MoveTowards(wall.transform.position,
					new Vector3(wall.transform.position.x, loweredYPos, wall.transform.position.z), step);
				
				yield return null;
			}

			E_LevelGameplayData.FindEntity(entity =>
				entity.f_Pin == GetComponent<LevelPin>().m_levelData.f_Pin).f_WallDown = true;
		}

		public void CheckWallStatus(bool wallDown)
		{
			if (wallDown) wall.transform.position = new Vector3(wall.transform.position.x, 
				loweredYPos, wall.transform.position.z);
				
			else wall.transform.position = new Vector3(wall.transform.position.x,
				normalYPos, wall.transform.position.z);
		}
	}
}
