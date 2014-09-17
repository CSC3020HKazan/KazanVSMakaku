using UnityEngine;

//Particle Singleton
public class ParticleHelper : MonoBehaviour
{
	public static ParticleHelper Instance;
	
	//particles
	public ParticleSystem HitEffect1;
	public ParticleSystem HitEffect2;
	public ParticleSystem ExplosionEffect;
	public ParticleSystem CollisionEffect;
	
	void Awake()
	{
		// Register the singleton
		if (Instance != null)
		{
			Debug.LogError("Multiple instances of ParticleHelper!");
		}
		Instance = this;
	}

	//if hit by bullet
	public void Hit1(Vector3 position)
	{
		//instantiate particle
		instantiate(HitEffect1, position);
		
	}

	//if hit by bullet
	public void Hit2(Vector3 position)
	{
		//instantiate particle
		instantiate(HitEffect2, position);
		
	}

	//if hit by bullet
	public void Explosion(Vector3 position)
	{
		//instantiate particle
		instantiate(ExplosionEffect, position);
		
	}

	//if player collides with object
	public void Col(Vector3 position)
	{
		//instantiate particle
		instantiate(CollisionEffect, position);
		
	}

	private void instantiate(ParticleSystem prefab, Vector3 position)
	{
		//instantiate the particle system
		ParticleSystem newParticleSystem = Instantiate(prefab,position,Quaternion.identity
		                                               ) as ParticleSystem;
		
		// Destroy it after it's lifetime
		Destroy(
			newParticleSystem.gameObject,
			newParticleSystem.startLifetime
			);

	}
}