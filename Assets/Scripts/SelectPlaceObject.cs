using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectPlaceObject : MonoBehaviour {

	public Canvas canvas;
	public EventSystem eventSystem;
	public GraphicRaycaster gr;
	[Space]
	public GameObject gate;
	public Gate.GateType[] gateTypes;
	public Sprite[] icons;
	public int selectedType = 0;
	[Space]
	public GameObject placeCursor;
	public Text placeCursor_text;

	Node wiringBeginNode;
	PointerEventData ped = new PointerEventData (null);

	public void SelectObject (int index) {
		selectedType = index;
	}

	void Update () {

		ped.position = Input.mousePosition;
		bool selectedTypeIsGate = (selectedType >= 0 && selectedType < gateTypes.Length);

		if (selectedTypeIsGate && !eventSystem.IsPointerOverGameObject ()) {
			placeCursor.SetActive (true);
			placeCursor.GetComponent<Image> ().sprite = icons [selectedType];
			placeCursor_text.text = "Place " + gateTypes [selectedType].ToString ();
			placeCursor.GetComponent<RectTransform> ().position = Input.mousePosition;
		} else {
			placeCursor.SetActive (false);
		}

		if (Input.GetMouseButtonDown (0)) {

			// Reset wiring begin node
			wiringBeginNode = null;

			// If there's a gate selected and the place cursor is active, we need to place a new gate!
			if (selectedTypeIsGate && placeCursor.activeInHierarchy && !eventSystem.IsPointerOverGameObject ()) {
				GameObject newGate = Instantiate (gate, placeCursor.GetComponent<RectTransform> ().position, Quaternion.identity, canvas.transform) as GameObject;
				newGate.GetComponent<Gate> ().gateType = gateTypes [selectedType];
			} else {
				DoWireBeginning ();
			}
		}

		if (wiringBeginNode != null) {
			CheckForWireEndNode ();
		}
	}

	void DoWireBeginning () {
		// If the wire tool is selected, place a wire beginning
		if (selectedType == -1) {
			Node n = GetRaycastedNode ();
			if (n != null) {
				wiringBeginNode = n;
			}
		}
	}

	void CheckForWireEndNode () {
		
		if (Input.GetMouseButtonUp (0)) {
			// Mouse released, need check for second node
			Node n = GetRaycastedNode ();
			if (n != null) {
				if (wiringBeginNode.type != n.type) {
					if (wiringBeginNode.type == Node.Type.OUTPUT) {
						wiringBeginNode.connectedTo.Add (n);
						n.connectedFrom.Add (wiringBeginNode);
						wiringBeginNode.parent.UpdateLines ();
					} else if (n.type == Node.Type.OUTPUT) {
						n.connectedTo.Add (wiringBeginNode);
						wiringBeginNode.connectedFrom.Add (n);
						n.parent.UpdateLines ();
					}
				} else {
					if (wiringBeginNode != null) {
						Destroy (wiringBeginNode.transform.Find ("LR " + (wiringBeginNode.transform.childCount - 1)).gameObject);
					}
				}
			} else {
				Destroy (wiringBeginNode.transform.Find ("LR " + (wiringBeginNode.transform.childCount - 1)).gameObject);
			}
		}
	}

	Node GetRaycastedNode () {
		List<RaycastResult> results = new List<RaycastResult> (0);
		gr.Raycast (ped, results);
		for (int i = 0; i < results.Count; i++) {
			if (results [i].gameObject.GetComponent<Node> () != null) {
				return results [i].gameObject.GetComponent<Node> ();
			}
		}
		return null;
	}
}
