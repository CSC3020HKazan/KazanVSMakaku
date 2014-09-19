using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBehaviour : MonoBehaviour {
	public GameObject fireballPrefab;
	public bool canFire = false;

	protected int _playerIndex = 0;
	[SerializeField]
	private float firingForce = 2000f;
	private List<GameObject>_platformTraversed;
	private Transform _fireballTransform;
	private bool _hasFired = false;
	
	protected virtual void InitialiseTag () {
		gameObject.tag = Tags.player;
		gameObject.name = Tags.player;
	}

	void Start () {	
		InitialiseTag();
		_fireballTransform = GameObject.Find(""+gameObject.name +"/FireBallSpawnPoint").transform;
		_platformTraversed = new List<GameObject> ();
	}

	void Update () {
		if (canFire) {
			if (!_fireballTransform) {
				Debug.Log ("No FireBallSpawnPoint GameObject");
				return;
			}
			CheckFire (Input.GetButtonDown(GetAttackInputTag()), _fireballTransform);
			if (Input.GetButtonUp(GetAttackInputTag())) {
				FireProjectile(_fireballTransform);
			}

		}
	}

	void OnControllerColliderHit (ControllerColliderHit hit) {
		Rigidbody body = hit.collider.attachedRigidbody;
		if (!body)
			return;
		NotifyPlatform (hit.gameObject);
		// GetShot ();

	}

	void CheckFire (bool attack, Transform _fireballTransform) {
		if (!_fireballTransform) {
			Debug.Log ("No FireBallSpawnPoint GameObject");
			return;
		}
		if(attack && canFire) {
			_hasFired = false;
			RaycastHit hit; // do the exploratory raycast first:
			if (Physics.Raycast (_fireballTransform.position, _fireballTransform.forward, out hit)){
				float delay = hit.distance / (2000); // calculate the flight time
				Vector3 hitPt = hit.point;
				hitPt.y -= delay * GameMaster.WorldSettings.GRAVITY; // calculate the bullet drop at the target
				Vector3 dir = hitPt - _fireballTransform.position; // use this to modify the shot direction
				// then do the actual shooting:
				Debug.DrawLine (_fireballTransform.position, hit.point, Color.green, 5);
				if (Physics.Raycast (_fireballTransform.position, dir, out hit)){
					//Debug.Log ("Sending ray");
					// do here the usual job when something is hit: 
					// apply damage, instantiate particles etc.	
				}
			}
		}
	}

	void FireProjectile (Transform _fireballTransform) {
		if (!_fireballTransform) {
			Debug.Log ("No FireBallSpawnPoint GameObject");
			return;
		}
		if (canFire && !_hasFired) {
			GameObject fireballClone = (GameObject) Instantiate (fireballPrefab, _fireballTransform.position, _fireballTransform.rotation);
			fireballClone.transform.LookAt(_fireballTransform.forward);
			fireballClone.rigidbody.AddForce (_fireballTransform.forward * firingForce, ForceMode.Acceleration);
			_hasFired = true;
			//fireTimeout = 0.0F;
		}
	}

	private void NotifyPlatform (GameObject hitObject) {
		BasePlatform basePlatform = hitObject.GetComponent<BasePlatform> ();
		if(!basePlatform)
			return;
		if (_platformTraversed.Count > 0) {
			if (_platformTraversed[_platformTraversed.Count - 1].Equals (hitObject))
				return;
			_platformTraversed[_platformTraversed.Count - 1].GetComponent<BasePlatform>().SetPlayerPresence(false,_playerIndex);
			_platformTraversed.Add(hitObject);

		} else {
			_platformTraversed.Add(hitObject);    
		}
		basePlatform.SetPlayerPresence(true, _playerIndex);
	} 

	public virtual string GetAttackInputTag  () {
		return Tags.UserInputs.attack;
	}

}