using UnityEngine;
using System.Collections.Generic;

public class FlockControllerAgent :  AgentAI {
	
	public float cohesionWeight = 1.0f;
	public float alignmentWeight = 1.0f;
	public float separationWeight =  1.0f;
	public int flockSize = 12;
	public float flockingSpeed = 8;
	[HideInInspector] 
	public Vector3 centerOfMass;
	[HideInInspector]
	public Vector3 target;

	private List<GameObject> _flock;

	protected override void InitialiseObjects () { 
		_flock = new List<GameObject> (flockSize) ;
		gameObject.name =  "FlockControllerAgent";
	}

	protected override void PerformBehaviour () {
		ComputeCenterOfMass ();
		if (_sight.playerSighted) {
			foreach (GameObject follower in _flock) {
				follower.GetComponent<FlockAgent> ().flockController  = null;
			}
			_flock.Clear ();
		}
		base.PerformBehaviour ();

	}

	private void ComputeCenterOfMass () {
		Vector3 result = gameObject.transform.position;
		foreach (GameObject flockAgent in _flock ) {
			result += flockAgent.transform.position;
		}
		centerOfMass = result / (_flock.Count + 1);
	}

	public bool CheckForSpace () {
		// Debug.Log ("CheckForSpace");
		return (_flock.Count - 1 < flockSize );
	}

	public void AddRecruit (GameObject newRecruit) {
		_flock.Add (newRecruit);
	}

	public void ReleaseRecruit (GameObject currentRecruit) {
		_flock.Remove (currentRecruit);
	}

	void OnTriggerEnter ( Collider other ) {	
		// Debug.Log ("FlockControllerAgent::OnTriggerEnter");
		FlockAgent otherFlockAgent = other.gameObject.GetComponent<FlockAgent> ();
		if (otherFlockAgent != null) {
			if (this.CheckForSpace() && otherFlockAgent.flockController == null) {
				this.AddRecruit(other.gameObject);
				otherFlockAgent.flockController = gameObject;
				// _neighbours.Add (other.gameObject);
			}
		}
	}
}