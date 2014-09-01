using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
	public float health = 100f;                         // How much health the player has left.
	public float resetAfterDeathTime = 5f;              // How much time from the player dying to the level reseting.
	public AudioClip deathClip;                         // The sound effect of the player dying.
	
	
	private Animator anim;                              // Reference to the animator component.
	private PlayerMovement playerMovement;              // Reference to the player movement script.
	private HashIDs hash;                               // Reference to the HashIDs.
	private SceneFadeInOut sceneFadeInOut;              // Reference to the SceneFadeInOut script.
	private LastPlayerSighting lastPlayerSighting;      // Reference to the LastPlayerSighting script.
	private float timer;                                // A timer for counting to the reset of the level once the player is dead.
	private bool playerDead =false;                            // A bool to show if the player is dead or not.
	
	
	void Awake ()
	{
		// Setting up the references.
	
	}
	
	
	void Update ()
	{
		// If health is less than or equal to 0...
		if(health <= 0f)
		{
			// ... and if the player is not yet dead...
			if(!playerDead)
				// ... call the PlayerDying function.
				PlayerDying();
			else
			{
				// Otherwise, if the player is dead, call the PlayerDead and LevelReset functions.
				PlayerDead();
				LevelReset();
			}
		}
	}
	
	
	void PlayerDying ()
	{

	}
	
	
	void PlayerDead ()
	{
		if (playerDead)
			//TODO
				return;
	}
	
	
	void LevelReset ()
	{

	}
	
	
	public void TakeDamage (float amount)
	{
		// Decrement the player's health by amount.
		health -= amount;
	}
}