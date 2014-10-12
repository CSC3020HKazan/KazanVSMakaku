using System;
using UnityEngine;

public sealed class GameMaster
{
	public GameObject[] PLAYERS = {null, null, null};
	public Vector3[] playerSightings;


	public class Physics {
		public const float GRAVITY = 15;
	}

	public class Pickups {
		public const float MAX_MANA_REWARD = 50f;
		public const float MAX_HEALTH_REWARD = 50f;

	}

	private class PlayerStatistics {
		// Total time played 
		// enemies destroyed
		
	}
	public const int CHECKPOINT_FREQUENCY = 4;

	private static readonly GameMaster _instance= new GameMaster ();
	private GameMaster() { playerSightings = new Vector3[PLAYERS.Length]; }
	
	public static GameMaster Instance {
		get {
			return _instance; 
		}
	}

	public bool AddPlayer (GameObject gameObj, int index) {
		if (index > 3 || index < 0)
			return false;
		else {
			if (PLAYERS[index] == null) {
				PLAYERS[index] = gameObj;	
				return true;
			} else 
				return false;
		} 
	}

	public bool IsPlayer (GameObject gameObj) {
		if (gameObj.tag == Tags.player || gameObj.tag == Tags.playerOne || gameObj.tag == Tags.playerTwo) {
			return true;
		}
		else 
			return false; 
	}
}