using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class Gate : MonoBehaviour {

	public static float wireThickness = 5f;

	public enum GateType {
		AND,
		OR,
		NAND,
		NOR,
		XOR,
		XNOR,
		NOT,
		Delay,
		Input,
		Output
	};
		
	public bool value;

	public GateType gateType;

	public Text gateName, gateValue;
	public Node inputNode, outputNode;
	public Toggle inputToggle;
	public Text outputText;

	void Start () {
		gateName.text = gateType.ToString ();
		if (gateType == GateType.Input) {
			gateName.gameObject.SetActive (false);
			gateValue.gameObject.SetActive (false);
			inputNode.gameObject.SetActive (false);
			inputToggle.gameObject.SetActive (true);
		} else if (gateType == GateType.Output) {
			gateName.gameObject.SetActive (false);
			gateValue.gameObject.SetActive (false);
			outputNode.gameObject.SetActive (false);
			outputText.gameObject.SetActive (true);
		}
	}

	void Update () {
		UpdateValue ();
		if (gateType != GateType.Output) {
			gateValue.text = value.ToString ();
		} else {
			outputText.text = value.ToString ();
		}
	}

	public void UpdateLines () {

		int numLines = transform.childCount;

		if (transform.Find ("Lines") != null)
			Destroy (transform.Find ("Lines").gameObject);
		GameObject g = new GameObject ("Lines");
		g.transform.parent = transform;

		List<Vector2> points = new List<Vector2> ();

		foreach (Node n in outputNode.connectedTo) {
			if (g.GetComponent<UILineRenderer> () == null) {
				g.AddComponent<UILineRenderer> ().LineThickness = wireThickness;
			}
			/*
			g.GetComponent<UILineRenderer>().Points = new Vector2[] {
				outputNode.GetComponent<RectTransform>().position,
				n.GetComponent<RectTransform>().position
			};
			*/
			points.Add (outputNode.GetComponent<RectTransform> ().position);
			points.Add (n.GetComponent<RectTransform> ().position);
		}

		g.GetComponent<UILineRenderer> ().Points = points.ToArray ();
	}

	public void UpdateValue () {

		List<Node> inputs = inputNode.connectedFrom;
		List<bool> values = new List<bool> ();

		if (gateType != GateType.Input) {
			if (inputNode == null || inputNode.connectedFrom == null || inputs.Count == 0)
				return;
		}

		for (int i = 0; i < inputs.Count; i++) {
			if (inputs [i] != null) {
				values.Add (inputs [i].parent.value);
			}
		}

		switch (gateType) {
		case GateType.AND:
			bool hasFoundFalse = false;
			foreach (bool value in values) {
				if (!value)
					hasFoundFalse = true;
			}
			value = !hasFoundFalse;
			break;
		case GateType.OR:
			bool hasFoundTrue = false;
			foreach (bool value in values) {
				if (value)
					hasFoundTrue = true;
			}
			value = hasFoundTrue;
			break;
		case GateType.NAND:
			bool hasFalse = false;
			foreach (bool value in values) {
				if (!value)
					hasFalse = true;
			}
			value = hasFalse;
			break;
		case GateType.NOR:
			bool hasTrue = false;
			foreach (bool value in values) {
				if (value)
					hasTrue = true;
			}
			value = !hasTrue;
			break;
		case GateType.XOR:
			int numTrues = 0;
			foreach (bool value in values) {
				if (value)
					numTrues++;
			}
			value = (numTrues == 1);
			break;
		case GateType.XNOR:
			int numTrue = 0;
			foreach (bool value in values) {
				if (value)
					numTrue++;
			}
			value = !(numTrue == 1);
			break;
		case GateType.NOT:
			value = !values [0];
			break;
		case GateType.Input:
			value = inputToggle.isOn;
			break;
		case GateType.Output:
			value = values [0];
			break;
		default:
			value = false;
			break;
		}
	}
}
