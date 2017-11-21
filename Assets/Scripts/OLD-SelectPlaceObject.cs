using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OLD_SelectPlaceObject : MonoBehaviour {

	public enum Object {
		WIRE,
		AND,
		OR,
		NAND,
		NOR,
		XOR,
		XNOR,
		NOT,
	};

	public Object reference;

	[Tooltip("Must follow order of enum!")]
	public GameObject[] objects;

	public int selectedObject = -1;

	public GameObject placePreview;

	GameObject selectedNode;

	void Update () {
		// If selectedObject isn't null
		if (selectedObject != -1) {
			// If there's a wire selected
			if ((Object)selectedObject == Object.WIRE) {
				// If the left mouse button is pressed
				if (Input.GetMouseButtonDown (0)) {
					RaycastHit2D hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
					// Raycast
					if(hit.collider != null) {
						// If we hit a node;
						if (hit.collider.tag == "Node") {

							if (selectedNode != null) {
								if (hit.collider.gameObject.Equals (selectedNode)) {
									// Selected the same node? Let's cancel placement
									selectedNode = null;
								}

								if (hit.transform.parent.Equals (selectedNode.transform.parent)) {
									// Same parent, connecting a gate to itself directly
									selectedNode = null;
								}

								if (hit.transform.GetComponent<Node> ().type == selectedNode.GetComponent<Node> ().type) {
									// Can't connect an input to an input or an output to an output
									selectedNode = null;
								}
							}

							if (selectedNode == null) {
								selectedNode = hit.collider.gameObject;
								selectedNode.GetComponent<Renderer> ().material.color = new Color (1f, 0.9f, 0.9f);
							} else {
								// Boom, two nodes. Let's make a wire

								GameObject newWire = new GameObject ("Wire");
								newWire.AddComponent<LineRenderer> ().SetPositions (new Vector3[] {selectedNode.transform.position, hit.transform.position});
								newWire.GetComponent<LineRenderer> ().startWidth = 0.1f;

								// Reset selectedNode to be ready for the next wire
								selectedNode = null;
							}
						}
					}
				}
			} else {
				placePreview.SetActive (true);
				placePreview.GetComponent<SpriteRenderer> ().sprite = objects [selectedObject].GetComponent<Image> ().sprite;

				if (Input.GetKeyDown (KeyCode.Escape)) {
					SelectNull ();
				}

				Vector3 c = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				c.Scale (new Vector3 (1f, 1f, 0f));
				placePreview.transform.position = c;

				if (Input.GetMouseButtonDown (0) && !EventSystem.current.IsPointerOverGameObject ()) {
					print (selectedObject);
					GameObject newObject = Instantiate (objects [selectedObject], c, Quaternion.identity);
				}
			}
		} else {
			placePreview.SetActive (false);
		}
	}

	public void SelectObject (int index) {
		selectedObject = index;
	}

	public void SelectNull () {
		selectedObject = -1;
	}
}
