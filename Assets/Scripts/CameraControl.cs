using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	[Header("Zoom")]
	public float zoomSensitivity = 0.66f;
	public float minZoom = 0.1f, maxZoom = 5f;

	[Header("Pan")]
	public float panSpeedMultiplier = 0.05f;

	void Update () {
		GetComponent<Camera> ().orthographicSize += -1f * Input.GetAxis ("Mouse ScrollWheel") * zoomSensitivity * (GetComponent<Camera>().orthographicSize);
		GetComponent<Camera> ().orthographicSize = Mathf.Clamp (GetComponent<Camera> ().orthographicSize, minZoom, maxZoom);

		float os = GetComponent<Camera> ().orthographicSize;
		transform.Translate (Input.GetAxis ("Horizontal") * panSpeedMultiplier * os, Input.GetAxis ("Vertical") * panSpeedMultiplier * os, 0f);
	}
}
