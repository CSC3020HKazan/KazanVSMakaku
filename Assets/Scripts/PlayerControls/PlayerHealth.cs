using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
	public float initialHealth = 100f;                         // How much health the player has left.
	public AudioClip deathClip;                         // The souinitialHealthnd effect of the player dying.
	[SerializeField]
	private ParticleSystem _healingParticleSystem;
	[SerializeField]
	private float respawnAfter = 5f;             // How much time from the player dying to the level reseting.
	[SerializeField]
	private ParticleSystem _damageParticleSystem;

		
	private Animator anim;                              // Reference to the animator component.
	private HashIDs hash;                               // Reference to the HashIDs.
	private float _currentHealth;
	private bool _playerDead = false;
	
	void Start () {
		// Setting up the references.
		_currentHealth = initialHealth;
	}
	
	
	void Update () {
		// If health is less than or equal to 0...
			// ... and if the player is not yet dead...
		if(_currentHealth <= 0 && !_playerDead)
			// ... call the PlayerDying function.
			PlayerDying();
		else if (_currentHealth < 0){
			// Otherwise, if the player is dead, call the PlayerDead and LevelReset functions.
			PlayerDead();
			LevelReset();
		}
	}
	
	void PlayerHealing () {

	}

	void PlayerGettingHit () {

	}
	
	void PlayerDying () {

	}	
	
	void PlayerDead () {
		if (_playerDead)
			return;
		_playerDead = true;
	}	
	
	void LevelReset () {

	}

	public void HealPlayer (float amount) {
		if (amount < 0){
			_currentHealth += amount;
			PlayerHealing ();
		} else 
			TakeDamage (-amount);
	} 
	
	public void TakeDamage (float amount) {
		// Decrement the player's health by amount.
		if (amount > 0 ) {
			_currentHealth -= amount;
			PlayerGettingHit ();
		} else 
			HealPlayer (-amount) ;
	}

	public float GetRespawnTimeout () {
		return respawnAfter;
	}

	public float GetCurrentHealth () {
		return _currentHealth;
	}

	public float GetInitialHealth () {
		return initialHealth;
	}

	public bool IsAlive () {
		return _currentHealth > 0;
	}
 
}