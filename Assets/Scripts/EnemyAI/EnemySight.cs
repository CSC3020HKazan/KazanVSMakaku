using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour {
	public float fieldOfViewAngle = 110f;           // Number of degrees, centred on forward, for the enemy see.
	public bool playerInSight;                      // Whether or not the player is currently sighted.
	public Vector3 personalLastSighting;            // Last place this enemy spotted the player.
	
	
	private NavMeshAgent nav;                       // Reference to the NavMeshAgent component.
	private SphereCollider col;                     // Reference to the sphere collider trigger component.
	private Animator anim;                          // Reference to the Animator.
	private LastPlayerSighting lastPlayerSighting;  // Reference to last global sighting of the player.
	private GameObject player;                      // Reference to the player.
	private Animator playerAnim;                    // Reference to the player's animator component.
	private PlayerHealth playerHealth;              // Reference to the player's health script.
	private HashIDs hash;                           // Reference to the HashIDs.
	private Vector3 previousSighting;               // Where the player was sighted last frame.
	
	
	void Awake ()
	{

	}
	
	
	void Update ()
	{

	}
	
	
	void OnTriggerStay (Collider other)
	{

	}
	
	
	void OnTriggerExit (Collider other)
	{

	}
	
	
	float CalculatePathLength (Vector3 targetPosition)
	{
		return 1.0f;
	}
}
