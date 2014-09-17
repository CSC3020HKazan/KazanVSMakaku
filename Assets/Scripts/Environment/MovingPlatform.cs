using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

	public float plummetingTimeout = 3f;
	public float fallingForce = 3000f;

	private bool _playerIsPresent = false;

	void Start () {
		gameObject.tag = Tags.movingPlatform;
	}

	void FixedUpdate () {
		if (_playerIsPresent) {
			Invoke ("MovePlatformDown", plummetingTimeout);
		} 
	}

	public void SetPlayerPresence (bool isPresent) {
		_playerIsPresent = isPresent;
	}

	void MovePlatformDown () {
		MovePlatform (Vector3.down, fallingForce);
	}

	void MovePlatform (Vector3 direction, float force) {
		Debug.Log ("MovePlatform");
		if (!_playerIsPresent) 
			return;
		rigidbody.isKinematic = false;
		rigidbody.useGravity = true;
		rigidbody.AddForce (direction * force, ForceMode.Acceleration) ;
	}
}