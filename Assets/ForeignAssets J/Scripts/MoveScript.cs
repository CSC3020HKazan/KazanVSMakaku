using UnityEngine;

//Move script for all object
public class MoveScript : MonoBehaviour
{
	//speed and direction vector
	public Vector3 speed = new Vector3(0, 0,0);

	public Vector3 direction = new Vector3(0,0, 0);

	//final movement vector that will be used to move object
	private Vector3 movement;
	
	void Update()
	{
		//movement = speed*direction
		movement = new Vector3(
			speed.x * direction.x,
			speed.y * direction.y,
			speed.z * direction.z);
	}
	//move
	void FixedUpdate()
	{
		rigidbody.velocity = movement;
	}

	//sets the direction of object to some vector
	public void setDirection(Vector3 v)
	{
		direction = v;
	}
}