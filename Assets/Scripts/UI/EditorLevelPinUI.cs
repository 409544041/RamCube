using System.Collections;
using System.Collections.Generic;
using Qbism.WorldMap;
using UnityEngine;
using UnityEngine.UI;

namespace Qbism.UI
{
	[ExecuteInEditMode]
	public class EditorLevelPinUI : MonoBehaviour
	{
		//Config parameters
		public LevelPin levelPin; //TO DO: this might break
		[SerializeField] Text buttonText;
		[SerializeField] float yOffset;

		private void Awake() 
		{
			levelPin.GetComponent<EditorSetPinValues>().pinUI = this.gameObject;
		}

		private void Start()
		{
			string pinID = NameGameObject();
			PositionElement();
			buttonText.text = pinID;
		}

		private void PositionElement()
		{
			Vector3 pos = levelPin.GetComponentInChildren<LineRenderer>().transform.position;
			Vector3 offsetPos = new Vector3(pos.x, pos.y + yOffset, pos.z);

			Vector2 screenPoint = Camera.main.WorldToScreenPoint(offsetPos);
			transform.position = screenPoint;
		}

		private string NameGameObject()
		{
			string pinID = levelPin.GetComponent<LevelPin>().levelID.ToString();
			transform.gameObject.name = "pin UI " + pinID;
			return pinID;
		}
	}
}

	