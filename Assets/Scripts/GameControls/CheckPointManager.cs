using UnityEngine;
using System.Collections.Generic;

public class CheckPointManager : SpawnManager { 

	public CheckPointManager () {
		
	}

	public void AddCheckPoint (Vector3 newSpawnPoint) {
		base.AddSpawnPoint(newSpawnPoint);
	}

	public Vector3 GetLastCheckPoint () {
		return _spawnPoints [_spawnPoints.Count - 1];
	}
}
