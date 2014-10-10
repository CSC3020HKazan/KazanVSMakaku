using UnityEngine;
using System.Collections;

/// Implements simple patrolling and flocking

public class AgentAI : MonoBehaviour {
	public float patrolSpeed = 12f;
	public Transform[] patrolWayPoints;
	public float patrolWaitTime  = 2f;

	[HideInInspector]
	protected NavMeshAgent _agent;
	protected AgentSight _sight;
	private float _patrolTimer = 0f;
	private int _wayPointIndex = 0;

	protected virtual void InitialiseObjects ()  { } // TO BE Implemented by children

	void Start () {
		_agent = gameObject.GetComponent<NavMeshAgent> ();
		if (_agent == null) {
			Debug.Log ("No NavMeshAgent Component Attached!");
			_agent = gameObject.AddComponent<NavMeshAgent> ();
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
		if (_sight.playerSighted)
			_agent.SetDestination (_sight.player.transform.position);
		else if (patrolWayPoints.Length > 0) 
			Patrol ();
	}

	void Patrol () {
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
		
		_agent.SetDestination (patrolWayPoints[_wayPointIndex].position);
	}
}