using UnityEngine;
using System.Collections;

public class ElementBallMovement : MonoBehaviour {
	public float rayDistance;
	public float defaultDestroyHeight = -15.0f;
	public GameObject currentDetonator;
	public float detailLevel = 0.1f;
	public float explosionLife = 10f;

	void Start () {
		gameObject.tag = Tags.elementBall ; 
	}	

	void OnDestroy () {
		Detonator dTemp = currentDetonator.GetComponent<Detonator>();
		GameObject exp = (GameObject) Instantiate(currentDetonator, transform.position, Quaternion.identity);
		dTemp = exp.GetComponent<Detonator>();
		if(dTemp != null)
			dTemp.detail = detailLevel;
		Destroy(exp, explosionLife);
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit; // do the exploratory raycast first:
		Ray movingRay = new Ray (transform.position, Vector3.forward);
		if (Physics.Raycast (movingRay, out hit, rayDistance)) {
			if (hit.collider.tag == Tags.environment) {
				Destroy (gameObject);
			} else if (hit.collider.tag == Tags.enemy) {
				Destroy (gameObject);
			}
		} if (transform.position.y < defaultDestroyHeight) {
			Destroy (gameObject);
		}
	}

	void OnCollisionEnter (Collision coll) {
		Destroy (gameObject);
	}
}
