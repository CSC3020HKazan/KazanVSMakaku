using UnityEngine;
using System.Collections;

public class EnemyAnimation : MonoBehaviour
{
	public float deadZone = 5f;             // The number of degrees for which the rotation isn't controlled by Mecanim.
	
	
	private Transform player;               // Reference to the player's transform.
	private EnemySight enemySight;          // Reference to the EnemySight script.
	private NavMeshAgent nav;               // Reference to the nav mesh agent.
	private Animator anim;                  // Reference to the Animator.
	private HashIDs hash;                   // Reference to the HashIDs script.
	private AnimatorSetup animSetup;        // An instance of the AnimatorSetup helper class.
	
	
	void Awake ()
	{

	}
	
	
	void Update () 
	{
		// Calculate the parameters that need to be passed to the animator component.
		NavAnimSetup();
	}
	
	
	void OnAnimatorMove ()
	{
	}
	
	
	void NavAnimSetup ()
	{

	}
	
	
	float FindAngle (Vector3 fromVector, Vector3 toVector, Vector3 upVector)
	{
		if(toVector == Vector3.zero)
			return 0f;

		float angle = Vector3.Angle(fromVector, toVector);
		Vector3 normal = Vector3.Cross (fromVector, toVector);	
		angle *= Mathf.Sign(Vector3.Dot(normal, upVector));
		angle *= Mathf.Deg2Rad;
		
		return angle;
	}
}
