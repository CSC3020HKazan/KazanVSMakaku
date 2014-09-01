using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	private GameObject target_;
	// pi/6 < theta && phi < pi/3
	public float radius = 5;
	public float elevation = 60;
	public float theta = 0;

	public float thetaThreshold = 30;
	public float elevationThreshold = 20;

	public float smoothSnapSpeed = 0.02f;
	public float criticalDampingConst = 0.02F;
	public float snapCriticalDampingConst = 0.5f;

	public float aimingRadiusFactor = 0.3f;

	public float lockCameraTimeout = 0.2F; 

	private bool _lockCamera = false;
	private float _thetaOffset = - 90;
	private bool _snap;
	private float _deltaTheta = 0;
	private CharacterController _targetController;
	private PlayerMovement _targetMovement;
	private float _theta, _elevation, _radius;
	private Vector3 _desiredCameraPosition = Vector3.zero; //in cartesian coordinates
	private Vector3 _desiredDeltaCameraPosition; // in Polar Coordinates

	void Awake () {
		target_ = GameObject.FindGameObjectWithTag (Tags.player);
		if (!target_) {
			Debug.Log ( "No Game Object to follow");
			return;
		}
		_targetController = target_.GetComponent<CharacterController> ();
		_targetMovement = target_.GetComponent<PlayerMovement> ();

		_theta = theta + _thetaOffset;
		_elevation = elevation;

		_desiredDeltaCameraPosition =  new Vector3 (radius, _theta * Mathf.Deg2Rad, _elevation * Mathf.Deg2Rad);
		_desiredCameraPosition = target_.transform.position + Utils.PolarToCartesian (_desiredDeltaCameraPosition);

	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void LateUpdate () {
		// Vector from the target to camera
		// Checking if the Camera was was far from the target
		//bool wasFar = Vector3.Magnitude ( transform.position - target_.transform.position) >= radius; 
		bool isNear = Vector3.Magnitude(transform.position - target_.transform.position) < radius;
		_lockCamera = isNear && (_targetController.velocity.magnitude != 0);

		Vector3 deltaPosition = (!_lockCamera) ? ((_snap ? snapCriticalDampingConst : criticalDampingConst) * (_desiredCameraPosition - transform.position) ): Vector3.zero;
		transform.position += deltaPosition;
		transform.LookAt (target_.transform.position);

		SetDesiredCameraPosition ();
		SetTargetRotation ();
		
	}

	void SetDesiredCameraPosition () {
		float thetaOffsetFactor = Input.GetAxisRaw ("CameraHorizontal");
		float elevationOffsetFactor = Input.GetAxisRaw ("CameraVertical");
		_snap = Input.GetButtonDown ("CameraSnap");
		// Debug.Log (""+ thetaOffsetFactor + " : " +elevationOffsetFactor); 
		if (_targetController != null) {
			// Debug.DrawLine (target_.transform.position, target_.transform.position + _targetController.velocity, Color.yellow);


		}
		CalculateDeltaTheta ();
		bool isAiming = AimControls () || false; // remove the true statement to allow for Aiming
		 // Evaluate theta
		_theta = theta + _deltaTheta +  _thetaOffset + (thetaThreshold) * ((isAiming) ? 1 : thetaOffsetFactor);
		_elevation = elevation + (elevationThreshold) * ((isAiming) ? 1 : elevationOffsetFactor);

		_desiredDeltaCameraPosition =  new Vector3 (_radius, _theta * Mathf.Deg2Rad, _elevation * Mathf.Deg2Rad);
		_desiredCameraPosition = target_.transform.position + Utils.PolarToCartesian (_desiredDeltaCameraPosition);
		Debug.DrawLine(target_.transform.position, _desiredCameraPosition);
		//Debug.Log (transform.position.ToString () + " : " + Utils.PolarToCartesian(Utils.CartesianToPolar(transform.position) )); 
	}

	private void CalculateDeltaTheta () {
		if (_targetController != null) {
			if (_targetController.velocity.magnitude < 0.2 )
				return;
			float movingTheta = Utils.CartesianToPolar (_targetController.velocity).y * Mathf.Rad2Deg;
			if (movingTheta < 0) 
				movingTheta = 360 + movingTheta;
			_deltaTheta = Mathf.Repeat (movingTheta + 180, 360);
			_deltaTheta += 90;
			if (_deltaTheta > 180) 
				_deltaTheta = _deltaTheta - 360;
		} 
	}

	bool AimControls () {
		if (_targetController == null)
			return false;
		if (_targetMovement == null)
			return false;
		if (Input.GetAxisRaw ("LeftTrigger") == -1) {
			_radius = radius * aimingRadiusFactor;
			return true;
		} else {
			_radius = radius;
			return false;
		}

	}

	void SetTargetRotation () {
		if (_lockCamera || _targetMovement == null || _targetController == null) 
			return;
		//if (_targetController.velocity.magnitude > 0.2f)
			//return;
		Vector3 cameraToTarget = transform.position - target_.transform.position;
		Vector3 moveDirection = new Vector3 (- cameraToTarget.x, 0, - cameraToTarget.z);
		_targetMovement.SetDirection (moveDirection);
	}

	bool IsCloseToTarget () {
		Vector3 cameraToTarget = transform.position - target_.transform.position;
		return !(Vector3.Magnitude (cameraToTarget) > radius);
	}
}
