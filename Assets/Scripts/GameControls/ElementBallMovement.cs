using UnityEngine;
using System.Collections;

public class ElementBallMovement : MonoBehaviour {
	public float rayDistance;
	public float defaultDestroyHeight = -15.0f;
	public GameObject currentDetonator;
	public float detailLevel = 0.1f;
	public float explosionLife = 10f;

	private ElementBallBehaviour _ballBehaviour;

	void Start () {
		gameObject.tag = Tags.elementBall ; 
		gameObject.name = Tags.elementBall;
		_ballBehaviour = gameObject.GetComponent<ElementBallBehaviour> ();
		if (_ballBehaviour == null)
			_ballBehaviour = gameObject.AddComponent<ElementBallBehaviour> ();
	}	

	void OnDestroy () {

		// Destroy(exp, explosionLife);
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit; // do the exploratory raycast first:
		// Ray movingRay = new Ray (transform.position, Vector3.forward);
		if (Physics.Raycast (transform.position , Vector3.forward, out hit, rayDistance)) {
			// Debug.Log ("ElementBallMovement::Update");
			PlayerHealth targetHealth = hit.collider.gameObject.GetComponent<PlayerHealth>();
			if (targetHealth != null) {
				if (targetHealth.IsAlive () ) {
					targetHealth.TakeDamage (_ballBehaviour.GetPotency());
				}
			}
			Detonator dTemp = currentDetonator.GetComponent<Detonator>();
			GameObject exp = (GameObject) Instantiate(currentDetonator, transform.position, Quaternion.identity);
			dTemp = exp.GetComponent<Detonator>();
			if(dTemp != null)
				dTemp.detail = detailLevel;
			Destroy (gameObject);
		} if (transform.position.y < defaultDestroyHeight) { // TODO Check for Lava Collision
			Detonator dTemp = currentDetonator.GetComponent<Detonator>();
			GameObject exp = (GameObject) Instantiate(currentDetonator, transform.position, Quaternion.identity);
			dTemp = exp.GetComponent<Detonator>();
			if(dTemp != null)
				dTemp.detail = detailLevel;
			Destroy (gameObject); 
		}
	}

	void OnCollisionEnter (Collision coll) {
		Destroy (gameObject);
	}
}
