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
		[SerializeField] float loweringSpeed = 1, loweringInterval = .1f, loweringDelay = 1f;
		[SerializeField] LevelPinRefHolder refs;

		public Coroutine InitiateWallLowering()
		{
			return StartCoroutine(LowerWallOneByOne());
		}

		private IEnumerator LowerWallOneByOne()
		{
			yield return new WaitForSeconds(loweringDelay);

			E_LevelGameplayData.FindEntity(entity =>
				entity.f_Pin == refs.m_levelData.f_Pin).f_WallDown = true;

			var wallPillars = wall.GetComponentsInChildren<WallPillarSpawner>();

			for (int i = 0; i < wallPillars.Length; i++)
			{
				StartCoroutine(LowerWallPillar(wallPillars[i].transform));

				yield return new WaitForSeconds(loweringInterval);
			}
		}

		private IEnumerator LowerWallPillar(Transform pillarTrans)
		{
			var step = loweringSpeed * Time.deltaTime;

			while (pillarTrans.transform.position.y > loweredYPos)
			{
				pillarTrans.position = Vector3.MoveTowards(pillarTrans.position,
					new Vector3(pillarTrans.position.x, loweredYPos, pillarTrans.position.z), step);

				yield return null;
			}
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
