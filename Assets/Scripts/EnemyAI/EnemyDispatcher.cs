using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyDispatcher : MonoBehaviour {
	public float dispatchRate = 2.0f;
	public GameObject enemyUnit;
	
	private Transform _spawnPoint;
	private EnemyWayPointManager _wayPointManager;

}