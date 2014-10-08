using UnityEngine;
using System.Collections.Generic;

public class CheckPointManager { 
	private List<CheckPoint> _checkpoints;
	public CheckPointManager () {
		_checkpoints = new List<CheckPoint> ();
	}

	public void AddCheckPoint (CheckPoint newCheckPoint) {
		// base.AddSpawnPoint(newSpawnPoint);
		_checkpoints.Add(newCheckPoint);
	}

	public CheckPoint GetLastCheckPoint () {
		if (_checkpoints.Count > 0)
			return _checkpoints [_checkpoints.Count - 1];
		else {
			Debug.Log ("Invalid Call to GetLastCheckPoint");
			return new CheckPoint();
		}
	}
}

public class CheckPoint {
	public readonly Vector3 position;
	public readonly Quaternion rotation;
	public readonly Vector3 velocity;
	public readonly Vector3 angularVelocity;

	public CheckPoint (Vector3 pos, Quaternion rot , Vector3 vel , Vector3 angVel ) {
		position = pos;
		rotation = rot;
		velocity = vel;
		angularVelocity = angVel;
	}

	public CheckPoint (Vector3 pos, Quaternion rot , Vector3 vel) {
		position = pos;
		rotation = rot;
		velocity = vel;
		angularVelocity = Vector3.zero;
	}


	public CheckPoint (Vector3 pos, Quaternion rot) {	
		position = pos;
		rotation = rot;
		velocity = Vector3.zero;
		angularVelocity = Vector3.zero;
	}

	public CheckPoint (Vector3 pos)  {
		position = pos;
		rotation = Quaternion.identity;
		velocity = Vector3.zero;
		angularVelocity = Vector3.zero;
	}

	public CheckPoint () {
		position = Vector3.zero;
		rotation = Quaternion.identity;
		velocity = Vector3.zero;
		angularVelocity = Vector3.zero;
	}
}

