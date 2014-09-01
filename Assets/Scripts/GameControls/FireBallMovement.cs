using UnityEngine;
using System.Collections;

public class FireBallMovement : MonoBehaviour {
	public float rayDistance;
	//public AudioClip explosionAudioClip;
	// Use this for initialization
	void Awake () {
	}

	void OnDestroy () {
		// TODO
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit; // do the exploratory raycast first:
		Ray movingRay = new Ray (transform.position, Vector3.forward);
		if (Physics.Raycast (movingRay, out hit, rayDistance)) {
			if (hit.collider.tag == Tags.environment) {
				GameMaster.RecordMiss ();
				Destroy (gameObject);

			} else if (hit.collider.tag == Tags.enemy) {
				GameMaster.RecordHit ();
				Destroy (gameObject);

			}

			//Debug.DrawLine (transform.position, transform.position + (rayDistance * rigidbody.velocity), Color.red);
		}
		if (transform.position.y < -15) {
			Destroy (gameObject);
		}

	}

	void OnCollisionEnter (Collision coll) {
		Destroy (gameObject);
	}


}
