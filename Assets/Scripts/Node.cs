using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

	public enum Type { INPUT, OUTPUT };
	public Type type;
	public Gate parent;
	public List<Node> connectedFrom = new List<Node> ();
	public List<Node> connectedTo = new List<Node> ();
}
