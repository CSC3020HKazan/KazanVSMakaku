using UnityEngine;
using System.Collections;

public class AgentSight : MonoBehaviour {
	
	public float fieldOfViewAngle = 200f;
	public float viewingDistance = 100f;
	public float shootingDistance = 12f;
	[HideInInspector]
	public bool playerSighted = false;
	[HideInInspector]
	public GameObject player;

	void Start () {
		SphereCollider sphereColl = gameObject.GetComponent<SphereCollider> ();
		if (sphereColl == null) {
			Debug.Log ("No Sphere Collider attached. Attaching a sphere collider.");
			sphereColl = gameObject.AddComponent<SphereCollider> ();
		}
		sphereColl.radius = viewingDistance;
		sphereColl.isTrigger = true;
	}

	void Update () {
		if (player != null && playerSighted) {
			if (!player.GetComponent<PlayerHealth>().IsAlive ())
				playerSighted = false;
		} 
	}

	void OnTriggerStay (Collider other) {
		if(GameMaster.Instance.IsPlayer(other.gameObject) ) {
			playerSighted = false;
			
			Vector3 direction = other.transform.position - transform.position;
			float angle = Vector3.Angle(direction, transform.forward);
			
			if(angle < fieldOfViewAngle * 0.5f) {
				RaycastHit hit;
				if(Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, viewingDistance)) {
					if(GameMaster.Instance.IsPlayer(hit.collider.gameObject)) {
						playerSighted = true;
						player = hit.collider.gameObject;
						int playerID = hit.collider.gameObject.GetComponent<PlayerMovement> ().playerIndex;
						GameMaster.Instance.playerSightings[playerID] = player.transform.position;
					}
				}
			}
		}
	}

	void OnTriggerExit ( Collider other ) {
		if (playerSighted) {
			if (player == other.gameObject)
				playerSighted = false;
		}
	}

}