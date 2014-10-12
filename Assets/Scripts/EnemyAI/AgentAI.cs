using UnityEngine;
using System.Collections;

/// Implements simple patrolling and flocking

public class AgentAI : MonoBehaviour {
	public float patrolSpeed = 12f;
	public float attackSpeedupFactor = 1.2f;
	public Transform[] patrolWayPoints;
	public float patrolWaitTime  = 2f;
	public float shootingDistance = 12f;
	public float meleeAttackDistance = 12f;
	public Transform fireballSpawnPoint;
	public GameObject arsenal;
	public float firingForce;

	[HideInInspector]
	protected NavMeshAgent _agent;
	protected AgentSight _sight;
	protected PlayerHealth _health;

	private float _shootingTimer = 0;
	private Vector3 _meleeAttackSnapshot;
	private float _patrolTimer = 0f;
	private int _wayPointIndex = 0;

	protected virtual void InitialiseObjects ()  { } // TO BE Implemented by children

	void Start () {
		gameObject.tag = Tags.enemy;
		gameObject.name = Tags.enemy;
		_agent = gameObject.GetComponent<NavMeshAgent> ();
		if (_agent == null) {
			Debug.Log ("No NavMeshAgent Component Attached!");
			_agent = gameObject.AddComponent<NavMeshAgent> ();
		}

		_health = gameObject.GetComponent<PlayerHealth> ();
		if (_health == null) {
			Debug.Log ("No PlayerHealth Component Attached!");
			_health = gameObject.AddComponent<PlayerHealth> ();
			_health.numberOfLives = 1;
			_health.initialHealth = 10;
		} 

		_sight = gameObject.GetComponent<AgentSight> ();
		if (_sight == null) {
			Debug.Log ("No AgentSight Component Attached!");
			_sight = gameObject.AddComponent<AgentSight> ();
		}
		InitialiseObjects();

	}

	void Update () {
		PerformBehaviour();
	}

	protected virtual void PerformBehaviour () {

		if (_sight.playerSighted) {
			_agent.SetDestination (_sight.player.transform.position);
			audio.Play();
			// Debug.Log (_agent.destination) ;
			if (arsenal != null)
				// if (_agent.remainingDistance <= shootingDistance) 
					Shooting ();
			// else {
			// 	if (_agent.remainingDistance <= _agent.stoppingDistance) {
			// 		PerformMeleeAttack () ;
			// 	}
			// }

		}
		else if (patrolWayPoints.Length > 0) {
			audio.Stop ();
			Patrol ();
		}
		else 
			Linger ();
	}

	void Shooting () {
		// Debug.Log (_sight.player.transform.position) ;
		// Debug.Log ("Shooting");
		// _agent.Stop ();
		_shootingTimer += Time.deltaTime;
		// // Debug.Log (_shootingTimer) ;
		ElementBallBehaviour ebb = arsenal.GetComponent<ElementBallBehaviour> ();
		if (ebb == null)	
			return;

		if (_shootingTimer > ebb.coolDownTimer ) {
			if (fireballSpawnPoint == null ) {
				// Component[] children = GetComponentsInChildren<Transform > (); 
				// foreach (Transform go in children) {
				// 	if (go.name == "FireBallSpawnPoint") {
				// 		fireballSpawnPoint = go;
				// 		break;
				// 	}
				// }
				// if (fireballSpawnPoint = null)
				// 	return;
				// Debug.Log ("No Spawn point");
				return;
			}
			if(_agent.remainingDistance <= shootingDistance ) {
				transform.LookAt (_sight.player.transform);
				GameObject fireballClone = (GameObject) Instantiate (arsenal, fireballSpawnPoint.position, fireballSpawnPoint.rotation);
				fireballClone.transform.LookAt(fireballSpawnPoint.forward);
				fireballClone.rigidbody.AddForce (fireballSpawnPoint.forward * firingForce, ForceMode.Acceleration);
				// Debug.Log ("AgentAI::Instantiate") ;
				_shootingTimer = 0.0f;
			}
		}
	}

	void PerformMeleeAttack () {
		Debug.Log ("PerformMeleeAttack");
	}

	void Patrol () {
		// Debug.Log ("Patrol");
		_agent.speed = patrolSpeed;
		if (patrolWayPoints.Length == 0) {
			Debug.Log ("No Waypoints to patrol. Entering Guard Mode!" );
			return;
		}
		if(_agent.remainingDistance < _agent.stoppingDistance) {
			_patrolTimer += Time.deltaTime;
			// Debug.Log(_patrolTimer);
			if(_patrolTimer >= patrolWaitTime) {
				if(_wayPointIndex == patrolWayPoints.Length - 1)
					_wayPointIndex = 0;
				else
					_wayPointIndex++;
				
				_patrolTimer = 0;
			}
		}
		else
			_patrolTimer = 0;
		// Debug.Log (_wayPointIndex);
		Vector3 target = patrolWayPoints[_wayPointIndex].position;
		_agent.SetDestination (target);
	}

	void Linger () {
		Debug.Log ("Linger");
	}

	void OnCollisionEnter (Collision other) {
		ElementBallBehaviour ballBehaviour = other.gameObject.GetComponent<ElementBallBehaviour> ();
		if (ballBehaviour != null) {
			// Debug.Log (_health.GetCurrentHealth());
			if (_health.IsAlive ()) {
				// Debug.Log ("OnCollisionEnter::ElementBallBehaviour");
				_health.TakeDamage (ballBehaviour.GetPotency());
				int deltaExp = (int)(_health.GetInitialHealth() / ballBehaviour.GetPotency ());
				ballBehaviour.owner.AddExperience (deltaExp);
			}
		} 
	}
}
