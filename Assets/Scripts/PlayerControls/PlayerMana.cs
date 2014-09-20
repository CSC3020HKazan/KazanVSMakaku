using UnityEngine;
using System.Collections;

public class PlayerMana : MonoBehaviour
{
	public float initialMana = 100f;                    
	private float replenishRate = 3f;

	private Animator anim;
	private HashIDs hash;
	
	private float _currentMana;
	//private bool _isDepleted = false;
	void Start () {
		_currentMana = initialMana;
	}
	
	
	void Update () {
		if (_currentMana < initialMana ) {
			ReplenishMana (Time.deltaTime * replenishRate);
		}
	}

	public void ExhaustMana (float amount) {
		// Decrement the player's health by amount.
		if (amount > 0)
			_currentMana -= amount;
		else 
			ReplenishMana (-amount); 
	}

	public void ReplenishMana (float amount) {
		if (amount > 0 ) {
			_currentMana += amount;
			Mathf.Clamp (_currentMana, 0, initialMana); 
		} else 
			ExhaustMana (-amount); 
	}

	public float GetCurrentMana () {
		return _currentMana;
	}

	public float GetInitialMana () {
		return initialMana;
	}

	public bool IsDepleted () {
		return _currentMana <= 0;
	}
}