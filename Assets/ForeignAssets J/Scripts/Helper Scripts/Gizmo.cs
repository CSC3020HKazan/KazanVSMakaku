using UnityEngine;
using System.Collections;

public class Gizmo : MonoBehaviour {
	[SerializeField]
	private Color preferedColor;
	[SerializeField]
	private float gizmoSize = 20;
	void Start () {

	}

	void Update () {

	}

	void OnDrawGizmos() {
		Gizmos.color = preferedColor; 
		Gizmos.DrawSphere (transform.position, gizmoSize) ;
	}


}