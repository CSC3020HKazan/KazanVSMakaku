using UnityEngine;
using System.Collections;

public class AnimatorSetup
{
	public float speedDampTime = 0.1f;              // Damping time for the Speed parameter.
	public float angularSpeedDampTime = 0.7f;       // Damping time for the AngularSpeed parameter
	public float angleResponseTime = 0.6f;          // Response time for turning an angle into angularSpeed.
	
	
	private Animator anim;                          // Reference to the animator component.
	private HashIDs hash;                           // Reference to the HashIDs script.
	
	
	// Constructor
	public AnimatorSetup(Animator animator, HashIDs hashIDs)
	{

	}
	
	
	public void Setup(float speed, float angle)
	{

	}   
}