using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
	public float initialHealth = 100f;                         // How much health the player has left.
	public int numberOfLives = 9;
	public AudioClip deathClip;                        // The souinitialHealthnd effect of the player dying.
	public GameObject deathEffect;
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
	private bool _wasAlive = false;
	void Start () {
		// Setting up the references.
		_currentHealth = initialHealth;
	}
	
	
	void Update () {
		// If health is less than or equal to 0...
			// ... and if the player is not yet dead...
		if (_playerDead && _wasAlive) {
			PlayerDying ();
		}
		_wasAlive = !_playerDead; 	
	}
	
	void PlayerHealing () {
		return;
	}

	void PlayerGettingHit () {
		return;
	}
	
	void PlayerDying () {
		Destroy (gameObject);
		Instantiate(deathEffect, transform.position, Quaternion.identity);
		return;
	}	
	
	void PlayerDead () {
		if (_playerDead)
			return;
		_playerDead = true;
	}	
	
	void LevelReset () {

	}

	public void HealPlayer (float amount) {
		if (amount >= 0){
			_currentHealth += amount;
			_currentHealth = (_currentHealth > initialHealth)  ? initialHealth : _currentHealth; 
			PlayerHealing ();
		} else 
			TakeDamage (-amount);
		if (_playerDead)
			_playerDead = (_currentHealth == initialHealth) ? false : _playerDead; 
	} 
	
	public void HealPlayer () {
		HealPlayer (initialHealth) ;
	}
	public void TakeDamage (float amount) {
		// Decrement the player's health by amount.

		if (!_playerDead) {
			if (amount >= 0 ) {
				_currentHealth -= amount;
				_currentHealth = (_currentHealth <= 0)  ? 0 : _currentHealth;
				
				PlayerGettingHit ();
			} else 
				HealPlayer (-amount) ;
		}
		if (_currentHealth == 0 ) { 
			_playerDead = true;
			numberOfLives --;
		}
	}

	public void RecordDeath () {
		numberOfLives --;
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

	public int GetLives ( ) {
		return numberOfLives;
	}

	public bool IsAlive () {
		return _currentHealth > 0;
	}
 
}