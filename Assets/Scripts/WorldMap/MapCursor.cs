using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Qbism.WorldMap
{
	public class MapCursor : MonoBehaviour
	{
		//Config parameters
		public RectTransform cursor;
		[SerializeField] RectTransform transToRotate;
		[SerializeField] RectTransform tailOrigin;
		[SerializeField] ParticleSystem cursorTail;
		[SerializeField] Transform cursorModel;
		[SerializeField] float maxCursorSpeed = 1000, cursorAcceleration = 100;
		[SerializeField] RectTransform canvasRectTrans;
		[SerializeField] float padding = 35;
		[SerializeField] LayerMask rayCastLayers;
		[SerializeField] Animator eyeAnim, mouthAnim;
		[SerializeField] MapCoreRefHolder mcRef;

		Vector2 currentPos;
		Vector2 prevPos;
		float cursorSpeed;
		Camera cam;
		MapLogicRefHolder mlRef;
		bool deselected;

		private void Awake()
		{
			cam = mcRef.cam;
			mlRef = mcRef.mlRef;
		}

		private void Start()
		{
			currentPos = cursor.anchoredPosition;
			cursorTail.transform.forward = cam.transform.forward;
			MoveTail();
			cursorTail.Play();
		}

		private void FixedUpdate()
		{
			Vector3 cursorWorldPos = GetWorldPos(cursor.transform.position);

			Physics.SyncTransforms();
			RaycastHit[] hits = Physics.RaycastAll(cursorWorldPos, cam.transform.forward, 100,
				rayCastLayers, QueryTriggerInteraction.Collide);

			if (hits.Length > 0)
			{
				var pinRef = hits[0].transform.GetComponentInParent<LevelPinRefHolder>();
				if (pinRef.button.enabled)
				{
					mlRef.pinTracker.SelectPin(pinRef.pinUI);
					deselected = false;
				}
			}

			else if (!deselected)
			{
				mlRef.pinTracker.DeselectPin(true);
				deselected = true;
			}
		}

		public void HandleCursorMovement(Vector2 stickValue)
		{
			var currentSpeed = (currentPos - prevPos).magnitude;
			if (Mathf.Approximately(currentSpeed, 0)) cursorSpeed = 0;
			cursorSpeed += cursorAcceleration;
			if (cursorSpeed >= maxCursorSpeed) cursorSpeed = maxCursorSpeed;
			
			stickValue *= cursorSpeed * Time.deltaTime;
			
			prevPos = currentPos;
			var newPos = currentPos + stickValue;

			var bottomLeft = GetAnchoredPos(new Vector2(padding, padding));
			var topRight = GetAnchoredPos(new Vector2(Screen.width - padding,
				Screen.height - padding));

			newPos.x = Mathf.Clamp(newPos.x, bottomLeft.x, topRight.x);
			newPos.y = Mathf.Clamp(newPos.y, bottomLeft.y, topRight.y);

			var dir = newPos - currentPos;

			cursor.anchoredPosition = newPos;
			currentPos = newPos;

			var rot = Quaternion.LookRotation(Vector3.forward, dir);
			var dirIsZero = V3Equal(dir, new Vector3(0, 0, 0));

			if (!dirIsZero)
			{
				cursorModel.rotation = rot;
				transToRotate.rotation = rot;
			}

			MoveTail();
			AnimateCursor(dirIsZero);
		}

		private void AnimateCursor(bool dirIsZero)
		{
			if (dirIsZero)
			{
				eyeAnim.SetBool("isMoving", false);
				mouthAnim.SetBool("isMoving", false);
			}
			else
			{
				eyeAnim.SetBool("isMoving", true);
				mouthAnim.SetBool("isMoving", true);
			}
		}

		private void MoveTail()
		{
			var tailPos = GetWorldPos(tailOrigin.transform.position);
			cursorTail.transform.position = tailPos;

			cursorTail.transform.position += cursorTail.transform.forward * 5;
		}

		public void PlaceCursor(LevelPinRefHolder selPin, bool onMapLoad,
			bool specificPos, bool checkMinMax, Vector2 pos)
		{
			Vector3 camToPointDiff = new Vector3(0, 0, 0);
			mlRef.centerPoint.syncCenterToCursor = false;
			if (selPin == null) selPin = mlRef.centerPoint.onSavedPinFetch();

			if (onMapLoad)
			{
				mcRef.mapCam.enabled = false;
				mcRef.camBrain.enabled = false;
				camToPointDiff = mcRef.mapCam.transform.position -
					mlRef.centerPoint.transform.position;
			}

			float xPos = 0;
			float zPos = 0;

			if (!specificPos) mlRef.centerPoint.FindPos(selPin, out xPos, out zPos);

			else if (specificPos && checkMinMax)
			{
				mlRef.centerPoint.ComparePosToMinMaxValues(out xPos, out zPos, pos.x, pos.y);
			}
			
			else if (specificPos && !checkMinMax)
			{
				xPos = pos.x; zPos = pos.y;
			}

			mlRef.centerPoint.transform.position = new Vector3(xPos, 0, zPos);

			if (onMapLoad)
			{
				mcRef.mapCam.transform.position = mlRef.centerPoint.transform.position +
					camToPointDiff;
				mcRef.mapCam.enabled = true;
				mcRef.camBrain.enabled = true;
			}

			StartCoroutine(PlaceOnlyCursor(selPin.pinUI.transform.position));
		}

		private IEnumerator PlaceOnlyCursor(Vector3 pos)
		{
			yield return null;
			var screenPoint = cam.WorldToScreenPoint(pos);
			var anchoredPos = GetAnchoredPos(new Vector2(screenPoint.x, screenPoint.y));
			cursor.anchoredPosition = anchoredPos;
			currentPos = anchoredPos;
			MoveTail();
			mlRef.centerPoint.syncCenterToCursor = true;
		}

		private Vector2 GetAnchoredPos(Vector2 pos)
		{
			Vector2 anchoredPos;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTrans, pos, null,
				out anchoredPos);
			return anchoredPos;
		}

		private Vector3 GetWorldPos(Vector3 screenPoint)
		{
			return cam.ScreenToWorldPoint(screenPoint);
		}

		private bool V3Equal(Vector3 a, Vector3 b)
		{
			return Vector3.SqrMagnitude(a - b) < 0.001;
		}
	}
}
