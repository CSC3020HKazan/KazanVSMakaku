using UnityEngine;
using System.Collections.Generic;

public class FlockAgent :  AgentAI {

	public float dampingSpeed = 0.1f;
	public float leaderDirectionContribution = 0.7f;
	[HideInInspector] 
	public Vector3 centerOfMass;
	[HideInInspector]
	public GameObject flockController;

	private Vector3 _separation = Vector3.zero;
	private Vector3 _alignment = Vector3.zero;
	private Vector3 _cohesion = Vector3.zero;
	private List<GameObject> _neighbours;

	protected override void InitialiseObjects () {
		_neighbours  = new List<GameObject> ();
		flockController = null;
		leaderDirectionContribution = Mathf.Clamp (leaderDirectionContribution , 0, 1.0f);
		gameObject.name = "FlockAgent";
	}

	protected override void PerformBehaviour () {
		if (flockController == null ) {
			base.PerformBehaviour ();
		}
		else {
			// base.PerformBehaviour();
			// _agent.Stop ();
			// Debug.Log ("FlockAgent::PerformBehaviour") ;

			NavMeshAgent leader = flockController.GetComponent<NavMeshAgent> ();
			_agent.SetDestination (leader.destination);
			_agent.speed = flockController.GetComponent<FlockControllerAgent> ().flockingSpeed;
			if (_sight.playerSighted)
				return;
				// ba1se.PerformBehaviour ();
			// FollowTheLeader () ;

		}

		// Debug.DrawLine (transform.position, transform.position + _agent.velocity , Color.red ,  0.4f) ;
	}

	private void FollowTheLeader () {
		float angle = 0;
		ComputeFlockingParameters (ref _cohesion, ref _alignment, ref _separation, ref angle);
		FlockControllerAgent leader = flockController.GetComponent<FlockControllerAgent> ();
		Vector3 deltaVelocity = Vector3.zero;
		_agent.speed = 0;
		deltaVelocity = leader.separationWeight * _separation +leader.alignmentWeight * _alignment + leader.cohesionWeight * _cohesion;
			// Debug.Log ("FollowTheLeader" +_neighbours.Count);
		deltaVelocity = leader.flockingSpeed * Vector3.Normalize (deltaVelocity) ;

		// Vector3 desiredVelocity = _agent.velocity + deltaVelocity;
		//Vector3 aVelocity = _agent.velocity;
		// _agent.velocity = Vector3.SmoothDamp (_agent.velocity, desiredVelocity, ref _dampingVelocity, dampingSpeed);
		// _agent.velocity = _agent.velocity + deltaVelocity;
		// _agent.velocity = leader.flockingSpeed * _agent.velocity;
		// _agent.velocity.Normalize (); 
		// _agent.velocity = _agent.speed * _agent.velocity;
		// Debug.DrawLine (transform.position, transform.position + desiredVelocity , Color.green ,  0.4f) ;

		// _agent.speed = leader.flockingSpeed;
		// transform.rotation = Quaternion.Euler (new Vector3 (transform.eulerAngles.x, angle + 90, transform.eulerAngles.z));
		transform.rotation = flockController.transform.rotation;
	}

	void ComputeFlockingParameters (ref Vector3 cohere, ref Vector3 align, ref Vector3 disperse, ref float eulerAngle) {
		align =Vector3.zero;
		if (flockController) {	
			// cohere = flockController.GetComponent<FlockControllerAgent>().centerOfMass - transform.position;
			// disperse =  Vector3.Normalize (transform.position - flockController.transform.position);

		} else {
			// cohere = transform.position;
			// disperse = Vector3.zero;
		}
		cohere = Vector3.zero;
		eulerAngle = Mathf.Atan2 (transform.position.x - flockController.transform.position.x, 
			transform.position.z - flockController.transform.position.z) * Mathf.Rad2Deg;
		if (_neighbours.Count > 0)
			eulerAngle = eulerAngle * leaderDirectionContribution;
		disperse = Vector3.zero;
		foreach ( GameObject neighbour in _neighbours ) {
			cohere += neighbour.transform.position;
			NavMeshAgent agent = neighbour.GetComponent<NavMeshAgent> ();
			align += agent.velocity;
			disperse += neighbour.transform.position - transform.position;
			eulerAngle += (1 - leaderDirectionContribution) * neighbour.transform.eulerAngles.y * Mathf.Rad2Deg;
		} 
		if (_neighbours.Count > 0 ) {
			align /= _neighbours.Count;
			disperse /= _neighbours.Count;
			cohere /= _neighbours.Count;
			eulerAngle /= _neighbours.Count + 1;
		}
		align = Vector3.Normalize (align);
		disperse *= -1;
		disperse = Vector3.Normalize(disperse) ;
		cohere = cohere - transform.position;
		cohere = Vector3.Normalize (cohere);
	}

	void OnTriggerEnter ( Collider other ) {
		FlockAgent otherFlockAgent = other.gameObject.GetComponent<FlockAgent> ();
		// Debug.Log ("FlockAgent::OnTriggerStay");
		
		if (otherFlockAgent != null) {

			if (flockController != null ) {
				FlockControllerAgent controllerAgent = flockController.GetComponent<FlockControllerAgent>();
				if (controllerAgent.CheckForSpace()  && (otherFlockAgent.flockController == null || otherFlockAgent.flockController == flockController) )   {
					controllerAgent.AddRecruit(other.gameObject);
					_neighbours.Add (other.gameObject);
				}
			}
		}
	}
}