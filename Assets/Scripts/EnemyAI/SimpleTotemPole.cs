using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleTotemPole : MonoBehaviour {

	public float deltaRotateY = 0.2f;
	public float firingTimeout = 0.2f;
	public float gravity = 20;
	public float cooldownTimeout = 2f;
	public bool canFire = false;
	public GameObject fireballPrefab;
	public List<Transform> fireballTransformList;
	public float cooldownSensor = 0.7f;

	private bool _targetInRange =false;
	private Vector3 _targetPosition = Vector3.zero;
	private string _targetTag = "";
	private Transform _defaultFireballTransform;
	private float _cooldownTimer = 0.0f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		Mathf.Clamp (_cooldownTimer, -cooldownTimeout, cooldownTimeout);
		if (_targetInRange)
			LookAtTarget ();
		else 
			RotateAboutPosition ();
		if (canFire) {
			int randomIndex = (int)Random.Range(0, fireballTransformList.Count);
			_defaultFireballTransform = fireballTransformList[randomIndex];
			if (_cooldownTimer < 0)
				FireProjectile ();
			else 
				_cooldownTimer -= Time.deltaTime;
		}
	}
 
	void OnTriggerEnter(Collider coll) {
		if (coll.tag == Tags.player || coll.tag == Tags.playerOne || coll.tag == Tags.playerTwo) {
			if (_targetTag == "" ){
				_targetTag = coll.tag;
				_targetInRange = true;
			}
			cooldownTimeout-=cooldownSensor;  // difficulty
		}
	}

	void OnTriggerStay(Collider coll) {
		if (coll.tag == _targetTag)
			_targetPosition = coll.transform.position;

	}

	void OnTriggerExit(Collider coll) {
		if (coll.tag == _targetTag) {
			_targetTag = "";
			_targetInRange = false;
			cooldownTimeout += cooldownSensor;
		}
	}

	void LookAtTarget () {
		float angle = Mathf.Atan2 (transform.position.x - _targetPosition.x, transform.position.z - _targetPosition.z) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (new Vector3 (transform.eulerAngles.x, angle + 90, transform.eulerAngles.z));
	}

	void RotateAboutPosition () {
		transform.rotation = Quaternion.Euler (new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y + deltaRotateY, transform.eulerAngles.z) );
	}

	void CheckFire (bool attack, Transform fireballTransform) {
		if (!fireballTransform) {
			Debug.Log ("No FireBallSpawnPoint GameObject");
			return;
		}
		if(attack && canFire) {
			RaycastHit hit; // do the exploratory raycast first:
			if (Physics.Raycast (fireballTransform.position, fireballTransform.forward, out hit)){
				float delay = hit.distance / (2000); // calculate the flight time
				Vector3 hitPt = hit.point;
				hitPt.y -= delay * gravity; // calculate the bullet drop at the target
				Vector3 dir = hitPt - fireballTransform.position; // use this to modify the shot direction
				// then do the actual shooting:
				Debug.DrawLine (fireballTransform.position, hit.point, Color.green, 5);
				if (Physics.Raycast (fireballTransform.position, dir, out hit)){
					//Debug.Log ("Sending ray");
					// do here the usual job when something is hit: 
					// apply damage, instantiate particles etc.	
				}
			}
		}
	}

	private void FireProjectile () {
		if (canFire) {
			GameObject target = GameObject.FindWithTag(_targetTag);
			if (!target) {
				// Debug.Log("Target defined not in the game");
				return;
			}
			GameObject fireballClone = (GameObject) Instantiate (fireballPrefab, _defaultFireballTransform.position, _defaultFireballTransform.rotation);
			fireballClone.transform.LookAt(_defaultFireballTransform.forward);
			Vector3 targetDirection = target.transform.position - _defaultFireballTransform.position;
			fireballClone.rigidbody.AddForce ( ((!target) ? _defaultFireballTransform.right: targetDirection.normalized)* 2000, ForceMode.Acceleration);
			_cooldownTimer = cooldownTimeout;
			//fireTimeout = 0.0F;
		}
	}
}
