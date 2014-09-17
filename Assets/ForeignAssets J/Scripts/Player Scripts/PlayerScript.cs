using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	//speed and direction vector
	public Vector3 speed;
	private Vector3 direction;

	//rotation speed
	public float rotateSpeed = 5;

	//jump value
	public float jumpValue = 5;

	//final movement vector that will be used to move object
	private Vector3 movement;

	//current orientation of player
	public float currentRotation = 0;

	//player rotation around y axis
	private float yRotation;
	
	private float verticalVelocity = 0;

	//modifier when pressing run
	public float runSpeedModifier = 0;

	private float run;

	private float maxSpeed;

	//maximum life(chane to respawn)
	public int maxLife = 3;
	

	//character controller
	CharacterController cc;

	
	void Awake()
	{
		//fetches character controller
		cc = GetComponent<CharacterController>();

		speed.y = 0;

		//calculates the max speed of character
		if(speed.x > speed.z)
		{
			maxSpeed = speed.x;
		}
		else maxSpeed = speed.z;

	}

	void Update()
	{
		if(cc.isGrounded) run = Run();
		//movement = speed*direction
		movement = new Vector3(run * speed.x * Input.GetAxis("Horizontal"),
		                       0, 
		                       run * speed.z * Input.GetAxis("Vertical"));
		
		
		//clamp velocity so diagonal movement isn't faster than straight movement
		movement = Vector3.ClampMagnitude (movement, maxSpeed*run);
		
		//jump movement should not be clamped
		movement.y = getVerticalMovement();
		
		//move in local coordinates
		movement = transform.rotation * movement;
		
		//move
		cc.Move(movement * Time.deltaTime);

		//side rotation
		yRotation = Input.GetAxis("Mouse X")* rotateSpeed;
		currentRotation += yRotation;
		transform.Rotate(new Vector3(0, yRotation, 0));	
	}
	

	//check if player should run
	float Run()
	{
		if(Input.GetKey(KeyCode.LeftShift))
		{
			return runSpeedModifier;
		}
		return 1;
	}

	//gets the vertical movement (jumping
	float getVerticalMovement()
	{
		//if character is on ground
		if(cc.isGrounded)
		{
			if(Input.GetKeyDown("space"))
			{
				//increase vertical velocity to jump value
				verticalVelocity = jumpValue;


				return verticalVelocity;
			}


			return -0.5f;
		}

		//else apply gravity
		verticalVelocity += Physics.gravity.y*Time.deltaTime;
		return verticalVelocity;
	}

	public Vector3 getMovement()
	{
		return movement;
	}
	public float getMaxSpeed()
	{
		return maxSpeed;
	}
}
