using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public void ClearAllGates () {
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Gate")) {
			Destroy (g);
		}
	}

	public void Quit () {
		Application.Quit ();
	}
}
