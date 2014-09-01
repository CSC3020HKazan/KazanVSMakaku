using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	public float turningDampTime = 0.01f;   // A smoothing value for turning the player.
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

	public bool canJump = false;
	public bool canDoubleJump = false;

	private Animator _anim;              // Reference to the animator component.
	private HashIDs _hash;               // Reference to the HashIDs.
	
	private Vector3 _currentForwardDirection = Vector3.forward;
	private Vector3 _currentRightDirection = Vector3.right;
	private Vector3 _desiredDirection = Vector3.zero;
	private Quaternion _desiredRotation = Quaternion.identity;

	private CharacterController _controller;
	private SpawnManager _spawnManager = new SpawnManager();

	private bool _hasDoubleJumped = false;
	//private bool _isJumping = false;
	
	void Awake ()
	{
		_controller = GetComponent<CharacterController> ();
		_spawnManager.AddSpawnPoint (transform.position);
	}
	
	
	void FixedUpdate ()
	{

	}

	void Update ()
	{
		if (transform.position.y < plummetingHeight) 
			Respawn ();
		MovementManagement ();
		//transform.position = transform.position + (1_desiredPosition - transform.position) * speedDampTime;
	}
	
	
	void MovementManagement () {
		float horizontal =  Input.GetAxisRaw ("Horizontal"); 
		float vertical =  Input.GetAxisRaw ("Vertical");
		bool isJumping = Input.GetButton ("Jump");
		bool isSprinting = Input.GetButton ("Sprint");

		_currentRightDirection = Vector3.right;
		_currentForwardDirection = Vector3.forward;

		if (_controller.isGrounded) {
			_desiredDirection = horizontal * _currentRightDirection + vertical * _currentForwardDirection;
			_desiredDirection = transform.TransformDirection(_desiredDirection);
			_desiredDirection *= (isSprinting ? sprintSpeed : walkSpeed);
			if (isJumping && canJump )
				_desiredDirection.y = (isSprinting) ? sprintJumpFactor * sprintSpeed : jumpSpeed;
		} else {
			if (isJumping && canDoubleJump && !_hasDoubleJumped) {
				_hasDoubleJumped = true;
			}
		}
		_desiredDirection.y -= gravity * Time.deltaTime;
		_controller.Move(_desiredDirection * Time.deltaTime);

		transform.rotation = Quaternion.Slerp (transform.rotation, _desiredRotation, Time.time * turningSpeed);
	}

	void AudioManagement (bool shout)
	{

	}

	void Respawn () {
		transform.position = _spawnManager.SpawnAtLastCheckPoint ();
	}

	public void SetDirection (Vector3 moveDirection) {
		_desiredRotation = Quaternion.LookRotation (moveDirection);
	}
}