using UnityEngine;
using System.Collections;

public class ThirdPersonCam : MonoBehaviour {

	//speed of rotation
	public float rotationSpeed = 5;
	
	//minimum and maximum vertical rotation value
	public float minY = -15;
	public float maxY = 15;
	
	//keep track of current rotation
	float currentRotation = 0;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
		//requested rotation
		float xRotation = Input.GetAxis("Mouse Y")*rotationSpeed;
		
		//needs to be in range of maximum/minimum rotation
		if(currentRotation + xRotation <= maxY && currentRotation + xRotation >= minY)
		{
			//rotate
			currentRotation += xRotation;
			transform.Rotate(new Vector3(-xRotation, 0, 0));	
		}

	}
}
