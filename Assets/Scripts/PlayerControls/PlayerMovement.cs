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
	public float gravity = 20;
	public float plummetingHeight = -20;
	public GameObject fireballPrefab;

	public bool canJump = false;
	public bool canDoubleJump = false;
	public bool canFire = false;

	private Animator _anim;              // Reference to the animator component.
	private HashIDs _hash;               // Reference to the HashIDs.
	
	private bool _canMove = true;
	private bool _hasFired = false;
	private bool _hasDoubleJumped = false;
	private bool _isAiming = false;
	private bool _notifiedPlatform = false;
	private Vector3 _currentForwardDirection = Vector3.forward;
	private Vector3 _currentRightDirection = Vector3.right;
	private Vector3 _desiredDirection = Vector3.zero;
	private Transform _fireballTransform;
	private List<GameObject> _platformsTraversed = new List<GameObject> ();
	private Quaternion _desiredRotation = Quaternion.identity;
	private CharacterController _controller;
	private CheckPointManager _checkpointManager = new CheckPointManager();
	//private bool _isJumping = false;
	
	protected virtual void InitialiseTag () {
		gameObject.tag = Tags.player;
		gameObject.name = Tags.player;
	}

	void Awake ()
	{
		InitialiseTag();
		_controller = GetComponent<CharacterController> ();
		_checkpointManager.AddCheckPoint (transform.position);
		_fireballTransform = GameObject.Find(""+gameObject.name +"/FireBallSpawnPoint").transform;
	}
	
	void FixedUpdate ()
	{

	}

	void Update ()
	{
		if (transform.position.y < plummetingHeight) 
			Respawn ();
		MovementManagement ();
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
	
	
	void MovementManagement () {
		float horizontal = GetHorizontalAxisRaw ();
		float vertical = Mathf.Clamp (GetVerticalAxisRaw (), -0.2f, 1); // punish turning back
		// float vertical = Mathf.Clamp (GetVerticalAxisRaw (), 1, 1); // enable turning Back 

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
		_desiredDirection.y -= gravity * Time.deltaTime;
		if (_canMove) 
			_controller.Move(_desiredDirection * Time.deltaTime);

		transform.rotation = Quaternion.Slerp (transform.rotation, _desiredRotation, Time.time * turningSpeed);
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
				hitPt.y -= delay * gravity; // calculate the bullet drop at the target
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


	void OnControllerColliderHit (ControllerColliderHit hit) {
		Rigidbody body = hit.collider.attachedRigidbody;
		if (!body)
			return;

		if (hit.gameObject.tag.Equals(Tags.movingPlatform)) {

			if (_platformsTraversed.Count > 0) {
				if (_platformsTraversed[_platformsTraversed.Count - 1].Equals (hit.gameObject))
					return;
				else {
					_platformsTraversed[_platformsTraversed.Count - 1].GetComponent<MovingPlatform>().SetPlayerPresence(false);
					_notifiedPlatform = false;
					_platformsTraversed.Add(hit.gameObject);
				}
			} else {
				_platformsTraversed.Add (hit.gameObject);
			}
			if (!_notifiedPlatform) {
				MovingPlatform movingPlatform = _platformsTraversed[_platformsTraversed.Count - 1].GetComponent<MovingPlatform> ();
				movingPlatform.SetPlayerPresence(true);
				_notifiedPlatform = true;
			}

		} else{
			if (_platformsTraversed.Count > 0) 
				_platformsTraversed[_platformsTraversed.Count - 1].GetComponent<MovingPlatform>().SetPlayerPresence (false);
			_notifiedPlatform = false;
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
			fireballClone.rigidbody.AddForce (_fireballTransform.forward * 2000, ForceMode.Acceleration);
			_hasFired = true;
			//fireTimeout = 0.0F;
		}
	}

	void Respawn () {
		transform.position = _checkpointManager.GetLastCheckPoint ();
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
	public virtual string GetAttackInputTag  () {
		return Tags.UserInputs.attack;
	}
}