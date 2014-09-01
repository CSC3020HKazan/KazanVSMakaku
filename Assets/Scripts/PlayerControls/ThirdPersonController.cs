using UnityEngine;
using System.Collections;

public class ThirdPersonController : MonoBehaviour {


	//private CharacterState _characterState;

	// The speed when walking 
	public float walkSpeed = 4.0F;
	// after trotAfterSeconds of walking we trot with trotSpeed 
	//public float trotSpeed = 4.0F;
	// when pressing "Fire3" button (cmd) we start running 
	//public float runSpeed = 6.0F;
	//public float inAirControlAcceleration = 3.0F;

	// How high do we jump when pressing jump and letting go immediately 
//	public float jumpHeight = 0.5F;

	// The gravity for the character 
	public float gravity = 20.0F;
	// The gravity in controlled descent mode 
	public float speedSmoothing = 10.0F;
	public float rotateSpeed = 500.0F; 
	//public float trotAfterSeconds = 3.0F;
//	public bool canJump = false;

	// Projectiles to fire
	public GameObject fireballPrefab;
	private Animator anim; 
	private HashIDs hash;
	
//	private float jumpRepeatTime = 0.05F;
//	private float jumpTimeout = 0.15F;
	private float groundedTimeout = 0.25F;

	// The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.
	private float lockCameraTimer = 0.0F;

	// The current move direction in x-z
	private Vector3 moveDirection = Vector3.zero;
	// The current vertical speed
	private float verticalSpeed = 0.0F;
	// The current x-z move speed
	private float moveSpeed = 0.0F;

	// The last collision flags returned from controller.Move
	private CollisionFlags collisionFlags; 

	// Are we jumping? (Initiated with jump button and not grounded yet)
//	private bool jumping = false;
//	private bool jumpingReachedApex = false;

	// Are we moving backwards (This locks the camera to not do a 180 degree spin)
	private bool movingBack = false;
	// Is the user pressing any keys?
	private bool isMoving = false;
	// When did the user start walking (Used for going into trot after a while)
	//private float walkTimeStart = 0.0F;
	// Last time the jump button was clicked down
//	private float lastJumpButtonTime = -10.0F;
	// Last time we performed a jump
//	private float lastJumpTime = -1.0F;

	//private float fireTimeout = 0.0F;
	[System.NonSerialized]
	public bool canFire = true;
	private bool hasFired = false;
	//private float attackingAnimationOffset = 0.25F;
	// the height we jumped from (Used to determine for how long to apply extra jump power after jumping.)
//	private float lastJumpStartHeight = 0.0F;
	private Vector3 inAirVelocity = Vector3.zero;
	private float lastGroundedTime = 0.0F;
	private bool isControllable = true;

	void Awake ()
	{
		moveDirection = transform.TransformDirection(Vector3.forward);
		anim = GetComponent<Animator>();
		hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
	}

	void MovementManagement ( float vertical, float horizontal) {
		bool grounded = IsGrounded ();
		Transform cameraTransform = Camera.main.transform;
		Vector3 forward = cameraTransform.TransformDirection (Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;
		Vector3 right = new Vector3 (forward.z, 0, -forward.x);



		bool wasMoving = isMoving;
		isMoving = Mathf.Abs (horizontal) > 0.1 || Mathf.Abs (vertical) > 0.1;
		
		if (vertical < -0.2)
			movingBack = true;
		else
			movingBack = false;
		// Are we moving backwards or looking backwards


		// Target direction relative to the camera
		Vector3 targetDirection = horizontal * right + vertical * forward;


		// Grounded controls
		if (grounded) {
			// Lock camera for short period when transitioning moving & standing still
			lockCameraTimer += Time.deltaTime;
			if (isMoving != wasMoving)
				lockCameraTimer = 0.0F;
			
			// We store speed and direction seperately,
			// so that when the character stands still we still have a valid forward direction
			// moveDirection is always normalized, and we only update it if there is user input.
			if (targetDirection != Vector3.zero) {
				// If we are really slow, just snap to the target direction
				if (moveSpeed < walkSpeed * 0.9 && grounded){
					moveDirection = targetDirection.normalized;
				}
				// Otherwise smoothly turn towards it
				else {
					moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
					moveDirection = moveDirection.normalized;
				}
			}
			
			// Smooth the speed based on the current target direction
			float curSmooth = speedSmoothing * Time.deltaTime;
			
			// Choose target speed
			//* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
			float targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0F);
			
			// Pick speed modifier
			//			if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))
			//			{
			//				targetSpeed *= runSpeed;
			//				_characterState = CharacterState.Running;
			//			}
			targetSpeed *= walkSpeed;

			moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);
			
			// Reset walk time start when we slow down
		}


	}


	void ApplyGravity ()
	{
		if (isControllable)	// don't move player at all if not controllable.
		{
			// Apply gravity
			//bool jumpButton = Input.GetButton("Jump");

			// When we reach the apex of the jump we send out a message
//			if (jumping && !jumpingReachedApex && verticalSpeed <= 0.0) {
//				jumpingReachedApex = true;
//				SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
//			}
		
			if (IsGrounded ())
				verticalSpeed = 0.0F;
			else
				verticalSpeed -= gravity * Time.deltaTime;
		}
	}

//	float CalculateJumpVerticalSpeed (float targetJumpHeight)
//	{
//		// From the jump height and gravity we deduce the upwards speed 
//		// for the character to reach at the apex.
//		return Mathf.Sqrt(2 * targetJumpHeight * gravity);
//	}

//	void DidJump ()
//	{
//		jumping = true;
//		jumpingReachedApex = false;
//		lastJumpTime = Time.time;
//		lastJumpStartHeight = transform.position.y;
//		lastJumpButtonTime = -10;
//		
//		_characterState = CharacterState.Jumping;
//	}

	void OnPauseGame () { 
		isControllable = false; 
	}
	
	void OnResumeGame () {
		isControllable = true; 
	}

	void Update() {
		if (!isControllable) {
			// kill all inputs if not controllable.
			Input.ResetInputAxes();
		}
		float v = Input.GetAxisRaw("Vertical");
		float h = Input.GetAxisRaw("Horizontal");
		bool attack = Input.GetButtonDown ("Fire1");
		bool fire = Input.GetButtonUp ("Fire1");

		if (Input.GetKeyUp (KeyCode.Q)) {
			GameObject [] torches = GameObject.FindGameObjectsWithTag (Tags.torchLight);
			foreach (GameObject torch in torches) {
				Light lights = torch.GetComponent<Light> ();
				lights.enabled  = !lights.enabled;
			}
		}

		if (canFire) {	
			anim.SetBool(hash.attackingBool, attack);
			CheckFire (attack);
			//			if ((Mathf.Abs (horizontal) > 0.1 || Mathf.Abs (vertical) > 0.1) && attack) {
			//				horizontal = (0.5F * horizontal) * (horizontal/Mathf.Abs(horizontal)) ;
			//				vertical = (0.5F * vertical) * (vertical/Mathf.Abs(vertical)) ;
			//			} 
		}

		if (GameMaster.GetCameraMode () == GameMaster.InGameCameraMode.OrbitalController) {
			Time.timeScale = 1;
			anim.SetBool (hash.danceBool, true);
		} else {
			Time.timeScale = (GameMaster.IsPaused()) ? 0 : 1;
			anim.SetBool (hash.danceBool, false);
		} 


		if(h != 0f || v != 0f) 
			// ... set the players rotation and set the speed parameter to 5.5f.
			anim.SetFloat(hash.speedFloat, 5.5f, 0.1f, Time.deltaTime);
		else
			// Otherwise set the speed parameter to 0.
			anim.SetFloat(hash.speedFloat, 0); 

			MovementManagement (v, h);
			// Apply gravity
			// - extra power jump modifies gravity
			// - controlledDescent mode modifies gravity
			ApplyGravity ();
			
			// Apply jumping logic
			//ApplyJumping ();
			
			// Calculate actual motion
			Vector3 movement = moveDirection * moveSpeed + new Vector3 (0, verticalSpeed, 0) + inAirVelocity;
			movement *= Time 	.deltaTime;
			
			// Move the controller
			CharacterController controller = GetComponent<CharacterController>();
			collisionFlags = controller.Move(movement);
			
			// Set rotation to the move direction
			if (IsGrounded())	
				transform.rotation = Quaternion.LookRotation (moveDirection, Vector3.up);
			else {
				Vector3 xzMove = movement;
				xzMove.y = 0;
				if (xzMove.sqrMagnitude > 0.001)
					transform.rotation = Quaternion.LookRotation(xzMove, Vector3.up);
			}
		

		if (fire) 
			FireProjectile ();

		if (!GameMaster.DisplayScore ()) {
			anim.SetBool (hash.deadBool, false);
		}
	}

	void CheckFire (bool attack) {
		Transform fireballSpawnPointTransform = GameObject.Find ("FireBallSpawnPoint").transform;
		if(attack && canFire) {
			hasFired = false;
			RaycastHit hit; // do the exploratory raycast first:
			if (Physics.Raycast(fireballSpawnPointTransform.position, fireballSpawnPointTransform.forward, out hit)){
				float delay = hit.distance / (2000); // calculate the flight time
				Vector3 hitPt = hit.point;
				hitPt.y -= delay * 9.8f; // calculate the bullet drop at the target
				Vector3 dir = hitPt - fireballSpawnPointTransform.position; // use this to modify the shot direction
				// then do the actual shooting:
				//Debug.DrawLine (fireballSpawnPointTransform.position, hit.point, Color.green, 5);
				if (Physics.Raycast(fireballSpawnPointTransform.position, dir, out hit)){
					//Debug.Log ("Sending ray");
					// do here the usual job when something is hit: 
					// apply damage, instantiate particles etc.
				}
			}
		}
	}

	void FireProjectile () {
		Transform fireballSpawnPointTransform = GameObject.Find ("FireBallSpawnPoint").transform;
		if (canFire && !hasFired && Input.GetButtonUp ("Fire1")) {
			GameObject fireballClone = (GameObject) Instantiate (fireballPrefab, fireballSpawnPointTransform.position, fireballSpawnPointTransform.rotation);
			fireballClone.transform.LookAt (fireballSpawnPointTransform.forward);
			fireballClone.rigidbody.AddForce (fireballSpawnPointTransform.forward * 2000, ForceMode.Acceleration);
			hasFired = true;
			//fireTimeout = 0.0F;
		}
	}
	
	void OnControllerColliderHit (ControllerColliderHit hit)
	{
	//	Debug.DrawRay(hit.point, hit.normal);
		if (hit.moveDirection.y > 0.01)
			return;
	}

	float GetSpeed () {
		return moveSpeed;
	}

//	public bool IsJumping () {
//		return jumping;
//	}

	bool IsGrounded () {
		return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
	}

	Vector3 GetDirection () {
		return moveDirection;
	}

	public bool IsMovingBackwards () {
		return movingBack;
	}

	public float GetLockCameraTimer () {
		return lockCameraTimer;
	}

	bool IsMoving ()
	{
		return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5;
	}

//	bool HasJumpReachedApex () {
//		return jumpingReachedApex;
//	}

	bool IsGroundedWithTimeout () {
		return lastGroundedTime + groundedTimeout > Time.time;
	}

	void Reset () {
		gameObject.tag = Tags.player;
	}

	public void LetsDance () {
		// StopController ();
		return;
	}

	private IEnumerator Wait (float secondsToWait) {
		yield return new WaitForSeconds (secondsToWait);
	}
}