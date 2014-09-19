using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// poor implementation of number of players

public class BasePlatform : MonoBehaviour {
	[HideInInspector]
	public const int NUM_PLAYERS = 2;
	
	private bool [] _playerIsPresent = new bool[NUM_PLAYERS + 1];

	protected virtual void InitialiseTag () {
		gameObject.tag = Tags.platform;
		gameObject.name = Tags.platform;
	} 

	void Start () {
		InitialiseTag ();
		for (int i = 0 ; i < NUM_PLAYERS ; i++ )
			_playerIsPresent[i] = false;
	}

	public void SetPlayerPresence (bool isPlayerPresent)  {
		SetPlayerPresence (isPlayerPresent, 0);
	}

	public void SetPlayerPresence (bool isPlayerPresent, int playerIndex)  {
		if (playerIndex < 0 || playerIndex > NUM_PLAYERS - 1 )
			Debug.Log ("Player Index out of range") ;
		else 
			_playerIsPresent[playerIndex] = isPlayerPresent;
	}

	protected bool IsPlayerPresent () {
		for ( int i = 0 ; i < NUM_PLAYERS; i ++ )
			if (_playerIsPresent[i])
				return true;
		return false;
	}

}