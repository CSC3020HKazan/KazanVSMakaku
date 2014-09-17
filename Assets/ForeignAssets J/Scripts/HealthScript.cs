using UnityEngine;
using System.Collections;

//health script
public class HealthScript : MonoBehaviour {

	//health of object
	public int maxHealth = 5;
	public int currentHealth;
	// Use this for initialization
	void Start () {
		//currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {

	}

	//decreases health
	public void decreaseHealth(int damage)
	{
		//take damage
		currentHealth -= damage;

		//if health below or equal to 0
		if(currentHealth <= 0)
		{
			destroyObj();
		}
	}

	//increase health
	public void increaseHealth(int heal)
	{
		//heal
		currentHealth += heal;

		//no more than current health
		if(currentHealth > maxHealth)
		{
			currentHealth = maxHealth;
		}
	}

	//return health
	public int getHealth(){return currentHealth;}

	//destroys object
	public void destroyObj()
	{
		// play destruction particle and soun
		//SoundScript.instance.playExplosionSound(transform.position);
		//ParticleHelper.Instance.Explosion(transform.position);
		
		// destroy object count in masterscript
		//MasterScript.DecreaseObjCount();
		
		//TextHelper.Instance.instantiateText2(transform.position,MasterScript.getObjCountText());
		
		// destroy gameobject
		Destroy(gameObject);
	}
}
