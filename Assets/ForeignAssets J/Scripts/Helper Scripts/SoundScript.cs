using UnityEngine;
using System.Collections;

public class SoundScript : MonoBehaviour {

	//sound instance
	public static SoundScript instance;

	//sound effects
	public AudioClip gunSound;
	public AudioClip explosion;
	public AudioClip health;
	public AudioClip mana;

	//singleton
	void Awake()
	{
		if(instance != null)
		{
			Debug.LogError("More than 1 instance of sound script");
		}
		instance = this;
	}

	//play gun sound at pos
	public void playGunSound(Vector3 pos)
	{
		makeSound(gunSound,pos,0.5f);
	}

	//play explosion sound at pos
	public void playExplosionSound(Vector3 pos)
	{
		makeSound(explosion,pos,2f);
	}
	public void playHealthSound(Vector3 pos)
	{
		makeSound(health,pos,2f);
	}
	public void playManaSound(Vector3 pos)
	{
		makeSound(mana,pos,2f);
	}
	//play a specific sound at pos and with volume vol
	public void makeSound(AudioClip clip, Vector3 pos , float vol)
	{
		AudioSource.PlayClipAtPoint(clip,pos,vol);
	}
}
