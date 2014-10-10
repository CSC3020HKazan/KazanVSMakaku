using UnityEngine;
using System.Collections;

public class PlayerMana : MonoBehaviour
{
	public float initialMana = 100f;                    
	private float replenishRate = 3f;

	private Animator anim;
	private HashIDs hash;
	private int _experience = 0;
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
		if (amount == 0)
			return; 
		if (amount > 0)
			_currentMana -= amount;
		else 
			ReplenishMana (-amount); 
	}

	public void ReplenishMana (float amount) {
		if (amount == 0)
			return;
		if (amount > 0 ) {
			_currentMana += amount;
			if (_currentMana > initialMana) _currentMana = initialMana; 
		} else 
			ExhaustMana (-amount); 
	}

	public void AddExperience (int amount) {	
		if (amount > 0)
			_experience += amount; 
	}

	public void Reset () {
		_currentMana = initialMana;
	}

	public float GetCurrentMana () {
		return _currentMana;
	}

	public int GetExperience () {
		return _experience;
	}

	public float GetInitialMana () {
		return initialMana;
	}

	public bool IsDepleted () {
		return _currentMana <= 0;
	}
}