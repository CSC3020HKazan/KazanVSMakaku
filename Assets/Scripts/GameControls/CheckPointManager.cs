using UnityEngine;
using System.Collections.Generic;

public class CheckPointManager : SpawnManager { 

	public CheckPointManager () {
		
	}

	public void AddCheckPoint (Vector3 newSpawnPoint) {
		base.AddSpawnPoint(newSpawnPoint);
	}

	public Vector3 GetLastCheckPoint () {
		if (_spawnPoints.Count > 0)
			return _spawnPoints [_spawnPoints.Count - 1];
		else {
			Debug.Log ("Invalid Call to GetLastCheckPoint");
			return Vector3.zero;
		}
	}
}
