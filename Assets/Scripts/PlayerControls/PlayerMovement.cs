using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
	public float turningSpeed = 0.01F	;
	public float playerTurningSpeed = 0.2f;
	public float speedDampTime = 0.02f;
	public float jumpAngle = 45;
	public float jumpSpeed = 8.0F;
	public float walkSpeed = 10;
	public float sprintSpeed = 15;
	public float sprintJumpFactor = 0.5f;
	public float doubleJumpFactor = 0.5f;
	public float plummetingHeight = -20;
	public Material targettedEnemyMat;
	public float fieldOfViewAngle = 120f;
	public bool canJump = false;
	public bool canDoubleJump = false;
	public float aimingSpeedFactor = 0.3f; 	
	[HideInInspector]
	public GameObject targettedEnemy;
	private Animator _anim;				// Reference to the animator component.
	private HashIDs _hash; 				// Reference to the HashIDs.
	
	private Material _originalEnemyMat;

	private bool _canMove = true;
	private bool _hasDoubleJumped = false;
	private bool _isAiming = false;
	private Vector3 _currentForwardDirection = Vector3.forward;
	private Vector3 _currentRightDirection = Vector3.right;
	private Vector3 _desiredDirection = Vector3.zero;
	// private Quaternion _desiredRotation = Quaternion.identity;
	private CharacterController _controller;
	private PlayerHealth _playerHealth;
	private PlayerMana _playerMana;
	private PlayerBehaviour _playerBehaviour;
	private CameraControl _followingCamera;
	private bool _isJumping = false;
	private bool _hasRespawned = false;
	private GameMaster _gameMaster = GameMaster.Instance;
	[HideInInspector]
	public int playerIndex = 0;

	protected virtual void InitialiseTag () {
		gameObject.tag = Tags.player;
		gameObject.name = Tags.player;
		playerIndex = 0;
	}
	public virtual string GetAttachedCameraTag () {
		return Tags.mainCamera;
	}

	void Start () {
		InitialiseTag();
		_desiredDirection = transform.TransformDirection (Vector3.forward);
		_desiredDirection = _desiredDirection.normalized;
		_controller = GetComponent<CharacterController> ();
		
		_followingCamera = GameObject.FindWithTag (GetAttachedCameraTag()).GetComponent<CameraControl>();
		if (!_followingCamera)	 
			Debug.Log ("No Camera Control script attached to this attached Camera");
		_playerHealth = gameObject.GetComponent<PlayerHealth> ();
		if (!_playerHealth) {
			Debug.Log ("No Player Health script attached");
			gameObject.AddComponent<PlayerHealth>();
		}
		_playerBehaviour = gameObject.GetComponent<PlayerBehaviour> ();
		if (!_playerBehaviour) {
			Debug.Log("No Player Behaviour script attached");
			gameObject.AddComponent<PlayerBehaviour>();
		}
		_playerMana = gameObject.GetComponent<PlayerMana>  ();
		if (!_playerMana) {
			Debug.Log("No Player Mana script attached");
			gameObject.AddComponent<PlayerMana>();
		}
		// RECORD PLAYER's GLOBAL PRESENCE
		if (!_gameMaster.AddPlayer (gameObject, playerIndex)) 
			Debug.Log("PLAYER NOT RECORDED! Either already exists or wrong index!") ;
	}
	
	void FixedUpdate () {
		if (targettedEnemy != null && !_isAiming) {
			targettedEnemy.renderer.material = _originalEnemyMat;
			targettedEnemy = null;
			_originalEnemyMat = null;
		}
	}

	void Update () {
		if ((_playerBehaviour.CheckForLava () || transform.position.y < plummetingHeight ) && !_hasRespawned)
			Respawn(); 
		MovementManagement ();
		SetupCameraPosition ();
	}			
	
	void OnTriggerEnter (Collider other) { 

	}

	void OnTriggerStay (Collider other) {
		CheckForTarget (other.gameObject);
	}

	void OnTriggerExit (Collider other) {

	}
	
	private void MovementManagement () {
		bool isJumping = GetJumpInput ();
		bool isSprinting = GetSprintInput();
		// Debug.Log (_controller.isGrounded);
		if (_controller.isGrounded) {
			_isJumping = false;
			if (_hasRespawned) 
				_hasRespawned = false;

			float horizontal = GetHorizontalAxisRaw ();
			float vertical = GetVerticalAxisRaw();

			if (horizontal != 0 || vertical != 0 ) {
				_currentForwardDirection = GameObject.FindWithTag(GetAttachedCameraTag()).transform.TransformDirection(Vector3.forward);
				_currentForwardDirection.y = 0;
				_currentForwardDirection = _currentForwardDirection.normalized;
				_currentRightDirection = new Vector3 (_currentForwardDirection.z, 0, -_currentForwardDirection.x) ;
			}
			Vector3 targetDirection = horizontal * _currentRightDirection + vertical * _currentForwardDirection;
			targetDirection = targetDirection.normalized;
			_desiredDirection = _desiredDirection.normalized;

			_desiredDirection = Vector3.RotateTowards (_desiredDirection, targetDirection, playerTurningSpeed * Time.deltaTime,  1000f);
			if (targetDirection.magnitude > 0) { 
				Vector3 lookDirection;
				if (targettedEnemy != null) {
					lookDirection = targettedEnemy.transform.position - transform.position;
				} else
					lookDirection = _desiredDirection;
				transform.rotation = Quaternion.LookRotation (lookDirection);
				transform.rotation = Quaternion.Euler (new Vector3 (0 , transform.rotation.eulerAngles.y, 0));

			}
			_desiredDirection *= ((isSprinting ? sprintSpeed : walkSpeed) * (_isAiming ? aimingSpeedFactor : 1.0f));
			if (isJumping && canJump ) {
				_desiredDirection.y = (isSprinting) ? (1 + sprintJumpFactor )* jumpSpeed : jumpSpeed;
				_isJumping = true;
			}
		} else {
			if (_isJumping && canDoubleJump && !_hasDoubleJumped) {
				_hasDoubleJumped = true;
				_desiredDirection.y = (isSprinting) ? (1 + sprintJumpFactor * 0.5f )* jumpSpeed : jumpSpeed;
			}
		}	
		_desiredDirection.y -= GameMaster.Physics.GRAVITY * Time.deltaTime;

		if (_canMove) 
			_controller.Move(_desiredDirection * Time.deltaTime);
		//_followingCamera.SetDesiredCameraPosition(transform.position, IsMovingBack());
	} 

	void SetupCameraPosition () {
		if (!_followingCamera)
			return;
		_followingCamera.SetDesiredCameraPosition (_controller.bounds.max, IsMovingBack()/*((!_controller.isGrounded || true) ? false : IsMovingBack())*/, IsMovingLeftRight ());
	}

	private void Respawn () {
		if (!_playerBehaviour)
			transform.position = Vector3.zero;
		CheckPoint cp = _playerBehaviour.RespawnPosition (); 
		transform.position = cp.position;
		transform.rotation = cp.rotation;
		_currentForwardDirection = Vector3.forward;
		_currentRightDirection = Vector3.right;
		_desiredDirection = Vector3.zero;
		_playerHealth.RecordDeath ();
		_playerHealth.HealPlayer();
		_playerMana.Reset ();
	}

	private void CheckForTarget (GameObject other) { 
		if (other.tag == Tags.enemy && IsInFieldOfView (other) && _isAiming) {
			if (targettedEnemy != null) {
				targettedEnemy.renderer.material = _originalEnemyMat;
				targettedEnemy = null;
				_originalEnemyMat = null;
			}
			// Debug.Log ("CheckForTarget");
			_originalEnemyMat = other.renderer.material;
			other.renderer.material = targettedEnemyMat 	;
			targettedEnemy = other;
		} 
	} 

	private bool IsInFieldOfView (GameObject other ) {
		if (other == null)
			return false; 
		float angle  = Vector3.Angle (other.transform.position - transform.position, transform.forward) ;
		// Debug.Log ("angle" + angle);
		return (angle <= fieldOfViewAngle / 2);
	}

	public bool IsMovingBack () {
		return GetVerticalAxisRaw () < 0;
	}

	public bool IsMovingLeftRight () {
		return GetHorizontalAxisRaw () != 0 && GetVerticalAxisRaw () == 0;
	}

	public void SetAiming (bool newAim) {
		_isAiming = newAim;
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