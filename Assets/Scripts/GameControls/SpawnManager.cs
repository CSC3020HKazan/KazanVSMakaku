using UnityEngine;
using System.Collections.Generic;

public class SpawnManager { 
	private List<Vector3> _spawnPoints; 

	public SpawnManager () {
		_spawnPoints = new List<Vector3>();
	}

	public void AddSpawnPoint (Vector3 newSpawnPoint) {
		_spawnPoints.Add (newSpawnPoint);
	}

	public Vector3 SpawnAtLastCheckPoint () {
		return _spawnPoints [_spawnPoints.Count - 1];
	}
}
