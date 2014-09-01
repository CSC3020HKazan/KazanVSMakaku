using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
	public float patrolSpeed = 2f;                          // The nav mesh agent's speed when patrolling.
	public float chaseSpeed = 5f;                           // The nav mesh agent's speed when chasing.
	public float chaseWaitTime = 5f;                        // The amount of time to wait when the last sighting is reached.
	public float patrolWaitTime = 1f;                       // The amount of time to wait when the patrol way point is reached.
	public Transform[] patrolWayPoints;                     // An array of transforms for the patrol route.
	
	
	private EnemySight enemySight;                          // Reference to the EnemySight script.
	private NavMeshAgent nav;                               // Reference to the nav mesh agent.
	private Transform player;                               // Reference to the player's transform.
	private PlayerHealth playerHealth;                      // Reference to the PlayerHealth script.
	private LastPlayerSighting lastPlayerSighting;          // Reference to the last global sighting of the player.
	private float chaseTimer;                               // A timer for the chaseWaitTime.
	private float patrolTimer;                              // A timer for the patrolWaitTime.
	private int wayPointIndex;                              // A counter for the way point array.
	
	
	void Awake ()
	{
	}
	
	
	void Update ()
	{

	}

	void EnemyBehaviour () {
//		// If the player is in sight and is alive...
//		if(enemySight.playerInSight && playerHealth.health > 0f)
//			// ... shoot.
//			Shooting();
//		
//		// If the player has been sighted and isn't dead...
//		else if(enemySight.personalLastSighting != lastPlayerSighting.resetPosition && playerHealth.health > 0f)
//			// ... chase.
//			Chasing();
//		
//		// Otherwise...
//		else
//			// ... patrol.
//			Patrolling();


	}
	
	void Shooting ()
	{

	}
	
	
	void Chasing ()
	{

	}
	
	
	void Patrolling ()
	{

	}
}
