using UnityEngine;
using System.Collections.Generic;

public class SpawnManager { 
	protected List<Vector3> _spawnPoints; 

	public SpawnManager () {
		_spawnPoints = new List<Vector3>();
	}

	public void AddSpawnPoint (Vector3 newSpawnPoint) {
		_spawnPoints.Add (newSpawnPoint);
	}
}
