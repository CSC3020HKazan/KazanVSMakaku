using UnityEngine;
using System.Collections;

public class SimpleEnemyAI : MonoBehaviour {
	public float firingTimeout = 0.2f;
	public float gravity = 20;
	public float cooldownTimeout = 2f;
	public float cooldownSensor = 0.7f;
	public float stoppingDistance = 0.5f;
	public bool canFire = false;
	public bool canMove = true;
	public float walkSpeed = 10;
	public GameObject fireballPrefab;
	public float turningSpeed = 12f;

	private string _targetTag = "";
	private float _cooldownTimer = 0.0f;
	private bool _targetInRange = false;
	private Transform _defaultFireballTransform;
	private SphereCollider _sphereCollider;
	private CharacterController _controller;

	// Use this for initialization
	void Start () {
		_sphereCollider = gameObject.GetComponent<SphereCollider> ();
		_defaultFireballTransform = GameObject.Find ("Enemy/FireBallSpawnPoint").transform;
		_controller = gameObject.GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		Mathf.Clamp (_cooldownTimer, -cooldownTimeout, cooldownTimeout);

		if (canFire) {
			if (_cooldownTimer < 0)
				FireProjectile ();
			else 
				_cooldownTimer -= Time.deltaTime;
		}
		ApproachTarget ();
	}
 
	void OnTriggerEnter(Collider coll) {
		if (coll.tag == Tags.player || coll.tag == Tags.playerOne || coll.tag == Tags.playerTwo) {
			if (_targetTag == "" ){
				_targetTag = coll.tag;
			}
			cooldownTimeout-=cooldownSensor;  // difficulty
			_targetInRange = true;
		}
	}

	void OnTriggerStay(Collider coll) {
		if (coll.tag == _targetTag && !_targetTag.Equals ("")) {
			canMove = (coll.transform.position - transform.position).magnitude >= stoppingDistance * ((_sphereCollider) ? _sphereCollider.radius*2 : 20);
			_targetInRange = true;
		}

	}

	void OnTriggerExit(Collider coll) {
		if (coll.tag == _targetTag) {
			_targetTag = "";
			cooldownTimeout += cooldownSensor;
			_targetInRange = false;
		}
	}

	private void CheckFire (bool attack, Transform fireballTransform) {
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
			GameObject fireballClone = (GameObject) Instantiate (fireballPrefab, _defaultFireballTransform.position, _defaultFireballTransform.rotation);
			fireballClone.transform.LookAt(_defaultFireballTransform.forward);
			fireballClone.rigidbody.AddForce ( _defaultFireballTransform.forward * 2000, ForceMode.Acceleration);
			_cooldownTimer = cooldownTimeout;
			//fireTimeout = 0.0F;
		}
	}

	private void ApproachTarget () {
		GameObject target = GameObject.FindWithTag(_targetTag);
		if (!target || !_controller) {
			// Debug.Log("Target defined not in the game");
			return;
		}
		if (canMove) {
			Vector3 moveDirection = walkSpeed * (target.transform.position - transform.position);
			moveDirection.y = 0.0f;
			_controller.Move (moveDirection * Time.deltaTime) ;
		}
		if (_targetInRange)
			LookAtTarget(target.transform.position);
	}

	private void LookAtTarget (Vector3 targetPosition) {
		float angle = Mathf.Atan2 (transform.position.x - targetPosition.x, transform.position.z - targetPosition.z) * Mathf.Rad2Deg;
		Quaternion desiredRotation = Quaternion.Euler (new Vector3 (transform.eulerAngles.x, angle +180, transform.eulerAngles.z));
		transform.rotation = Quaternion.Slerp (transform.rotation, desiredRotation, Time.fixedDeltaTime * turningSpeed);
	}
}