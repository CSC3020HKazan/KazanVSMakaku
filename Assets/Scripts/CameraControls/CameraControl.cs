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
	public float minElevation = 20;
	public float maxElevation = 90;
	public float elevationDampTime = 0.01f;
	public float thetaDampTime = 0.01f;
	public float snapThetaDampTime = 0.01f;
	public float radiusDampTime = 0.01f;
	public float criticalDampingConst = 0.02F;
	public float snapCriticalDampingConst = 0.5f;
	public float aimingRadiusFactor = 0.3f;
	public float fallingHeight = 0.0f;
	public float movingBackRadius = 3.0f;
	public float targetViewFactor = 0.6f;

	private float _computedThetaVelocity = 0;
	private float _computedRadiusVelocity = 0;
	private Vector3 positionVelocity = Vector3.zero;
	private float _theta = 0f, _elevation, _radius;
	private CharacterController _targetController;
	private PlayerMovement _targetMovement;
	private Vector3 _desiredCameraPosition = Vector3.zero; //in cartesian coordinates
	// private Vector3 _previousTargetPosition = Vector3.zero, _currentTargetPosition = Vector3.zero;

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

		// _theta = theta + _thetaOffset;
		_elevation = elevation;

		_desiredCameraPosition = target_.transform.position + SphereToCartesian (_radius, _theta, _elevation);
		// Initialise HUD
		HeadsUpDisplay hud = gameObject.GetComponentInChildren<HeadsUpDisplay> ();
		if (hud == null) {
			Debug.Log ("HeadsUpDisplay not attached!") ;
			return;

		}
		if (target_ != null)  {
			PlayerHealth targetHealth = target_.GetComponent<PlayerHealth> ();
			PlayerMana targetMana= target_.GetComponent<PlayerMana> ();

			if (  targetMana == null || targetHealth == null ) 
				return;
			hud.SetPlayerHealthComponent (targetHealth) ;
			hud.SetPlayerManaComponent (targetMana) ;

		}
	}

	// Update is called once per frame
	void LateUpdate () {
		if (!target_ )
			return;
		if (_targetMovement)
			_targetMovement.SetAiming(AimControls());
		// bool isBelowTarget = transform.position.y < target_.transform.position.y;
		// if (GetCameraVerticalAxisRaw() > 0.5f || GetCameraHorizontalAxisRaw () > 0.5f)
		// 	SetDesiredCameraPosition (_targetController.bounds.max, _targetMovement.IsMovingBack (), _targetMovement.IsMovingLeftRight());
		// Debug.Log ("_theta::" + _theta +"::"+ GetCameraHorizontalAxisRaw () );
		if (GetCameraSnapInput () || transform.position.y < target_.transform.position.y)  {
			float computedTheta = target_.transform.rotation.eulerAngles.y + 180;
			if (computedTheta > 360)
				computedTheta = computedTheta - 360;
			if (computedTheta < 0)
				computedTheta += 360;
			// _theta = computedTheta;
			_theta = Mathf.SmoothDampAngle (_theta, computedTheta, ref _computedThetaVelocity, snapThetaDampTime) ;
			_desiredCameraPosition = _targetController.bounds.max + SphereToCartesian (_radius, _theta, elevation);

		}
		transform.position= Vector3.SmoothDamp (transform.position, _desiredCameraPosition, ref positionVelocity, (GetCameraSnapInput ()) ? snapCriticalDampingConst : criticalDampingConst ) ;
	}

	public void SetDesiredCameraPosition (Vector3 targetPosition, bool isMovingBack, bool isMovingLeftRight) {
		bool isAiming = AimControls () && _targetMovement.targettedEnemy != null;
		// targetTheta = theta + thetaThreshold * GetCameraHorizontalAxisRaw ();
		// theta = Mathf.SmoothDampAngle (theta, targetTheta, ref vel, 0.01f) ;

		// ]]_desiredCameraPosition = targetPosition + radius * new Vector3(Mathf.Cos (elevation) * Mathf.Cos (theta) , - Mathf.Sin(elevation) * Mathf.Cos(theta), -Mathf.Sin(theta) );

		// float thetaOffsetFactor =GetCameraHorizontalAxisRaw();
		// float elevationOffsetFactor = GetCameraVerticalAxisRaw();

		// Debug.Log (""+ thetaOffsetFactor + " : " +elevationOffsetFactor); 
		// CalculateDeltaTheta (isMovingBack);

		// float targetTheta = theta + _deltaTheta +  _thetaOffset + (thetaThreshold) * (/*(isAiming)*/ false ? 1 : thetaOffsetFactor);
		// _theta = Mathf.SmoothDampAngle (_theta, targetTheta, ref vel, 0.01f) ;
		// float targetElevation = _elevation + (elevationThreshold) * ((isAiming && !isMovingBack && true) ? 1 : elevationOffsetFactor);
		// _elevation = Mathf.SmoothDampAngle (_elevation, targetElevation, ref _computedElevationVelocity, elevationDampTime) ;
		// _elevation = targetElevation;
		float targetRadius = (( isAiming && (!isMovingBack || !isMovingLeftRight ))? ( aimingRadiusFactor) : 1) * radius;
		targetRadius = (!isMovingBack || isMovingLeftRight ? 1 : movingBackRadius) * targetRadius;
		_radius = Mathf.SmoothDamp (_radius, targetRadius, ref _computedRadiusVelocity, radiusDampTime);

		//if (!isMovingBack)
			// theta = Mathf.SmoothDampAngle (theta, targetTheta, ref _computedThetaVelocity, thetaDampTime);
		if (GetCameraHorizontalAxisRaw () > 0.6|| GetCameraHorizontalAxisRaw () < -0.6)
			_theta = _theta + (thetaThreshold ) * GetCameraHorizontalAxisRaw ();
		if (GetCameraVerticalAxisRaw () > 0.6|| GetCameraVerticalAxisRaw () < -0.6) 
			_elevation = _elevation + (elevationThreshold ) * GetCameraVerticalAxisRaw ();
		_elevation = Mathf.Clamp (_elevation , minElevation, maxElevation) ;
		// Debug.Log ("GetCameraVerticalAxisRaw ():: " + GetCameraVerticalAxisRaw () + "_elevation ::"+ _elevation);
		if (_theta < 0)
			_theta += 360;
		// _theta = Mathf.Abs (360 - _theta) ;
		_theta = Mathf.Repeat (_theta, 360);
		// theta = _userTheta;
		_desiredCameraPosition = targetPosition + SphereToCartesian( _radius, _theta, _elevation) ;
		// Debug.Log (target_.transform.rotation.eulerAngles );
		// Debug.DrawLine(targetPosition, _desiredCameraPosition);
		// Debug.Log (transform.position.ToString () + " : " + Utils.PolarToCartesian(Utils.CartesianToPolar(transform.position) )); 
		if (!isAiming)
			transform.LookAt (_targetController.bounds.max ) ;
		else 
			transform.LookAt ((1 - targetViewFactor) * _targetController.bounds.max + targetViewFactor * _targetMovement.targettedEnemy.transform.position);
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

	private Vector3 SphereToCartesian (float newRadius, float newTheta, float newElevation) {
		return new Vector3( newRadius * Mathf.Sin (newElevation * Mathf.Deg2Rad) * Mathf.Sin (newTheta * Mathf.Deg2Rad) , 
							newRadius * Mathf.Cos (newElevation * Mathf.Deg2Rad),   
							newRadius * Mathf.Sin (newElevation * Mathf.Deg2Rad) * Mathf.Cos (newTheta * Mathf.Deg2Rad)	);

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
