using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	protected GameObject target_;
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

	private bool _snap = false;
	private float _thetaOffset = -90;
	private float _theta, _elevation, _radius;
	private float _deltaTheta = 0;
	private CharacterController _targetController;
	private PlayerMovement _targetMovement;
	private Vector3 _desiredCameraPosition = Vector3.zero; //in cartesian coordinates
	private Vector3 _previousTargetPosition = Vector3.zero, _currentTargetPosition = Vector3.zero;

	protected virtual void InitialiseTarget  () {
		target_ = GameObject.FindGameObjectWithTag (Tags.player);
	}
	protected virtual void InitialiseTag () {
		gameObject.tag = Tags.mainCamera; 
		gameObject.name = Tags.mainCamera; 

	}
	// Use this for initialization
	void Start () {
		InitialiseTarget ();
		InitialiseTag ();
		if (!target_)
			Debug.Log ( "No Game Object to follow");

		_targetController = target_.GetComponent<CharacterController> ();
		if (!_targetController)
			Debug.Log("Target has no Character Controller Component");

		_targetMovement = target_.GetComponent<PlayerMovement> ();
		if (!_targetMovement)
			Debug.Log("Target has no Character Controller Component");	

		_theta = theta + _thetaOffset;
		_elevation = elevation;

		_desiredCameraPosition = target_.transform.position +
						 Utils.PolarToCartesian (new Vector3 (radius, _theta * Mathf.Deg2Rad, _elevation * Mathf.Deg2Rad));
	}

	// Update is called once per frame
	void LateUpdate () {
		if (!target_)
			return;

		bool isBelowTarget = transform.position.y < target_.transform.position.y;
		if (_targetController.velocity.magnitude > 0 || isBelowTarget )
			SetDesiredCameraPosition (target_.transform.position, _targetMovement.IsMovingBack ());
		else if (_targetController.velocity.magnitude == 0 && (GetCameraVerticalAxisRaw() > 0.5f || GetCameraHorizontalAxisRaw () > 0.5f ) || _snap )
			SetDesiredCameraPosition (target_.transform.position, _targetMovement.IsMovingBack ());

		Vector3 deltaPosition = (_snap ? snapCriticalDampingConst : criticalDampingConst) * (_desiredCameraPosition - transform.position) ;
		transform.position +=  deltaPosition;
		transform.LookAt (target_.transform.position);
	}

	public void SetDesiredCameraPosition (Vector3 targetPosition, bool isMovingBack) {
		float thetaOffsetFactor =GetCameraHorizontalAxisRaw();
		float elevationOffsetFactor = GetCameraVerticalAxisRaw();

		_snap = GetCameraSnapInput();
		// Debug.Log (""+ thetaOffsetFactor + " : " +elevationOffsetFactor); 
		CalculateDeltaTheta (isMovingBack);
		bool isAiming = AimControls ();
		if (_targetMovement)
			_targetMovement.SetAiming(isAiming);
		 // Evaluate theta
		float vel = 20;
		float targetTheta = theta + _deltaTheta +  _thetaOffset + (thetaThreshold) * ((isAiming) ? 1 : thetaOffsetFactor);
		_theta = Mathf.SmoothDampAngle (_theta, targetTheta, ref vel, 0.01f) ;
		float targetElevation = elevation + (elevationThreshold) * ((isAiming && !isMovingBack) ? 1 : elevationOffsetFactor);
		// Mathf.Clamp (targetElevation, 0, 90);
		_elevation = Mathf.SmoothDampAngle (_elevation, targetElevation, ref vel, 0.01f) ;
		float targetRadius = ((isAiming && !isMovingBack) ? ( aimingRadiusFactor) : 1) * radius;
		targetRadius = (isMovingBack ? 2.5F : 1) * targetRadius;
		_radius = Mathf.SmoothDamp (_radius, targetRadius, ref vel, 0.01f) ;

		_desiredCameraPosition = targetPosition + Utils.PolarToCartesian (new Vector3 (_radius, _theta * Mathf.Deg2Rad, _elevation * Mathf.Deg2Rad));
		Debug.DrawLine(targetPosition, _desiredCameraPosition);
		//Debug.Log (transform.position.ToString () + " : " + Utils.PolarToCartesian(Utils.CartesianToPolar(transform.position) )); 
	}

	private void CalculateDeltaTheta (bool isMovingBack) {
		if (!_targetController)
			return;
		float movingTheta = Utils.CartesianToPolar (_targetController.velocity).y * Mathf.Rad2Deg;
		if (movingTheta < 0) 
			movingTheta = 360 + movingTheta;
		if (!isMovingBack)
			movingTheta += 180;
		_deltaTheta = Mathf.Repeat (movingTheta, 360);
		_deltaTheta += 90;
		if (_deltaTheta > 180) 
			_deltaTheta = _deltaTheta - 360;
	}

	bool AimControls () {
		if (_targetController == null || _targetMovement == null)
			return false;
		if (GetLeftTriggerAxisRaw() == -1) {
			return true;
		} else {
			return false;
		}

	}

	void SetTargetRotation () {
		if ( _targetMovement == null || _targetController == null) 
			return;
		Vector3 cameraToTarget = transform.position - target_.transform.position;
		Vector3 moveDirection = new Vector3 (- cameraToTarget.x, 0, - cameraToTarget.z);
		_targetMovement.SetDirection (moveDirection);
	}

	bool IsMovingTowardsCamera () {
		return Vector3.Magnitude(_previousTargetPosition - transform.position) >  
				Vector3.Magnitude(_currentTargetPosition - transform.position);
	}

	private void RecordTargetPosition () {
		if (!target_)
			return;
		_previousTargetPosition = _currentTargetPosition;
		_currentTargetPosition = target_.transform.position;
	}	

	protected virtual float GetCameraHorizontalAxisRaw () {
		return Input.GetAxisRaw (Tags.CameraInputs.cameraHorizontal);
	}

	protected virtual float GetCameraVerticalAxisRaw () {
		return Input.GetAxisRaw (Tags.CameraInputs.cameraVertical);
	}

	protected virtual bool GetCameraSnapInput () {
		return Input.GetButtonDown (Tags.CameraInputs.cameraSnap);
	}

	protected virtual float GetLeftTriggerAxisRaw () {
		return Input.GetAxisRaw (Tags.CameraInputs.leftTrigger);
	}
	
}
