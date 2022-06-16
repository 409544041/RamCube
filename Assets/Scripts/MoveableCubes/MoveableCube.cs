using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Qbism.Cubes;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.MoveableCubes
{
	public class MoveableCube : MonoBehaviour
	{
		//Config parameters
		[Header("Flip Center")]
		[SerializeField] Transform center = null;
		public Transform up = null;
		public Transform down = null;
		public Transform left = null;
		public Transform right = null;
		[Header("Flipping & Lowering")]
		[SerializeField] int turnStep = 18;
		[SerializeField] float timeStep = 0.01f;
		[SerializeField] float lowerStep = 0.5f;
		[Header("Feedback")]
		[SerializeField] AudioClip landClip = null;
		public Vector3 moveScale = new Vector3(.8f, .8f, .8f);
		public Vector3 dockedScale = new Vector3(.9f, .9f, .9f);
		public CubeRefHolder refs;

		//States
		public bool isBoosting { get; set; } = false;
		public bool hasBumped { get; set; } = false;
		public bool isOutOfBounds { get; set; } = false;
		Quaternion resetRot;
		public int orderOfMovement { get; set; } = -1;
		Vector3 faceScale;
		public FloorCube currentFloorCube { get; set; }
		public bool newPlayerMove { get; set; } = false;

		//Actions, events, delegates etc
		public Func<Vector2Int, bool> onWallKeyCheck, onFloorKeyCheck, onMoveableKeyCheck, onMovingCheck;
		public Func<Vector2Int> onPlayerPosCheck;
		public Func<Vector2Int, Vector2Int, bool> onWallForCubeAheadCheck;
		public event Action<Transform, Vector3, Vector2Int, MoveableCube, Vector2Int, Vector2Int, Vector2Int> onFloorCheck;
		public event Action<Vector2Int, Vector3, Vector2Int> onStartMovingMoveable;
		public event Action<Vector2Int, MoveableCube, bool> onStopMovingMoveable;
		public event Action<MoveableCube, Transform, Vector3, Vector2Int> onActivatePlayerMove;
		public event Action<int, MoveableCube> onUpdateOrderInTimebody;

		private void Start()
		{
			UpdateCenterPosition();
			refs.cubePos.RoundPosition();
			transform.localScale = moveScale;
			if (refs.movFaceMesh != null)
			{
				faceScale = refs.movFaceMesh.transform.localScale;
				refs.movFaceMesh.transform.localScale =
					new Vector3(faceScale.x * moveScale.x, faceScale.y, faceScale.z * moveScale.z);
			}
			resetRot = transform.rotation;
			currentFloorCube = 
				refs.gcRef.glRef.cubeHandler.FetchCube(refs.cubePos.FetchGridPos(), true); ;
		}

		public void InitiateMove(Transform side, Vector3 turnAxis, Vector2Int posAhead, Vector2Int originPos)
		{
			Vector2Int currentPos = refs.cubePos.FetchGridPos();

			if (CheckForWallAhead(currentPos, posAhead) || hasBumped)
			{
				hasBumped = false;
				onStopMovingMoveable(currentPos, this, false);
				
				if (refs.movEffector != null) refs.movEffector.ToggleEffectFace(true);
				return;
			}

			StartCoroutine(Move(side, turnAxis, posAhead, originPos, currentPos));
		}

		public IEnumerator Move(Transform side, Vector3 turnAxis, Vector2Int posAhead, 
			Vector2Int originPos, Vector2Int prevPos)
		{
			if(onMoveableKeyCheck(posAhead))
			{
				hasBumped = true;
				onStartMovingMoveable(posAhead, turnAxis, refs.cubePos.FetchGridPos());
			} 	

			if(posAhead == onPlayerPosCheck())	//Checking if it bumps into player
			{
				onActivatePlayerMove(this, side, turnAxis, posAhead);
				hasBumped = true;
			}

			if(onFloorKeyCheck(posAhead)) //Normal movement
			{
				if (refs.movEffector != null) refs.movEffector.ToggleEffectFace(false);
				var startRot = transform.rotation;

				for (int i = 0; i < (90 / turnStep); i++)
				{
					if (!newPlayerMove)
					{
						transform.RotateAround(side.position, turnAxis, turnStep);
						yield return new WaitForSeconds(timeStep);
					}
					else
					{
						transform.rotation = startRot;
						transform.Rotate(turnAxis, 90, Space.World);
						transform.position = new Vector3(posAhead.x, transform.position.y, posAhead.y);
						break;
					}
				}

				refs.cubePos.RoundPosition();
				UpdateCenterPosition();

				if (side == up) posAhead += Vector2Int.up;
				else if (side == down) posAhead += Vector2Int.down;
				else if (side == left) posAhead += Vector2Int.left;
				else if (side == right) posAhead += Vector2Int.right;

				CheckFloorInNewPos(side, turnAxis, posAhead, originPos, prevPos);
			}

			//Docking
			else if(!onFloorKeyCheck(posAhead))
			{
				if (refs.movEffector != null) refs.movEffector.ToggleEffectFace(false);
				var startRot = transform.rotation;

				for (int i = 0; i < (180 / turnStep); i++)
				{
					if (!newPlayerMove)
					{
						transform.RotateAround(side.position, turnAxis, turnStep);
						yield return new WaitForSeconds(timeStep);
					}
					else
					{
						transform.rotation = startRot;
						transform.Rotate(turnAxis, 180, Space.World);
						transform.position = 
							new Vector3(posAhead.x, transform.position.y - 1, posAhead.y);
						break;
					}
				}

				transform.localScale = dockedScale;
				if (refs.movFaceMesh != null) refs.movFaceMesh.transform.localScale = faceScale;
				transform.rotation = resetRot; //reset rotation so shrink anim plays correct way up

				if (refs.movEffector != null)
				{
					refs.movEffector.UpdateFacePos();
					refs.movEffector.ToggleEffectFace(true);
					refs.cubeUI.UpdateUIPos();
				}

				refs.cubePos.RoundPosition();
				hasBumped = false;
				var cubePos = refs.cubePos.FetchGridPos();
				onStopMovingMoveable(cubePos, this, true);

				AddComponents(cubePos);
				if (refs.cubeUI != null) refs.cubeUI.showCubeUI = true;
			}
		}

		public void InitiateLowering(Vector2Int cubePos, bool fromBoost)
		{
			Vector3 targetPos = new Vector3(transform.position.x,
				transform.position.y - 1, transform.position.z);
			float step = lowerStep * Time.deltaTime;

			StartCoroutine(BecomeFloorByLowering(targetPos, step, cubePos, fromBoost));
		}

		private IEnumerator BecomeFloorByLowering(Vector3 targetPos, float step, 
			Vector2Int cubePos, bool fromBoost)
		{
			if (fromBoost && !newPlayerMove)
			{
				var juiceDur = refs.boostJuicer.FetchJuiceDur();
				refs.boostJuicer.PlayPostBoostJuice();
				yield return new WaitForSeconds(juiceDur);
			}

			while (transform.position.y > targetPos.y && !newPlayerMove)
			{
				transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
				yield return timeStep;
			}

			transform.position = targetPos;
			transform.localScale = dockedScale;
			if (refs.movFaceMesh != null) refs.movFaceMesh.transform.localScale = faceScale;
			transform.rotation = resetRot; //reset rotation so shrink anim plays correct way up
			
			if (refs.movEffector != null)
			{
				refs.movEffector.UpdateFacePos();
				refs.movEffector.ToggleEffectFace(true);
				refs.cubeUI.UpdateUIPos();
			}

			refs.cubePos.RoundPosition();
			hasBumped = false;

			onStopMovingMoveable(cubePos, this, true);

			AddComponents(cubePos);
			if (refs.cubeUI != null) refs.cubeUI.showCubeUI = true;

			//below to avoid scaling bug when boosting into wall and then lowering while fastforward moving
			yield return new WaitForSeconds(.15f);
			refs.mesh.transform.localScale = Vector3.one;
		}

		private void AddComponents(Vector2Int cubePos)
		{
			refs.floorCompAdder.AddComponent(cubePos, this.gameObject, refs.movEffector);
			UpdateCenterPosition();

			if (refs.movEffector == null) return;
			refs.movEffector.AlignCubeRotToFaceRot();
			refs.movEffector.AddMoveEffectorComponents(this.gameObject);
		}

		public bool CheckForWallAhead(Vector2Int currentPos, Vector2Int posAhead)
		{
			if (onMoveableKeyCheck(posAhead))
			{
				var posAheadOfAhead = posAhead + (posAhead - currentPos);
				return onWallForCubeAheadCheck(posAhead, posAheadOfAhead);
			}
			else return onWallKeyCheck(posAhead);
		}

		public void ApplyOrderOfMovement(int order)
		{
			orderOfMovement = order;
			onUpdateOrderInTimebody(order, this);
		}

		public void UpdateCenterPosition()
		{
			center.position = transform.position;
			refs.lineRender.transform.position = new Vector3 (transform.position.x, 
				refs.lineRender.transform.position.y, transform.position.z);

			if (refs.movEffector != null)
			{
				refs.movEffector.UpdateFacePos();
				refs.cubeUI.UpdateUIPos();
			}
		}

		public void CheckFloorInNewPos(Transform side, Vector3 turnAxis, Vector2Int posAhead,
			Vector2Int originPos, Vector2Int prevPos)
		{
			onFloorCheck(side, turnAxis, posAhead, this, refs.cubePos.FetchGridPos(), originPos, prevPos);
		}

		public void PlayLandClip()
		{
			AudioSource.PlayClipAtPoint(landClip, refs.gcRef.cam.transform.position, .05f);
		}
	}
}
