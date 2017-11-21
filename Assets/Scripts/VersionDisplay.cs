using UnityEngine;
using UnityEngine.UI;

public class VersionDisplay : MonoBehaviour {

	public VersionControlScriptableObject v;

	void Awake () {
		GetComponent<Text> ().text = GetComponent<Text> ().text.Replace ("{VERSION}", v.version);
	}
}
