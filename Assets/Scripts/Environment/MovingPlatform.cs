using UnityEngine;
using System.Collections;

public class MovingPlatform : BasePlatform {

	public float plummetingTimeout = 3f;
	public float fallingForce = 3000f;

	private Vector3 _platformPositionSnapshot;

	protected override void InitialiseTag () {
		gameObject.tag = Tags.movingPlatform;
		gameObject.name = Tags.movingPlatform;
		_platformPositionSnapshot = transform.position;
	}

	void FixedUpdate () {
		if (IsPlayerPresent()) {
			Invoke ("MovePlatformDown", plummetingTimeout);
		} else {
			transform.position = _platformPositionSnapshot;
		}
	}

	void MovePlatformDown () {
		MovePlatform (Vector3.down, fallingForce);
	}

	void MovePlatform (Vector3 direction, float force) {
		//Debug.Log ("MovePlatform");
		if (!IsPlayerPresent()) 
			return;
		rigidbody.isKinematic = false;
		rigidbody.useGravity = true;
		rigidbody.AddForce (direction * force, ForceMode.Acceleration) ;
	}
}