using UnityEngine;
using System.Collections.Generic;

public class EnemyWayPointManager : SpawnManager { 

	public EnemyWayPointManager () {
		
	}

	public void AddWayPoint (Vector3 newSpawnPoint) {
		base.AddSpawnPoint(newSpawnPoint);
	}

	public Vector3 GetLastWayPoint () {
		return _spawnPoints [_spawnPoints.Count - 1];
	}
}