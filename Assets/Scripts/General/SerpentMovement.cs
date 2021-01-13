using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using MoreMountains.Feedbacks;

namespace Qbism.General
{
	public class SerpentMovement : MonoBehaviour
	{
		//Config paramters
		[SerializeField] Transform[] bodyParts;
		[SerializeField] float minDistance = 1.25f;
		[SerializeField] float slowDownValue = .25f;
		[SerializeField] MMFeedbacks overshootJuice = null;

		//Cache
		SplineFollower follower;

		//States
		float dis;
		Transform curBodyPart;
		Transform PrevBodyPart;
		Vector3 lastPos;
		float moveSpeed = 0;
		bool isSlowing = false;

		private void Awake() 
		{
			follower = GetComponent<SplineFollower>();
		}

		void Start()
		{
			// foreach (Transform bodyPart in bodyParts)
			// {
			// 	bodyPart.GetComponent<MeshRenderer>().enabled = false;
			// }

			StartCoroutine(CalcVelocity());
		}

		void Update()
		{
			if(isSlowing) SlowDown();

			Move();
		}

		IEnumerator CalcVelocity()
		{
			while (Application.isPlaying)
			{
				lastPos = transform.position;
				yield return null;
				moveSpeed = Mathf.RoundToInt(Vector3.Distance(transform.position, lastPos) / Time.deltaTime);
			}
		}

		public void Move()
		{
			float currSpeed = follower.followSpeed;
			bodyParts[0].Translate(bodyParts[0].forward * currSpeed * Time.smoothDeltaTime, Space.World);

			for (int i = 1; i < bodyParts.Length; i++)
			{

				curBodyPart = bodyParts[i];
				PrevBodyPart = bodyParts[i - 1];

				dis = Vector3.Distance(PrevBodyPart.position, curBodyPart.position);

				Vector3 newpos = PrevBodyPart.position;

				float T = Time.deltaTime * dis / minDistance * currSpeed;

				if (T > 0.5f) T = 0.5f;
				curBodyPart.position = Vector3.Slerp(curBodyPart.position, newpos, T);
				curBodyPart.rotation = Quaternion.Slerp(curBodyPart.rotation, PrevBodyPart.rotation, T);
			}
		}

		public void InitiateSlowDown()
		{
			isSlowing = true;
		}

		private void SlowDown()
		{
			follower.followSpeed -= slowDownValue;
			if(follower.followSpeed <= 0)
			{
				isSlowing = false;
				StopMovement();
			} 
		}

		public void StopMovement()
		{
			follower.followSpeed = 0;

			overshootJuice.Initialization();
			overshootJuice.PlayFeedbacks();
		}

		public void ShowSerpent()
		{
			foreach (Transform bodyPart in bodyParts)
			{
				bodyPart.GetComponent<MeshRenderer>().enabled = true;
			}
		}
	}
}
