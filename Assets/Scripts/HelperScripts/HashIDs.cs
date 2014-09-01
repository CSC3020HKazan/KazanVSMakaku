using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour
{
	// Here we store the hash tags for various strings used in our animators.
	public int dyingState;
	public int locomotionState;
	public int danceState;
	public int attackingState;
	public int idleState;
	public int deadBool;
	public int speedFloat;
	public int attackingBool;
	public int gettingHitBool;
	public int playerInSightBool;
	public int shotFloat;
	public int aimWeightFloat;
	public int angularSpeedFloat;
	public int openBool;
	public int danceBool;
	
	
	void Awake ()
	{
		dyingState = Animator.StringToHash("Base Layer.Die");
		locomotionState = Animator.StringToHash("Base Layer.Locomotion");
		attackingState = Animator.StringToHash("Base Layer.Attack");
		danceState = Animator.StringToHash ("Base Layer.Dance");
		idleState = Animator.StringToHash ("Base Layer.Idle");
		deadBool = Animator.StringToHash("Dead");
		speedFloat = Animator.StringToHash("Speed");
		attackingBool = Animator.StringToHash("Attacking");
		gettingHitBool = Animator.StringToHash("GettingHit");
		playerInSightBool = Animator.StringToHash("PlayerInSight");
		shotFloat = Animator.StringToHash("Shot");
		aimWeightFloat = Animator.StringToHash("AimWeight");
		angularSpeedFloat = Animator.StringToHash("AngularSpeed");
		openBool = Animator.StringToHash("Open");
		danceBool = Animator.StringToHash ("Dance");
	}
}