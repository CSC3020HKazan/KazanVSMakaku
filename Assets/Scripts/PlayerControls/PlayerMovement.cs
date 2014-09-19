using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
	public float turningSpeed = 0.01F	;
	public float speedDampTime = 0.02f;
	public float jumpAngle = 45;
	public float jumpSpeed = 8.0F;
	public float walkSpeed = 10;
	public float sprintSpeed = 15;
	public float sprintJumpFactor = 0.5f;
	public float doubleJumpFactor = 0.5f;
	public float plummetingHeight = -20;

	public bool canJump = false;
	public bool canDoubleJump = false;

	private Animator _anim;				// Reference to the animator component.
	private HashIDs _hash; 				// Reference to the HashIDs.
	
	private bool _canMove = true;
	private bool _hasDoubleJumped = false;
	private bool _isAiming = false;
	private Vector3 _currentForwardDirection = Vector3.forward;
	private Vector3 _currentRightDirection = Vector3.right;
	private Vector3 _desiredDirection = Vector3.zero;
	private Quaternion _desiredRotation = Quaternion.identity;
	private CharacterController _controller;
	private CheckPointManager _checkpointManager = new CheckPointManager();
	private GameObject _followingCamera;

	//private bool _isJumping = false;
	
	protected virtual void InitialiseTag () {
		gameObject.tag = Tags.player;
		gameObject.name = Tags.player;
	}

	public virtual string GetAttachedCameraTag () {
		return Tags.mainCamera;
	}

	void Start () {
		InitialiseTag();
		_controller = GetComponent<CharacterController> ();
		_checkpointManager.AddCheckPoint (transform.position);
		_followingCamera = GameObject.FindWithTag (GetAttachedCameraTag() );
		if (!_followingCamera)	
			Debug.Log ("No Camera Attached to this Player");
	}
	
	void FixedUpdate () {

	}

	void Update () {
		if (transform.position.y < plummetingHeight) 
			Respawn ();
		MovementManagement ();
		SetupCameraPosition ();
	}			
	
	
	void MovementManagement () {
		float horizontal = GetHorizontalAxisRaw ();
		float vertical = GetVerticalAxisRaw();

		bool isJumping = GetJumpInput ();
		bool isSprinting = GetSprintInput();

		_currentRightDirection = Vector3.right;
		_currentForwardDirection = Vector3.forward;

		if (_controller.isGrounded) {
			_desiredDirection = horizontal * _currentRightDirection + vertical * _currentForwardDirection;
			_desiredDirection = transform.TransformDirection(_desiredDirection);
			_desiredDirection *= ((isSprinting ? sprintSpeed : walkSpeed) * (_isAiming ? 0.6f : 1.0f));
			if (isJumping && canJump )
				_desiredDirection.y = (isSprinting) ? sprintJumpFactor * sprintSpeed : jumpSpeed;
		} else {
			if (isJumping && canDoubleJump && !_hasDoubleJumped) {
				_hasDoubleJumped = true;
			}
		}
		
		_desiredDirection.y -= GameMaster.WorldSettings.GRAVITY * Time.deltaTime;


		if (_canMove) 
			_controller.Move(_desiredDirection * Time.deltaTime);

		transform.rotation = Quaternion.Slerp (transform.rotation, _desiredRotation, Time.time * turningSpeed);
	}

	void SetupCameraPosition () {
		if (!_followingCamera)
			return;
		//_followingCamera.
	}

	void Respawn () {
		transform.position = _checkpointManager.GetLastCheckPoint ();
	}

	public void SetPlayerRotation () {
		if (GetHorizontalAxisRaw() ==  0 && GetVerticalAxisRaw () == 0) 
			return;
		float angle = Mathf.Atan2 (GetVerticalAxisRaw(), GetHorizontalAxisRaw()) * Mathf.Rad2Deg;
		Debug.Log (angle);
		transform.rotation = Quaternion.Slerp (transform.rotation, 
				Quaternion.Euler (new Vector3 (transform.eulerAngles.x, angle - 90, transform.eulerAngles.z)), Time.deltaTime * turningSpeed);
	}

	public void SetAiming (bool newAim) {
		_isAiming = newAim;
	}

	public void SetDirection (Vector3 moveDirection) {
		_desiredRotation = Quaternion.LookRotation (moveDirection);
	}

	public void SetControllable (bool new_controllable) {
		_canMove = new_controllable;
	}

	public virtual float GetHorizontalAxisRaw () {
		return  Input.GetAxisRaw (Tags.UserInputs.horizontal);
	}
	public virtual float GetVerticalAxisRaw () {
		return  Input.GetAxisRaw (Tags.UserInputs.vertical);
	}
	public virtual bool GetSprintInput () {
		return Input.GetButton (Tags.UserInputs.sprint);
	}	
	public virtual bool GetJumpInput () {
		return  Input.GetButton (Tags.UserInputs.jump);
	}
}